using BusinessLogicLayer.ViewModels;
using BusinessLogicLayer.ViewModels.FuelCard;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFuelCardService
    {
        /// <summary>
        /// Gets all fuel cards from repo and converts it to dto objects
        /// </summary>
        /// <returns>IEnumerable of FuelCardListViewModels</returns>
        IEnumerable<FuelCardListViewModel> TryGet();

        /// <summary>
        /// Gets data of a specific fuel card from repo and converts it to dto object
        /// </summary>
        /// <param name="id">id of a specific fuel card</param>
        /// <returns>FuelCardViewModel</returns>
        FuelCardViewModel TryGet(int id);

        /// <summary>
        /// Handles logic for removing fuel card from list
        /// </summary>
        /// <param name="id">id of a fuel card</param>
        /// <returns>result of action</returns>
        void TryRemove(int id);

        /// <summary>
        /// sets all selectlists in model for create/edit forms
        /// </summary>
        /// <returns>FuelCardModel</returns>
        FuelCardModel TryInit(FuelCardModel? model = null);

        /// <summary>
        /// Converts dto to db object and asks repo to add this fuel card to db
        /// </summary>
        /// <param name="model">DTO</param>
        /// <returns>ID of new fuel card</returns>
        int TryCreate(FuelCardModel model);

        FuelCardModel TryEdit(int id);

        /// <summary>
        /// converts new values of a fuelcard to db object and asks repo to update this object.
        /// </summary>
        /// <param name="model">updated values fuel card</param>
        /// <returns>id of fuel card</returns>
        int TryEdit(FuelCardModel model);

    }
}
