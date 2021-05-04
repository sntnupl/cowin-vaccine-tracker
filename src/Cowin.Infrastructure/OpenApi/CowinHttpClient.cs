using Cowin.Domain.Abstractions;
using Cowin.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cowin.Infrastructure.OpenApi
{
    public class CowinHttpClient : ICowinHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CowinHttpClient> logger;
        private const string pathAllStates = "admin/location/states";
        private const string pathDistrictsInState = "admin/location/districts";
        private const string pathCalendarByDistrict = "appointment/sessions/public/calendarByDistrict";
        private const string pathCalendarByPin = "appointment/sessions/public/calendarByPin";

        public CowinHttpClient(
            HttpClient httpClient,
            ILogger<CowinHttpClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<AllStates> GetAllStates()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, pathAllStates);
            request.Headers.Add("Accept", "application/json");

            try {
                using (var response = await httpClient.SendAsync(request)) {
                    if (!response.IsSuccessStatusCode) {
                        logger.LogError($"Error fetching all states. Http Response Code: {response.StatusCode}");
                        return AllStates.EmptyObject();
                    }

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var allStates = JsonSerializer.Deserialize<AllStates>(responseJson, new JsonSerializerOptions {
                        AllowTrailingCommas = true,
                        IgnoreNullValues = true,
                    });

                    return allStates;
                }
            }
            catch(Exception ex) {
                logger.LogError(ex, "Exception encountered while fetching all states.");
                return null;
            }
        }


        public async Task<DistrictsInState> GetAllDistrictsInState(State matchingState)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{pathDistrictsInState}/{matchingState.Id}");
            request.Headers.Add("Accept", "application/json");

            try {
                using (var response = await httpClient.SendAsync(request)) {
                    if (!response.IsSuccessStatusCode) {
                        logger.LogError($"Error fetching districts in state {matchingState}. Http Response Code: {response.StatusCode}");
                        return DistrictsInState.EmptyObject();
                    }

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var districtsInState = JsonSerializer.Deserialize<DistrictsInState>(responseJson, new JsonSerializerOptions {
                        AllowTrailingCommas = true,
                        IgnoreNullValues = true,
                    });

                    return districtsInState;
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, $"Error fetching districts in state {matchingState}");
                return null;
            }
        }

        
        public async Task<List<VaccineCenter>> FindMatchingVaccineCenter(
            int district, 
            string weekStart, 
            int age,
            bool ignoreIfUnavailable)
        {
            var result = new List<VaccineCenter>();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{pathCalendarByDistrict}?district_id={district}&date={weekStart}");
            request.Headers.Add("Accept", "application/json");

            try {
                using (var response = await httpClient.SendAsync(request)) {
                    if (!response.IsSuccessStatusCode) {
                        logger.LogError($"Error fetching vaccination centers for district {district} and date {weekStart}. Http Response Code: {response.StatusCode}");
                        return result;
                    }

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var vaccineCenters = JsonSerializer.Deserialize<VaccineCalendarByDistrictResponse>(responseJson, new JsonSerializerOptions {
                        AllowTrailingCommas = true,
                        IgnoreNullValues = true,
                    });

                    if (vaccineCenters.IsEmpty()) return result;

                    List<VaccineCenter> matchingCenters = vaccineCenters.WithMatchingAgeLimit(age, ignoreIfUnavailable);
                    return matchingCenters;
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Error fetching vaccination centers.");
                return null;
            }
        }

        public async Task<List<VaccineCenter>> FindMatchingVaccineCenterByPinCode(
            int pin, 
            string weekStart, 
            int age, 
            bool ignoreIfUnavailable)
        {
            var result = new List<VaccineCenter>();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{pathCalendarByPin}?pincode={pin}&date={weekStart}");
            request.Headers.Add("Accept", "application/json");

            try {
                using (var response = await httpClient.SendAsync(request)) {
                    if (!response.IsSuccessStatusCode) {
                        logger.LogError($"Error fetching vaccination centers for Pin {pin} and date {weekStart}. Http Response Code: {response.StatusCode}");
                        return result;
                    }

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var vaccineCenters = JsonSerializer.Deserialize<VaccineCalendarByDistrictResponse>(responseJson, new JsonSerializerOptions {
                        AllowTrailingCommas = true,
                        IgnoreNullValues = true,
                    });

                    if (vaccineCenters.IsEmpty())
                        return result;

                    List<VaccineCenter> matchingCenters = vaccineCenters.WithMatchingAgeLimit(age, ignoreIfUnavailable);
                    return matchingCenters;
                }
            }
            catch (Exception ex) {
                logger.LogError(ex, "Error fetching vaccination centers.");
                return null;
            }
        }
    }
}
