using DataAccessLayer.Model;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IVehicleRepo : IRepository<Vehicle>
    {
        bool DoesVinExists(string vin);
        bool DoesLicensePlateExists(string licensePlate);
        List<Vehicle> GetOnlyAvailableVehicles(int? id = null);
    }
}
