using DataAccessLayer.Model;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IDriverRepo : IRepository<Driver>
    {

        #region HELPERS
        /// <summary>
        /// Function for checking if NationalInsuranceNr Exists
        /// </summary>
        /// <param name="nationalInsuranceNr)">nationalInsuranceNr of driver</param>
        bool DoesNationalInsuranceNrExists(string nationalInsuranceNr);

        /// <summary>
        /// Function for checking if NationalInsuranceNr is valid
        /// </summary>
        ///<param name="nationalInsuranceNr)">nationalInsuranceNr of driver</param>
        public bool DoesNationalInsuranceNrIsValid(string nationalInsuranceNr);

        /// <summary>
        /// Driver without fuelcard
        /// </summary>
        /// <returns>list of drivers without fuelCard</returns>
        List<Driver> GetOnlyAvailableDrivers(int? id = null);

        #endregion

    }
}