using Cowin.Domain.Abstractions;
using Cowin.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cowin.Infrastructure.Notification
{
    public class ConsoleNotification : INotificationService
    {
        private readonly ILogger<ConsoleNotification> logger;

        public ConsoleNotification(ILogger<ConsoleNotification> logger)
        {
            this.logger = logger;
        }

        public Task NotifyUser(List<VaccineCenter> vaccineCenters, string week)
        {
            if (null == vaccineCenters || vaccineCenters.Count < 1) return Task.CompletedTask;

            //Console.Clear();
            Console.WriteLine($"Found {vaccineCenters.Count} vaccine centers for week starting {week}:");
            foreach (var vc in vaccineCenters) {
                Console.WriteLine(CompactNotification(vc));
            }

            return Task.CompletedTask;
        }

        public Task ShowDistrictsToUser(DistrictsInState districtsInState, string state)
        {
            var sb = new StringBuilder($"---------- Districts in {state} ----------\n");

            foreach(var district in districtsInState.Districts) {
                sb.AppendLine($"{district.Name}, ID: {district.Id}");
            }

            Console.WriteLine(sb.ToString());
            return Task.CompletedTask;
        }

        private string CompactNotification(VaccineCenter vc)
        {
            var sb = new StringBuilder("---------------------------------------\n");
            sb.AppendLine($"{vc.Name} | {vc.PinCode} | {vc.StartTime} - {vc.EndTime} | {vc.BillType}");
            foreach(var session in vc.Sessions) {
                sb.AppendLine($"  - {session.AvailableCapacity} available on {session.Date}, age limit {session.MinAgeLimit} for {session.VaccineName} vaccine ");
            }
            return sb.ToString();
        }
    }
}
