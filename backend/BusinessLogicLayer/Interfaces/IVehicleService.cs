using BusinessLogicLayer.ViewModels;
using BusinessLogicLayer.ViewModels.Vehicle;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// interface that defines all methods that are going to be used in business logic of vehicles
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Gets all vehicles from repo and converts it to dto objects
        /// </summary>
        /// <returns>IEnumerable of VehicleListViewModels</returns>
        IEnumerable<VehicleListViewModel> TryGet();

        /// <summary>
        /// Gets data of a specific vehicle from repo and converts it to dto object
        /// </summary>
        /// <param name="id">id of specific vehicle</param>
        /// <returns>VehicleViewModel</returns>
        VehicleViewModel TryGet(int id);

        /// <summary>
        /// Handles logic for removing vehicle from list
        /// </summary>
        /// <param name="id">id of vehicle</param>
        /// <returns>result of action</returns>
        void TryRemove(int id);

        /// <summary>
        /// sets all selectlists in model for create/edit forms
        /// </summary>
        /// <returns>VehicleModel</returns>
        VehicleModel TryInit(VehicleModel? model = null);

        /// <summary>
        /// Converts dto to db object and asks repo to add this vehicle to db
        /// </summary>
        /// <param name="model">DTO</param>
        /// <returns>ID of new vehicle</returns>
        int TryCreate(VehicleModel model);

        /// <summary>
        /// sets properties dto for the edit form
        /// </summary>
        /// <param name="id">id of vehicle</param>
        /// <returns>VehicleModel</returns>
        VehicleModel TryEdit(int id);

        /// <summary>
        /// converts new values of vehicle to db object and asks repo to update this object.
        /// </summary>
        /// <param name="model">updated values vehicle</param>
        /// <returns>id of vehicle</returns>
        int TryEdit(VehicleModel model);
    }
}
