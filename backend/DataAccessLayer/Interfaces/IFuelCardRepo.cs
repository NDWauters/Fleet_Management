using DataAccessLayer.Model;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IFuelCardRepo : IRepository<FuelCard>
    {
        /// <summary>
        /// Checks if given cardNumber already exists in db.
        /// </summary>
        /// <param name="CardNumber"></param>
        /// <returns>bool</returns>
        bool DoesCardNumberExists(string CardNumber);

        /// <summary>
        /// Gets only fuelCards from DB.
        /// </summary>
        /// <returns>>List of fuels cards objects</returns>
        List<FuelCard> GetOnlyAvailableFuelCards(int? id = null);
    }
}
