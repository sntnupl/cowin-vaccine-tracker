using ConsoleAppFramework;
using Cowin.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cowin.VaccineTrackers.Commanders
{
    class GetDistrictId: ConsoleAppBase
    {
        private readonly ICowinHttpClient _cowinHttpClient;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetDistrictId> _logger;

        public GetDistrictId(
            ICowinHttpClient cowinHttpClient,
            INotificationService notificationService,
            IConfiguration configuration,
            ILogger<GetDistrictId> logger)
        {
            _cowinHttpClient = cowinHttpClient;
            _notificationService = notificationService;
            _configuration = configuration;
            _logger = logger;
        }

        // dotnet run -- getdistrictid bystate -s karnataka
        [Command("byState", "Get District IDs in a State")]
        public async Task ByState(
            [Option("s", "State name")] string state)
        {
            if (string.IsNullOrEmpty(state)) {
                _logger.LogError($"Invalid state {state}");
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
            if (default == districtsInState)
                return;
            if (districtsInState.IsEmpty()) {
                _logger.LogError("No districts found.");
                return;
            }

            Console.Clear();
            await _notificationService.ShowDistrictsToUser(districtsInState, matchingState.Name);
        }
    }
}
