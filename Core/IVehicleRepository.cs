using System.Threading.Tasks;
using vega.Core.models;

namespace vega.Core
{
    public interface IVehicleRepository
    {
        void Add(Vehicle vehicle);
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        void Remove(Vehicle vehicle);
    }
}