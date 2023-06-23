using DataAccessLayer.Model;

namespace DataAccessLayer.Interfaces
{
    public interface IFuelTypeRepo : IRepository<FuelType>
    {
        /// <summary>
        /// Function for deleting fuelCardfueltype from fuelCard (linking table)
        /// </summary>
        /// <param name="fuelCardId"></param>
        /// <param name="fueltypeId"></param>
        void DeleteFuelCardFuelType( int fueltypeId , int fuelCardId); 
    }
}
