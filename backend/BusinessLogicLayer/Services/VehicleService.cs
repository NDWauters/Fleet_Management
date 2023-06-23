using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels;
using BusinessLogicLayer.ViewModels.Vehicle;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLogicLayer.Services
{   
    /// <summary>
    /// class which contains all business logic of the vehicle domain
    /// </summary>
    public class VehicleService : IVehicleService
    {
        #region PROPS
        private readonly IVehicleRepo _vehicleRepo;
        private readonly IBrandRepo _brandRepo;
        private readonly IFuelTypeRepo _fuelTypeRepo;
        private readonly IVehicleTypeRepo _vehicleTypeRepo;
        private readonly IDriverRepo _driverRepo;
        #endregion

        #region CONSTR
        public VehicleService(
            IVehicleRepo vehicleRepo, 
            IBrandRepo brandRepo, 
            IFuelTypeRepo fuelTypeRepo, 
            IVehicleTypeRepo vehicleTypeRepo, 
            IDriverRepo driverRepo)
        {
            _vehicleRepo = vehicleRepo;
            _brandRepo = brandRepo;
            _fuelTypeRepo = fuelTypeRepo;
            _vehicleTypeRepo = vehicleTypeRepo;
            _driverRepo = driverRepo;
        }
        #endregion

        #region SELECTLISTS
        /// <summary>
        /// sets all selectlists in vehiclemodel for create/edit forms
        /// </summary>
        /// <returns>VehicleModel</returns>
        /// <exception cref="VehicleException">if something goes wrong in getting data from repo</exception>
        public VehicleModel TryInit(VehicleModel? model = null)
        {
            try
            {
                if (model == null)
                {
                    model = new VehicleModel
                    { 
                        BrandSelectList = new SelectList(
                            _brandRepo.GetAll(),
                            "BrandID", "Name"),
                        FuelTypeSelectList = new SelectList(
                            _fuelTypeRepo.GetAll(),
                            "FuelTypeID", "Name"),
                        VehicleTypeSelectList = new SelectList(
                            _vehicleTypeRepo.GetAll(),
                            "VehicleTypeID", "Name"),
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

                model.BrandSelectList = new SelectList(
                    _brandRepo.GetAll(),
                    "BrandID", "Name", 
                    model.BrandID);

                model.FuelTypeSelectList = new SelectList(
                    _fuelTypeRepo.GetAll(),
                    "FuelTypeID", "Name", 
                    model.FuelTypeID);

                model.VehicleTypeSelectList = new SelectList(
                    _vehicleTypeRepo.GetAll(),
                    "VehicleTypeID", "Name", 
                    model.VehicleTypeID);

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
                throw new VehicleException(e.Message);
            }
        }
        #endregion

        #region INDEX
        /// <summary>
        /// Gets all vehicles from repo and converts it to dto objects
        /// </summary>
        /// <returns>IEnumerable of VehicleListViewModels</returns>
        /// <exception cref="VehicleException">If something goes wrong with getting data from repo.</exception>
        public IEnumerable<VehicleListViewModel> TryGet()
        {
            try
            {
                var vehicles = _vehicleRepo.GetAll();

                return vehicles
                    .Select(x => new VehicleListViewModel
                    {
                        VehicleID = x.VehicleID,
                        Model = x.Model,
                        LicensePlate = x.LicensePlate,
                        Vin = x.Vin,
                        Brand = x.Brand.Name,
                        VehicleType = x.VehicleType.Name,
                        FuelType = x.FuelType.Name,
                        Color = x.Color,
                        AmountDoors = x.AmountDoors,
                        Driver = x.Driver != null 
                            ? $"{x.Driver?.LastName} {x.Driver?.FirstName.Substring(0,1)}." 
                            : "",
                        DriverID = x.DriverID,
                        FuelCard = x.Driver?.FuelCard?.CardNumber ?? "",
                        FuelCardID = x.Driver?.FuelCardID
                    });
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }
        #endregion

        #region DETAILS
        /// <summary>
        /// Gets data of a specific vehicle from repo and converts it to dto object
        /// </summary>
        /// <param name="id">id of specific vehicle</param>
        /// <returns>VehicleViewModel</returns>
        /// <exception cref="VehicleException">Invalid ID or db object null</exception>
        public VehicleViewModel TryGet(int id)
        {
            try
            {
                if (id <= 0) throw new VehicleException("ID is invalid.");

                var vehicle = _vehicleRepo.Get(id);

                if (vehicle == null) throw new VehicleException("Db objeci is NULL.");

                return new VehicleViewModel
                {
                    Model = vehicle.Model,
                    AmountDoors = vehicle.AmountDoors,
                    Brand = vehicle.Brand.Name,
                    DriverLastName = vehicle.Driver?.LastName ?? "",
                    DriverFirstName = vehicle.Driver?.FirstName ?? "",
                    Color = vehicle.Color,
                    DriverID = vehicle.DriverID,
                    FuelCard = vehicle.Driver?.FuelCard?.CardNumber ?? "",
                    FuelCardID = vehicle.Driver?.FuelCardID,
                    FuelType = vehicle.FuelType.Name,
                    LicensePlate = vehicle.LicensePlate,
                    VehicleID = vehicle.VehicleID,
                    VehicleType = vehicle.VehicleType.Name,
                    Vin = vehicle.Vin,
                };
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }
        #endregion

        #region REMOVE
        /// <summary>
        /// Checks given id and asks repo to soft delete vehicle with this id.
        /// </summary>
        /// <param name="id">ID of specific vehicle</param>
        /// <exception cref="VehicleException">if id is invalid or somthing goes wrong in db</exception>
        public void TryRemove(int id)
        {
            try
            {
                if (id <= 0) throw new VehicleException("ID is invalid.");

                _vehicleRepo.Delete(id);
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }
        #endregion

        #region CREATE
        /// <summary>
        /// Converts dto to db object and asks repo to add this vehicle to db
        /// </summary>
        /// <param name="model">DTO</param>
        /// <returns>ID of new vehicle</returns>
        public int TryCreate(VehicleModel model)
        {
            try
            {
                if (_vehicleRepo.DoesVinExists(model.Vin)) 
                {
                    return -1;
                }

                if (_vehicleRepo.DoesLicensePlateExists(model.LicensePlate))
                {
                    return -2;
                }

                var vehicle = new Vehicle
                {
                    Model = model.Model,
                    BrandID = model.BrandID,
                    FuelTypeID = model.FuelTypeID,
                    VehicleTypeID = model.VehicleTypeID,
                    Color = model.Color,
                    AmountDoors = model.AmountDoors,
                    LicensePlate = model.LicensePlate,
                    DriverID = model.DriverID,
                     Vin = model.Vin,
                    IsActive = true
                };

                int newID = _vehicleRepo.Add(vehicle);

                return newID;
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }
        #endregion

        #region EDIT
        /// <summary>
        /// Sets properties of dto by properties of vehicle object from db
        /// </summary>
        /// <param name="id">id of vehicle</param>
        /// <returns>VehicleModel</returns>
        public VehicleModel TryEdit(int id)
        {
            try
            {
                if (id <= 0) throw new VehicleException("ID is invalid.");

                var vehicle = _vehicleRepo.Get(id);

                if (vehicle == null) throw new VehicleException("Db object is NULL.");

                var model = new VehicleModel 
                { 
                    VehicleID = id,
                    Model = vehicle.Model,
                    BrandID = vehicle.BrandID,
                    FuelTypeID = vehicle.FuelTypeID,
                    VehicleTypeID = vehicle.VehicleTypeID,
                    DriverID = vehicle.DriverID,
                    Vin = vehicle.Vin,
                    LicensePlate = vehicle.LicensePlate,
                    AmountDoors = vehicle.AmountDoors,
                    Color = vehicle.Color
                };

                // get data for selectlists
                var fullModel = TryInit(model);

                return fullModel;
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }

        /// <summary>
        /// converts new values of vehicle to db object and asks repo to update this object.
        /// </summary>
        /// <param name="model">updated values vehicle</param>
        /// <returns>id of vehicle</returns>
        public int TryEdit(VehicleModel model)
        {
            try
            {
                if (model.VehicleID <= 0) 
                    throw new VehicleException("ID is invalid.");

                var vehicle = _vehicleRepo.Get(model.VehicleID);

                if (vehicle == null) 
                    throw new VehicleException("Db object is NULL.");

                if (model.Vin.Trim() != vehicle.Vin.Trim() &&
                    _vehicleRepo.DoesVinExists(model.Vin))
                    return -1;

                if (model.LicensePlate.Trim() != vehicle.LicensePlate.Trim() &&
                    _vehicleRepo.DoesLicensePlateExists(model.LicensePlate))
                    return -2;

                vehicle.Model = model.Model;
                vehicle.BrandID = model.BrandID;
                vehicle.FuelTypeID = model.FuelTypeID;
                vehicle.VehicleTypeID = model.VehicleTypeID;
                vehicle.DriverID = model.DriverID;
                vehicle.Vin = model.Vin;
                vehicle.LicensePlate = model.LicensePlate;
                vehicle.AmountDoors = model.AmountDoors;
                vehicle.Color = model.Color;

                if (model.DriverID == null && vehicle.Driver != null)
                {
                    vehicle.Driver.VehicleID = null;
                }

                var id = _vehicleRepo.Update(vehicle);

                return id;
            }
            catch (Exception e)
            {
                throw new VehicleException(e.Message);
            }
        }
        #endregion
    }
}
