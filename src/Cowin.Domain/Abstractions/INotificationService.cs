using Cowin.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cowin.Domain.Abstractions
{
    public interface INotificationService
    {
        Task NotifyUser(List<VaccineCenter> vaccineCenters, string weekStart, int age);
        Task ShowDistrictsToUser(DistrictsInState districtsInState, string state);
    }
}
