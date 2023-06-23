using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels;
using BusinessLogicLayer.ViewModels.FuelCard;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// Class which contains all business logic of the fuel card domain.
    /// </summary>
    public class FuelCardService : IFuelCardService
    {
        #region PROPS
        private readonly IFuelCardRepo _fuelCardRepo;
        private readonly IDriverRepo _driverRepo;
        private readonly IFuelTypeRepo _fuelTypeRepo;
        #endregion

        #region CONSTR
        public FuelCardService(
            IFuelTypeRepo fuelTypeRepo,
            IDriverRepo driverRepo,
            IFuelCardRepo fuelCardRepo)
        {
            _fuelCardRepo = fuelCardRepo;
            _driverRepo = driverRepo;
            _fuelTypeRepo = fuelTypeRepo;
        }
        #endregion

        #region INDEX
        /// <summary>
        /// Gets all fuel cards from repo and converts it to dto objects.
        /// </summary>
        /// <returns>IEnumerable of FuelCardListViewModels</returns>
        /// <exception cref="VehicleException"></exception>
        public IEnumerable<FuelCardListViewModel> TryGet()
        {
            try
            {
                var fuelCards = _fuelCardRepo.GetAll().OrderBy(x => x.CardNumber);

                return fuelCards
                    .Select(x => new FuelCardListViewModel
                    {
                        FuelCardID = x.FuelCardID,
                        CardNumber = x.CardNumber,
                        ExpirationDate = x.ExpirationDate,
                        Pincode = x.Pincode,
                        Isdisabled = x.IsDisabled,
                        Driver = x.Driver != null
                            ? $"{x.Driver?.LastName} {x.Driver?.FirstName}"
                            : "",
                        DriverID = x.DriverID,
                        FuelTypeID = x.FuelCardFuelType
                        .Select(x => x.FuelTypeID)
                        .ToList(),
                        FuelType = string.Join(" ", x.FuelCardFuelType.Select(x => x.FuelType?.Name))
                    });
            }
            catch (Exception e)
            {
                throw new FuelCardException(e.Message);
            }
        }
        #endregion

        #region DETAILS
        /// <summary>
        /// Gets data of a specific fuel card from repo and converts it to dto object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>FuelCardViewModel</returns>
        /// <exception cref="FuelCardException"></exception>
        public FuelCardViewModel TryGet(int id)
        {
            try
            {
                if (id <= 0) throw new FuelCardException("ID is invalid.");

                var fuelCard = _fuelCardRepo.Get(id);

                if (fuelCard == null) throw new FuelCardException("Db object is NULL.");

                return new FuelCardViewModel
                {
                    FuelCardID = fuelCard.FuelCardID,
                    CardNumber = fuelCard.CardNumber,
                    ExpirationDate = fuelCard.ExpirationDate,
                    Pincode = fuelCard.Pincode,
                    IsDisabled = fuelCard.IsDisabled,
                    FuelType = string.Join(" ", fuelCard.FuelCardFuelType.Select(x => x.FuelType?.Name)),

                    Driver = fuelCard.Driver != null
                            ? $"{fuelCard.Driver?.LastName} {fuelCard.Driver?.FirstName}"
                            : "",
                    DriverFirstName = fuelCard?.Driver?.FirstName ?? "",
                    DriverLastName = fuelCard?.Driver?.LastName ?? "",
                    DriverID = fuelCard.DriverID,
                    DriverNationalInsuranceNr = fuelCard?.Driver?.NationalInsuranceNr ?? "",
                    DriverLicenseType = fuelCard.Driver != null 
                        ? string
                            .Join('-', fuelCard.Driver.DriverDriverLicenseType
                            .Select(x => x.DriverLicenseType?.Name)) 
                        : "",
                    DriverdateOfBirth = fuelCard.Driver?.DateOfBirth,
                };
            }
            catch (Exception e)
            {
                throw new FuelCardException(e.Message);
            }
        }
        #endregion

        #region REMOVE
        /// <summary>
        /// Checks given id and asks repo to soft delete fuel card with this id.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FuelCardException"></exception>
        public void TryRemove(int id)
        {
            try
            {
                if (id <= 0) { throw new FuelCardException("ID is invalid."); }

                _fuelCardRepo.Delete(id);
            }

            catch (Exception e)
            {
                throw new FuelCardException(e.Message);
            }
        }
        #endregion

        #region SELECTLISTS
        /// <summary>
        /// Sets all selectlists in fuelCardmodel for create/edit forms.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>FuelCardModel</returns>
        /// <exception cref="FuelCardException"></exception>
        public FuelCardModel TryInit(FuelCardModel? model = null)
        {
            try
            {
                if (model == null)
                {
                    model = new FuelCardModel
                    {
                        FuelTypeSelectList = new MultiSelectList(
                            _fuelTypeRepo.GetAll(),
                            "FuelTypeID", "Name"),
                        DriverSelectList = new SelectList(
                            _driverRepo
                            .GetOnlyAvailableDrivers()
                            .Select(x => new 
                            { 
                                x.DriverID, 
                                FullName = $"{x.FirstName} {x.LastName}" 
                            }),
                            "DriverID", "FullName"),
                    };

                    return model;
                }

                model.FuelTypeSelectList = new MultiSelectList(
                    _fuelTypeRepo.GetAll(),
                   "FuelTypeID", "Name",
                    model.FuelTypeID);

                model.DriverSelectList = new SelectList(
                    _driverRepo.GetOnlyAvailableDrivers(model.DriverID)
                    .Select(x => new
                    {
                        x.DriverID,
                        FullName = $"{x.FirstName} {x.LastName}"
                    }),
                    "DriverID", "FullName");

                return model;
            }
            catch (Exception e)
            {
                throw new FuelCardException(e.Message);
            }
        }
        #endregion

        #region CREATE
        /// <summary>
        /// Converts dto to db object and asks repo to add this fuel card to db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ID of new fuel card</returns>
        /// <exception cref="FuelCardException"></exception>
        public int TryCreate(FuelCardModel model)
        {
            try
            {
                if (_fuelCardRepo.DoesCardNumberExists(model.CardNumber))
                {
                    //throw new FuelCardException("Card number already exists.");
                    return 0;
                }

                var fuelCard = new FuelCard
                {
                    CardNumber = model.CardNumber,
                    ExpirationDate = model.ExpirationDate,
                    Pincode = model.Pincode,
                    DriverID = model.DriverID,
                    IsDisabled = model.IsDisabled,
                    IsActive = true
                };

                if (model.FuelTypeID != null)
                {
                    foreach (var fueltypeID in model.FuelTypeID)
                    {
                        fuelCard.FuelCardFuelType.Add(new FuelCardFuelType
                        {
                            FuelTypeID = fueltypeID
                        });
                    }
                }

                int newID = _fuelCardRepo.Add(fuelCard);

                return newID;
            }
            catch (Exception e)
            {
                throw new FuelCardException(e.Message);
            }
        }
        #endregion

        #region EDIT
        #region GET
        /// <summary>
        /// Sets properties of dto by properties of fuelCard object from db
        /// </summary>
        /// <param name="id">id of fuelCard</param>
        /// <returns>FuelCardModel</returns>
        public FuelCardModel TryEdit(int id)
        {
            try
            {
                if (id <= 0) throw new FuelCardException("ID is invalid.");

                var fuelCard = _fuelCardRepo.Get(id);

                if (fuelCard == null) throw new FuelCardException("Db object is NULL.");

                var model = new FuelCardModel
                {
                    FuelCardID = id,
                    DriverID = fuelCard.DriverID,
                    CardNumber = fuelCard.CardNumber,
                    ExpirationDate = fuelCard.ExpirationDate,
                    Pincode = fuelCard.Pincode,
                    IsDisabled = fuelCard.IsDisabled,
                    FuelTypeID = fuelCard.FuelCardFuelType
                    .Select(x => x.FuelTypeID)
                    .ToList()
                    //    Driver = fuelCard.Driver != null
                    //            ? $"{fuelCard.Driver?.LastName} {fuelCard.Driver?.FirstName}"
                    //            : "",
                };

                // get data for selectlists
                var fullModel = TryInit(model);

                return fullModel;
            }
            catch (Exception e)
            {
                throw new FuelCardException(e.Message);
            }
        }
        #endregion

        #region PUT
        /// <summary>
        /// converts new values of fuel card to db object and asks repo to update this object.
        /// </summary>
        /// <param name="model">updated values fuel card</param>
        /// <returns>id of a fuelcard</returns>
        public int TryEdit(FuelCardModel model)
        {
            try
            {
                if (model.FuelCardID <= 0)
                    throw new FuelCardException("ID is invalid.");

                var fuelCard = _fuelCardRepo.Get(model.FuelCardID);

                if (fuelCard == null)
                    throw new FuelCardException("Db object is NULL.");

                if (model.CardNumber.Trim() != fuelCard.CardNumber.Trim() &&
                    _fuelCardRepo.DoesCardNumberExists(model.CardNumber)) { 
                    //throw new FuelCardException("Card number already in use.");
                    return 0;
                }

                fuelCard.CardNumber = model.CardNumber;
                fuelCard.ExpirationDate = model.ExpirationDate;
                fuelCard.Pincode = model.Pincode;
                fuelCard.IsDisabled = model.IsDisabled;
                fuelCard.DriverID = model.DriverID;
                fuelCard.FuelCardID = model.FuelCardID;

                // reset driver in fuelcard if driver in model is null
                if (model.DriverID == null && fuelCard.Driver != null)
                {
                    fuelCard.Driver.FuelCardID = null;
                }

                if (model.FuelTypeID != null)
                {
                    foreach (var fuelType in fuelCard.FuelCardFuelType)
                    {
                        _fuelTypeRepo.DeleteFuelCardFuelType(fuelType.FuelTypeID, fuelCard.FuelCardID);
                    }
                    foreach (var fueltypeID in model.FuelTypeID)
                    {
                        fuelCard.FuelCardFuelType.Add(new FuelCardFuelType
                        {
                            FuelTypeID = fueltypeID
                        });
                    }
                }

                var id = _fuelCardRepo.Update(fuelCard);

                return id;
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }
        #endregion
        #endregion
       
    }
}
