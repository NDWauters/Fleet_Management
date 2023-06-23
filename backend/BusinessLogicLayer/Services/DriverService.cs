using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels;
using BusinessLogicLayer.ViewModels.Driver;
using DataAccessLayer.Model;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// class which contains all business logic of the driver domain
    /// </summary>
    public class DriverService : IDriverService 
    {
        #region PROPS
        private readonly IDriverRepo _driverRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly IDriverLicenseTypeRepo _driverLicenseTypeRepo;
        private readonly IFuelCardRepo _fuelCardRepo;
        private readonly IVehicleRepo _vehicleRepo;
        #endregion

        #region CONSTR
        public DriverService(
            IDriverRepo driverRepo,
            IAddressRepo addressRepo,
            IDriverLicenseTypeRepo driverLicenseTypeRepo,
            IFuelCardRepo fuelCardRepo,
            IVehicleRepo vehicleRepo
            )
        {
            _driverRepo = driverRepo;
            _addressRepo = addressRepo;
            _driverLicenseTypeRepo = driverLicenseTypeRepo;
            _fuelCardRepo = fuelCardRepo;
            _vehicleRepo = vehicleRepo;
        }
        #endregion

        #region INDEX-GET ALL
        /// <summary>
        /// Gets all drivers from repo and converts it to dto objects
        /// </summary>
        /// <returns>IEnumerable of DriverListViewModels</returns>
        /// <exception cref="DriverException">If something goes wrong with getting data from repo.</exception>
        public IEnumerable<DriverListViewModel> TryGet()
        {
            try
            {
                var drivers = _driverRepo.GetAll();
                
                return drivers.Select(x => new DriverListViewModel
                {
                        DriverID = x.DriverID,
                        LastName = x.LastName,
                        FirstName = x.FirstName,
                        DateOfBirth = x.DateOfBirth,
                        NationalInsuranceNr = x.NationalInsuranceNr,
                        AddressID = x.Address.AddressID,
                        
                        Street = x.Address.Street,
                        Number = x.Address.Number,
                        Zipcode = x.Address.Zipcode,
                        Place = x.Address.Place,

                        VehicleID = x.VehicleID ?? 0,
                        Model = x.Vehicle?.Model ?? "",
                        Brand = x.Vehicle?.Brand.Name ?? "",
                        LicensePlate = x.Vehicle?.LicensePlate ?? "",

                        FuelCardID = x.FuelCardID ?? 0,
                        CardNumber = x.FuelCard?.CardNumber ?? "",
                    });
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryGet()", e);
            }
        }
        #endregion

        #region DETAILS
        /// <summary>
        /// Gets data of a specific driver from repo and converts it to dto object
        /// </summary>
        /// <param name="id">id of specific driver</param>
        /// <returns>DriverViewModel</returns>
        /// <exception cref="DriverException">Invalid ID or db object null</exception>
        public DriverViewModel TryGet(int id)
        {
            try
            {
                if (id <= 0) throw new DriverException("Error in TryGet(int id) - ID is invalid.");

                var driver = _driverRepo.Get(id);

                if (driver == null) throw new DriverException("Error in TryGet(int id) - Db object is NULL.");

                return new DriverViewModel
                {
                    DriverID = driver.DriverID,
                    LastName = driver.LastName,
                    FirstName = driver.FirstName,
                    DateOfBirth = driver.DateOfBirth,
                    NationalInsuranceNr = driver.NationalInsuranceNr,
                    AddressID = driver.Address.AddressID,

                    Street = driver.Address.Street,
                    Number = driver.Address.Number,
                    Zipcode = driver.Address.Zipcode,
                    Place = driver.Address.Place,

                    DriverLicenseTypeID = driver.DriverDriverLicenseType
                                .Select(x => x.DriverLicenseTypeID)
                                .ToList(),
                
                    DriverLicenseType = string
                        .Join('-', driver.DriverDriverLicenseType
                        .Select(x => x.DriverLicenseType?.Name)),
                    //DriverLicenseTypes = driver.DriverDriverLicenseType.Select(x => x.DriverLicenseType.Name).ToList(),

                    VehicleID = driver.VehicleID ?? 0,
                    Model = driver.Vehicle?.Model ?? "",
                    Brand = driver.Vehicle?.Brand?.Name ?? "",
                    LicensePlate = driver.Vehicle?.LicensePlate ?? "",

                    FuelCardID = driver.FuelCardID ?? 0,
                    CardNumber = driver.FuelCard?.CardNumber ?? "",
                    ExpirationDate = driver.FuelCard?.ExpirationDate ?? null,
                };
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryGet(int id)", e);
            }
        }
        #endregion

        #region REMOVE
        /// <summary>
        /// Checks given id and asks repo to soft delete driver with this id.
        /// </summary>
        /// <param name="id">ID of specific driver</param>
        /// <exception cref="DriverException">if id is invalid or something goes wrong in db</exception>
        public void TryRemove(int id)
        {
            try
            {
                if (id <= 0) throw new DriverException("Error in TryRemove(int id) - ID is invalid.");

                _driverRepo.Delete(id);
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryRemove(int id)", e);
            }
        }
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
        /// Converts dto to db object and asks repo to add this driver to db
        /// </summary>
        /// <param name="model">DTO</param>
        /// <returns>ID of new driver</returns>
        public int TryCreate(DriverModel model)
        {
            try
            {
                if (_driverRepo.DoesNationalInsuranceNrExists(model.NationalInsuranceNr))
                {
                    //throw new DriverException("Error in TryCreate(DriverModel model) - NationalInsuranceNr number already exists.");
                    return 0;
                }
                else
                {
                    if (!checkNationalInsuranceNr(model.NationalInsuranceNr))
                        {
                        throw new DriverException("Error in TryCreate(DriverModel model) - NationalInsuranceNr number is not valid.");
                    }
                }

                var driver = new Driver
                {
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    DateOfBirth = model.DateOfBirth,
                    NationalInsuranceNr = model.NationalInsuranceNr,
                    VehicleID = model.VehicleID,
                    FuelCardID = model.FuelCardID,
                    IsActive = true
                };

                driver.Address = new Address
                {
                    Street = model.Address.Street,
                    Number = model.Address.Number,
                    Place = model.Address.Place,
                    Zipcode = model.Address.Zipcode,
                    IsActive = true
                };

                if (model.DriverLicenseTypeID != null)
                {
                    foreach (var driverLicenseID in model.DriverLicenseTypeID)
                    {
                        driver.DriverDriverLicenseType.Add(new DriverDriverLicenseType
                        {
                            DriverLicenseTypeID = driverLicenseID
                        });
                    }
                }

                var newID = _driverRepo.Add(driver);

                //update kolom AddressID in driver -> gebeurt niet automatisch
                if(newID != 0)
                {
                    var newDriver = _driverRepo.Get(newID);
                    int newAddressID = newDriver.Address.AddressID;
                    newDriver.AddressID = newAddressID;
                }

                //Update kolom DriverID in Vehicle -> gebeurt niet automatisch
                if (model.VehicleID != null)
                {
                    var vehicle = _vehicleRepo.Get((int)model.VehicleID);
                    vehicle.DriverID = newID;
                }

                //Update kolom DriverID in FuelCard -> gebeurt niet automatisch
                if (model.FuelCardID != null)
                {
                    var fuelCard = _fuelCardRepo.Get((int)model.FuelCardID);
                    fuelCard.DriverID = newID;
                }

                _driverRepo.Update(driver);

                return newID;
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryCreate(DriverModel model)", e);
            }

        }
        #endregion

        #endregion

        #region EDIT/UPDATE

        #region EDIT/UPDATE - (GET)
        /// <summary>
        /// Sets properties of dto by properties of driver object from db and gets selectList data
        /// </summary>
        /// <param name="id">id of driver</param>
        /// <returns>DriverModel</returns>
        public DriverModel TryEdit(int id)
        {
            try
            {
                if (id <= 0) throw new DriverException("Error in TryEdit(int id) - ID is invalid.");

                var driver = _driverRepo.Get(id);

                if (driver == null) throw new DriverException("Error in TryEdit(int id) - Db object is NULL.");

                var model = new DriverModel
                {
                    DriverID = id,
                    LastName = driver.LastName,
                    FirstName = driver.FirstName,
                    DateOfBirth = driver.DateOfBirth,
                    NationalInsuranceNr = driver.NationalInsuranceNr,
                    VehicleID = driver.VehicleID,
                    FuelCardID = driver.FuelCardID,
                    CardNumber = driver.FuelCard?.CardNumber ?? "",
                    Address = new AddressModel
                    {
                        Number = driver.Address.Number,
                        Place = driver.Address.Place,
                        Zipcode = driver.Address.Zipcode,
                        Street = driver.Address.Street
                    },
                    DriverLicenseTypeID = driver.DriverDriverLicenseType
                    .Select(x => x.DriverLicenseTypeID)
                    .ToList()
                };

                // get data for selectlists
                var fullModel = TryInit(model);

                return fullModel;
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryEdit(int id)", e);
            }
        }
        #endregion

        #region EDIT/UPDATE - (POST)
        /// <summary>
        /// converts new values of driver to db object and asks repo to update this object.
        /// </summary>
        /// <param name="model">updated values driver</param>
        /// <returns>id of driver</returns>
        public int TryEdit(DriverModel model)
        {
            try
            {
                if (model.DriverID <= 0)
                    throw new DriverException("Error in TryEdit(DriverModel model) - ID is invalid.");

                var driver = _driverRepo.Get(model.DriverID);

                if (driver == null)
                    throw new DriverException("Error in TryEdit(DriverModel model) - Db object is NULL.");


                if (model.NationalInsuranceNr.Trim() != driver.NationalInsuranceNr.Trim() &&
                    _driverRepo.DoesNationalInsuranceNrExists(model.NationalInsuranceNr))
                {
                    //throw new DriverException("Error in TryEdit(DriverModel model) - NationalInsuranceNr already in use.");
                    return 0;
                }
                else
                {
                    if (!checkNationalInsuranceNr(model.NationalInsuranceNr))
                    {
                        throw new DriverException("Error in TryCreate(DriverModel model) - NationalInsuranceNr number is not valid.");
                    }
                        
                    
                    if (!_driverRepo.DoesNationalInsuranceNrIsValid(model.NationalInsuranceNr))
                    {
                        throw new DriverException("Error in TryEdit(DriverModel model) - NationalInsuranceNr not valid.");
                    }
                       
                }

                driver.LastName = model.LastName;
                driver.FirstName = model.FirstName;
                driver.DateOfBirth = model.DateOfBirth;
                driver.NationalInsuranceNr = model.NationalInsuranceNr;
                driver.VehicleID = model.VehicleID;
                driver.FuelCardID = model.FuelCardID;
                driver.Address.Street = model.Address.Street;
                driver.Address.Number = model.Address.Number;
                driver.Address.Zipcode = model.Address.Zipcode;
                driver.Address.Place = model.Address.Place;

                // reset vehicle in driver if vehicle in model is null
                if (model.VehicleID == null)
                {
                    if (driver.Vehicle != null) 
                        driver.Vehicle.DriverID = null;
                }
                else //Update kolom DriverID in Vehicle -> gebeurt niet automatisch
                {
                    var vehicle = _vehicleRepo.Get((int)model.VehicleID);
                    vehicle.DriverID = driver.DriverID;
                }

                // reset fuelcard in driver if fuelcard in model is null
                if (model.FuelCardID == null)
                {
                    if (driver.FuelCard != null)
                        driver.FuelCard.DriverID = null;
                }
                else //Update kolom DriverID in FuelCard -> gebeurt niet automatisch
                {
                    var fuelCard = _fuelCardRepo.Get((int)model.FuelCardID);
                    fuelCard.DriverID = driver.DriverID;
                }

                if (model.DriverLicenseTypeID != null)
                {
                    foreach (var licenseType in driver.DriverDriverLicenseType)
                    {
                        _driverLicenseTypeRepo
                            .DeleteDriverDriverLicenseType(driver.DriverID, licenseType.DriverLicenseTypeID);
                    }

                    foreach (var driverLicenseID in model.DriverLicenseTypeID)
                    {
                        driver.DriverDriverLicenseType.Add(new DriverDriverLicenseType
                        {
                            DriverLicenseTypeID = driverLicenseID
                        });
                    }
                }

                var id = _driverRepo.Update(driver);

                return id;
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryEdit(DriverModel model)", e);
            }
        }
        #endregion

        #endregion

        #region HELPERS
        /// <summary>
        /// Check for valid NationalInsuranceNr
        /// </summary>
        /// <returns>bool</returns>
        public bool checkNationalInsuranceNr(string rrn) 
        {
             var bestaandeControleGetal = rrn.Substring(9, 2);
             var deelVanStringIndienGeborenVoor2000 = rrn.Substring(0, 9);
             var getal = Int64.Parse(deelVanStringIndienGeborenVoor2000);
             var berekendControleGetal = 97 - (getal % 97);
                if (Int64.Parse(bestaandeControleGetal) == berekendControleGetal)
                {
                    return true;
                }
                else
                {
                    string deelVanStringIndienGeborenNa2000 = "2" + deelVanStringIndienGeborenVoor2000;
                    getal = Int64.Parse(deelVanStringIndienGeborenNa2000);
                    berekendControleGetal = 97 - (getal % 97);
                    if (Int64.Parse(bestaandeControleGetal) == berekendControleGetal)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
        }


        /// <summary>
        /// sets all selectlists in drivermodel for create/edit forms
        /// </summary>
        /// <returns>DriverModel</returns>
        /// <exception cref="DriverException">if something goes wrong in getting data from repo</exception>
        public DriverModel TryInit(DriverModel? model = null)
        {
            try
            {
                if (model == null)
                {
                    model = new DriverModel
                    {
                        DriverLicenseTypeSelectList = new MultiSelectList(
                            _driverLicenseTypeRepo.GetAll(), 
                            "DriverLicenseTypeID", 
                            "Name"),
                        FuelCardSelectList = new SelectList(
                            _fuelCardRepo.GetOnlyAvailableFuelCards(), 
                            "FuelCardID", 
                            "CardNumber"),
                        VehicleSelectList = new SelectList(
                            _vehicleRepo.GetOnlyAvailableVehicles(), 
                            "VehicleID", 
                            "LicensePlate")
                    };

                    return model;
                }

                model.DriverLicenseTypeSelectList = new MultiSelectList(
                    _driverLicenseTypeRepo.GetAll(),
                    "DriverLicenseTypeID", "Name",
                    model.DriverLicenseTypeID);

                model.FuelCardSelectList = new SelectList(
                    _fuelCardRepo.GetOnlyAvailableFuelCards(model.FuelCardID),
                    "FuelCardID", "CardNumber",
                    model.FuelCardID);

                model.VehicleSelectList = new SelectList(
                    _vehicleRepo.GetOnlyAvailableVehicles(model.VehicleID),
                    "VehicleID", "LicensePlate",
                    model.VehicleID);

                return model;
            }
            catch (Exception e)
            {
                throw new DriverException("Error in TryInit(DriverModel? model = null)", e);
            }
        }
        #endregion
    }
}
