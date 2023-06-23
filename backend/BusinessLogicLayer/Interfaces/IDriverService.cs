using BusinessLogicLayer.ViewModels;
using BusinessLogicLayer.ViewModels.Driver;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// interface that defines all methods that are going to be used in business logic of driver
    /// </summary>
    public interface IDriverService
    {

        #region INDEX-GET ALL
        /// <summary>
        /// Gets all drivers from repo and converts it to dto objects -(index)
        /// </summary>
        /// <returns>IEnumerable of DriverListViewModel</returns>
        IEnumerable<DriverListViewModel> TryGet();
        #endregion

        #region DETAILS
        /// <summary>
        /// Gets data of a specific driver from repo and converts it to dto object -(detail)
        /// </summary>
        /// <param name="id">id of specific driver</param>
        /// <returns>DriverViewModel</returns>
        DriverViewModel TryGet(int id);
        #endregion

        #region REMOVE
        /// <summary>
        /// Handles logic for removing driver from list
        /// </summary>
        /// <param name="id">id of driver</param>
        /// <returns>result of action</returns>
        void TryRemove(int id);
        #endregion

        #region CREATE/ADD

        #region CREATE/ADD - (GET)
        /// <summary>
        /// sets empty dto for the create form and gets selectList data
        /// all executed in TryInit in REGION HELPERS
        /// </summary>
        #endregion

        #region CREATE/ADD - (POST)
        /// <summary>
        /// Converts dto to db object and asks repo to add this driver to db -(create)
        /// </summary>
        /// <param name="model">DTO</param>
        /// <returns>ID of new driver</returns>
        int TryCreate(DriverModel model);
        #endregion

        #endregion

        #region EDIT/UPDATE

        #region EDIT/UPDATE - (GET)
        /// <summary>
        /// sets properties dto for the edit form and gets selectList data
        /// </summary>
        /// <param name="id">id of driver</param>
        /// <returns>DriverModel</returns>
        DriverModel TryEdit(int id);
        #endregion

        #region EDIT/UPDATE - (POST)
        /// <summary>
        /// converts new values of driver to db object and asks repo to update this object.
        /// </summary>
        /// <param name="model">updated values driver</param>
        /// <returns>id of driver</returns>
        int TryEdit(DriverModel model);
        #endregion

        #endregion

        #region HELPERS
        /// <summary>
        /// sets all selectlists in model for create/edit forms
        /// </summary>
        /// <returns>DriverModel</returns>
        DriverModel TryInit(DriverModel? model = null);
        #endregion       

    }
}
