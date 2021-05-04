using Cowin.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cowin.Domain.Abstractions
{
    public interface ICowinHttpClient
    {
        Task<AllStates> GetAllStates();
        
        Task<DistrictsInState> GetAllDistrictsInState(State matchingState);
        
        Task<List<VaccineCenter>> FindMatchingVaccineCenter(int district, string weekStart, int age, bool ignoreIfUnavailable);
        Task<List<VaccineCenter>> FindMatchingVaccineCenterByPinCode(int pin, string weekStart, int age, bool v);
    }
}
