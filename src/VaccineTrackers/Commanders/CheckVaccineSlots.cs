using ConsoleAppFramework;
using Cowin.Domain.Abstractions;
using Cowin.Infrastructure.Dates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cowin.VaccineTrackers.Commanders
{
    class CheckVaccineSlots : ConsoleAppBase
    {
        private readonly ICowinHttpClient _cowinHttpClient;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CheckVaccineSlots(
            ICowinHttpClient cowinHttpClient,
            INotificationService notificationService,
            IConfiguration configuration,
            ILogger<CheckVaccineSlots> logger)
        {
            this._cowinHttpClient = cowinHttpClient;
            this._notificationService = notificationService;
            this._configuration = configuration;
            this._logger = logger;
        }



        // dotnet run -- CheckVaccineSlots bydistrict -s karnataka -d bbmp -a 18 --date 03-05-2021 -w 8
        [Command("byDistrict", "Track Vaccine availability by District")]
        public async Task Start(
            [Option("s", "State name")] string state,
            [Option("d", "District nam (example: BBMP)")] string district,
            [Option("a", "Minimum Age limit (18, 45 or 60)")] int age,
            [Option("Starting date from which to track in dd-mm-yyyy format. Example: 03-11-2021. If skipped will start from current week.")] string date = null,
            [Option("w", "Number of future weeks to check for availability")] int futureWeeks = 4,
            [Option("c", "If true, will continuously check for vaccine availability.")] bool continuous = false)
        {
            if (age < 18 || age > 200) {
                _logger.LogError($"Invalid Age {age}");
                return;
            }
             if (futureWeeks < 1 || futureWeeks > 53) {
                _logger.LogError("Invalid number of weeks to check for availability.");
                return;
            }

            _logger.LogInformation("Fetching states..");
            var allstates = await _cowinHttpClient.GetAllStates();
            if (default == allstates) return;
            if (allstates.IsEmpty()) {
                _logger.LogError($"No states found.");
                return;
            }

            var matchingState = allstates.GetState(state);
            if (default == matchingState) return;
            if (matchingState.IsEmpty()) {
                _logger.LogError($"Invalid State {state}");
                return;
            }

            _logger.LogInformation($"Fetching districts for state {state}");
            var districtsInState = await _cowinHttpClient.GetAllDistrictsInState(matchingState);
            if (default == districtsInState) return;
            if (districtsInState.IsEmpty()) {
                _logger.LogError("No districts found.");
                return;
            }

            var matchingDistrict = districtsInState.GetDistrict(district);
            if (default == matchingDistrict) return;
            if (matchingDistrict.IsEmpty()) {
                _logger.LogError($"Invalid District {district}");
                return;
            }


            var next12Weeks = DateHelpers.GetStartDatesForUpcomingWeeks(date, futureWeeks);
            if (null == next12Weeks || next12Weeks.Count < 1) {
                _logger.LogError($"Invalid date {date}");
                return;
            }

            if (!continuous) {
                try {
                    Console.Clear();
                    foreach (var weekStart in next12Weeks) {
                        var vaccineCenters = await _cowinHttpClient
                            .FindMatchingVaccineCenter(
                                matchingDistrict.Id,
                                weekStart,
                                age,
                                true);
                        if (null == vaccineCenters || vaccineCenters.Count < 1) {
                            _logger.LogDebug($"No vaccine center found in district {district} for week starting on {weekStart} for age >= {age}.");
                            continue;
                        }

                        await _notificationService.NotifyUser(vaccineCenters, weekStart);
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                    Console.WriteLine("..Done");
                    //Console.ReadLine();
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "Error found while checking for vaccine.");
                }
            }
            else {
                int iteration = 0;
                try {
                    while (!this.Context.CancellationToken.IsCancellationRequested) {
                        try {
                            Console.Clear();
                            Console.WriteLine($"iteration {++iteration}..");
                            foreach (var weekStart in next12Weeks) {
                                var vaccineCenters = await _cowinHttpClient
                                    .FindMatchingVaccineCenter(
                                        matchingDistrict.Id,
                                        weekStart,
                                        age,
                                        true);
                                if (null == vaccineCenters || vaccineCenters.Count < 1) {
                                    _logger.LogDebug($"No vaccine center found in district {district} for week starting on {weekStart} for age >= {age}.");
                                    continue;
                                }
                                await _notificationService.NotifyUser(vaccineCenters, weekStart);
                            }
                        }
                        catch (Exception ex) {
                            _logger.LogError(ex, "Error found while checking for vaccine.");
                        }

                        // wait for next time
                        await Task.Delay(TimeSpan.FromMinutes(1), this.Context.CancellationToken);
                    }
                }
                catch (Exception ex) when (!(ex is OperationCanceledException)) {
                    _logger.LogError(ex, $"Error encountered during Continuous tracking by district {district}");
                }
                finally { }
            }
        }
    }
}
