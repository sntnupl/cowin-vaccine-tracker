using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Cowin.Domain.Entities
{
    public class VaccineCalendarByDistrictResponse
    {
        [JsonPropertyName("centers")]
        public List<VaccineCenter> Centers { get; set; }

        public bool IsEmpty()
        {
            return null == Centers || Centers.Count < 1;
        }

        public List<VaccineCenter> WithMatchingAgeLimit(int age, bool ignoreIfUnavailable)
        {
            if (IsEmpty()) return new List<VaccineCenter>();

            return Centers.Where(c => c.MatchesAgeLimit(age, ignoreIfUnavailable)).ToList();
        }
    }

    public class VaccineCenter
    {
        [JsonPropertyName("center_id")]
        public int CenterId { get; set; }


        [JsonPropertyName("name")]
        public string Name { get; set; }


        [JsonPropertyName("state_name")]
        public string State { get; set; }


        [JsonPropertyName("district_name")]
        public string District { get; set; }


        [JsonPropertyName("block_name")]
        public string Block { get; set; }

        [JsonPropertyName("pincode")]
        public int PinCode { get; set; }


        [JsonPropertyName("lat")]
        public int Lat { get; set; }


        [JsonPropertyName("long")]
        public int Long { get; set; }


        [JsonPropertyName("from")]
        public string StartTime { get; set; }


        [JsonPropertyName("to")]
        public string EndTime { get; set; }


        [JsonPropertyName("fee_type")]
        public string BillType { get; set; }


        [JsonPropertyName("sessions")]
        public List<VaccineSession> Sessions { get; set; }

        internal bool MatchesAgeLimit(int age, bool ignoreIfUnavailable)
        {
            if (null == Sessions || Sessions.Count < 1) return false;

            var sessionsInAgeLimit = Sessions.Where(s => s.MinAgeLimit <= age && s.AvailableCapacity > 0).ToList();
            return sessionsInAgeLimit?.Count > 0;
        }
    }

    public class VaccineSession
    {
        [JsonPropertyName("session_id")]
        public string Id { get; set; }


        [JsonPropertyName("date")]
        public string Date { get; set; }


        [JsonPropertyName("available_capacity")]
        public double AvailableCapacity { get; set; }


        [JsonPropertyName("min_age_limit")]
        public int MinAgeLimit { get; set; }


        [JsonPropertyName("vaccine")]
        public string VaccineName { get; set; }


        [JsonPropertyName("slots")]
        public List<string> Slots { get; set; }
    }
}
