using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.ViewModels.Vehicle;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using FleetService.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestLayer
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleRepo> _vehicleRepoMock;
        private readonly Mock<IBrandRepo> _brandRepoMock;
        private readonly Mock<IFuelTypeRepo> _fuelTypeRepoMock;
        private readonly Mock<IVehicleTypeRepo> _vehicleTypeRepoMock;
        private readonly Mock<IDriverRepo> _driverRepoMock;

        private readonly IVehicleService _vehicleService;
        private readonly VehicleController _vehicleController;
        private readonly Mock<ILoggerFactory> _logger;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly Vehicle _vehicle;
        private readonly List<Vehicle> _vehicles;

        public VehicleControllerTests()
        {
            // init repos
            _vehicleRepoMock = new Mock<IVehicleRepo>();
            _brandRepoMock = new Mock<IBrandRepo>();
            _fuelTypeRepoMock = new Mock<IFuelTypeRepo>();
            _vehicleTypeRepoMock = new Mock<IVehicleTypeRepo>();
            _driverRepoMock = new Mock<IDriverRepo>();

            // init service
            _vehicleService = new VehicleService(
                _vehicleRepoMock.Object,
                _brandRepoMock.Object,
                _fuelTypeRepoMock.Object,
                _vehicleTypeRepoMock.Object,
                _driverRepoMock.Object);

            // init logger and urlhelper
            _logger = new Mock<ILoggerFactory>();
            var mockControllerLogger = new Mock<ILogger<VehicleController>>();

            _logger.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(() => mockControllerLogger.Object);

            //_logger = new Mock<ILoggerFactory>();
            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // init controller
            _vehicleController = new VehicleController(_vehicleService, _logger.Object)
            {
                Url = _mockUrlHelper.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                },
            };

            // setup urlhelper
            _mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns(It.IsAny<string>())
                .Verifiable();

            // init seed data
            _vehicle = new Vehicle
            {
                VehicleID = 1,
                AmountDoors = 5,
                BrandID = 1,
                Brand = new Brand { BrandID = 1, Name = "Mercedes" },
                Color = "Grijs",
                FuelTypeID = 1,
                FuelType = new FuelType { FuelTypeID = 1, Name = "E95" },
                LicensePlate = "1ABC123",
                IsActive = true,
                Model = "C180",
                Vin = "test",
                VehicleTypeID = 1,
                VehicleType = new VehicleType { VehicleTypeID = 1, Name = "Personenwagen" }
            };

            _vehicles = new List<Vehicle> { _vehicle };
        }

        #region INDEX

        [Fact]
        public void Index_ShouldReturnOkObjectResult()
        {
            // asign
            var vehicles = new List<Vehicle>();

            _vehicleRepoMock
                .Setup(x => x.GetAll())
                .Returns(vehicles);

            // act
            var result = _vehicleController.Index();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Index_ShouldReturnNoContentObjectResult()
        {
            // asign
            _vehicleRepoMock
                .Setup(x => x.GetAll())
                .Throws<Exception>();

            // act
            var result = _vehicleController.Index();

            // assert
            Assert.IsType<NoContentResult>(result);
        }

        #endregion

        #region DETAILS

        [Fact]
        public void Details_InvalidID_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 0;

            // act
            var result = _vehicleController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_DbObject_null_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 2;
            Vehicle? vehicle = null;

            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Returns(vehicle);

            // act
            var result = _vehicleController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_DbException_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 1;
            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Throws<Exception>();

            // act
            var result = _vehicleController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_ValidID_ShouldReturnOkObjectResult()
        {
            // asign
            var id = 1;
            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Returns(_vehicle);

            // act
            var result = _vehicleController.Details(id);

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Details_ValidID_ShouldReturnVehicleViewModel()
        {
            // asign
            var id = 1;
            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Returns(_vehicle);

            // act
            var result = _vehicleController.Details(id);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<VehicleViewModel>(okObjectResult.Value);
            Assert.Equal(id, model.VehicleID);
            Assert.Equal("C180", model.Model);
            Assert.Equal("Mercedes", model.Brand);
            Assert.Null(model.DriverID);
            Assert.Equal("E95", model.FuelType);
            Assert.Equal("Personenwagen", model.VehicleType);
            Assert.Equal("test", model.Vin);
            Assert.Equal("1ABC123", model.LicensePlate);
            Assert.Equal("Grijs", model.Color);
            Assert.Equal(5, model.AmountDoors);
        }

        #endregion

        #region REMOVE

        [Fact]
        public void Remove_InvalidID_ShouldReturnBadRequest()
        {
            // asign
            var id = 0;

            // act
            var result = _vehicleController.Remove(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Remove_DbException_ShouldReturnBadRequest()
        {
            // asign
            var id = 1;
            _vehicleRepoMock
                .Setup(x => x.Delete(id))
                .Throws<Exception>();

            // act
            var result = _vehicleController.Remove(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Remove_ValidID_ShouldReturnNoContentResult()
        {
            // asign
            var id = 1;
            _vehicleRepoMock
                .Setup(x => x.Delete(id))
                .Verifiable();

            // act
            var result = _vehicleController.Remove(id);

            // assert
            Assert.IsType<NoContentResult>(result);
        }

        #endregion

        #region CREATE

        #region GET

        [Fact]
        public void Create_GET_ShouldReturnOkObjectResult()
        {
            // asign
            _brandRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Brand>());

            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _vehicleTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<VehicleType>());

            _driverRepoMock
                .Setup(x => x.GetOnlyAvailableDrivers(null))
                .Returns(new List<Driver>());

            // act
            var result = _vehicleController.Create();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Create_GET_ShouldReturnVehicleModel()
        {
            // asign
            _brandRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Brand>());

            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _vehicleTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<VehicleType>());

            _driverRepoMock
                .Setup(x => x.GetOnlyAvailableDrivers(null))
                .Returns(new List<Driver>());

            // act
            var result = _vehicleController.Create();

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<VehicleModel>(okObjectResult.Value);
            Assert.NotNull(model.BrandSelectList);
            Assert.NotNull(model.DriverSelectList);
            Assert.NotNull(model.FuelTypeSelectList);
            Assert.NotNull(model.VehicleTypeSelectList);
        }

        [Fact]
        public void Create_GET_DbException_ShouldReturnBadRequestObjectResult()
        {
            // asign
            _brandRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Brand>());

            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _vehicleTypeRepoMock
                .Setup(x => x.GetAll())
                .Throws<Exception>();

            _driverRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Driver>());

            // act
            var result = _vehicleController.Create();

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region POST

        [Fact]
        public void Create_Post_InvalidModelState_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            // act
            _vehicleController.ModelState
                .AddModelError("ModelState is not valid.", "Property is required.");

            var result = _vehicleController.Create(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Create_Post_LicensePlateAlreadyExists_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
                .Setup(x => x.DoesLicensePlateExists(model.LicensePlate))
                .Returns(true);

            // act
            var result = _vehicleController.Create(model);

            // assert
            var okObject = Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Create_Post_VinAlreadyExists_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
                .Setup(x => x.DoesVinExists(model.Vin))
                .Returns(true);

            // act
            var result = _vehicleController.Create(model);

            // assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Create_Post_ValidObject_ShouldReturnCreatedAtAction()
        {
            // asign
            var model = new VehicleModel
            {
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
                .Setup(x => x.DoesVinExists(model.Vin))
                .Returns(false);

            _vehicleRepoMock
                .Setup(x => x.DoesLicensePlateExists(model.LicensePlate))
                .Returns(false);

            // act
            var result = _vehicleController.Create(model);

            // assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Create", createdAtActionResult.ActionName);
        }

        #endregion

        #endregion

        #region EDIT

        #region GET

        [Fact]
        public void Edit_Get_InvalidID_ShouldReturnBadRequest()
        {
            // asign
            var id = 0;

            // act
            var result = _vehicleController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Get_DbObject_Null_ShouldReturnBadRequest()
        {
            // asign
            var id = 2;
            Vehicle? vehicle = null;

            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Returns(vehicle);

            // act
            var result = _vehicleController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Get_DbException_ShouldReturnBadRequest()
        {
            // asign
            var id = 2;
            Vehicle? vehicle = null;

            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Throws<Exception>();

            // act
            var result = _vehicleController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Get_ValidID_ShouldReturnOkObjectResult()
        {
            // asign
            var id = 1;

            _vehicleRepoMock
                .Setup(x => x.Get(id))
                .Returns(_vehicle);

            _brandRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Brand>());

            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _vehicleTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<VehicleType>());

            _driverRepoMock
                .Setup(x => x.GetOnlyAvailableDrivers(null))
                .Returns(new List<Driver>());

            // act
            var result = _vehicleController.Edit(id);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<VehicleModel>(okObjectResult.Value);
            Assert.NotNull(model);
            Assert.NotNull(model.BrandSelectList);
            Assert.NotNull(model.FuelTypeSelectList);
            Assert.NotNull(model.VehicleTypeSelectList);
            Assert.NotNull(model.DriverSelectList);
            Assert.Null(model.DriverID);
            Assert.Equal(_vehicle.Model, model.Model);
            Assert.Equal(_vehicle.BrandID, model.BrandID);
            Assert.Equal(_vehicle.FuelTypeID, model.FuelTypeID);
            Assert.Equal(_vehicle.VehicleTypeID, model.VehicleTypeID);
            Assert.Equal(_vehicle.VehicleID, model.VehicleID);
            Assert.Equal(_vehicle.Vin, model.Vin);
            Assert.Equal(_vehicle.LicensePlate, model.LicensePlate);
            Assert.Equal(_vehicle.Color, model.Color);
            Assert.Equal(_vehicle.AmountDoors, model.AmountDoors);
        }

        #endregion

        #region POST

        [Fact]
        public void Edit_Put_InvalidModelState_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                VehicleID = 1,
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            // act
            _vehicleController.ModelState
                .AddModelError("ModelState is not valid.", "Property is required.");

            var result = _vehicleController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Put_InvalidID_Model_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                VehicleID = 0,
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
               .Setup(x => x.Get(model.VehicleID))
               .Returns(_vehicle);

            // act
            var result = _vehicleController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Put_DbObject_Null_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                VehicleID = 1,
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            Vehicle? vehicle = null;

            _vehicleRepoMock
                .Setup(x => x.Get(model.VehicleID))
                .Returns(vehicle);

            // act
            var result = _vehicleController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Put_LicensePlateAlreadyExists_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                VehicleID = 1,
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
                .Setup(x => x.Get(model.VehicleID))
                .Returns(_vehicle);

            _vehicleRepoMock
                .Setup(x => x.DoesLicensePlateExists(model.LicensePlate))
                .Returns(true);

            // act
            var result = _vehicleController.Edit(model);

            // assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Edit_Put_VinAlreadyExists_ShouldReturnBadRequest()
        {
            // asign
            var model = new VehicleModel
            {
                VehicleID = 1,
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
                .Setup(x => x.Get(model.VehicleID))
                .Returns(_vehicle);

            _vehicleRepoMock
                .Setup(x => x.DoesVinExists(model.Vin))
                .Returns(true);

            // act
            var result = _vehicleController.Edit(model);

            // assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Edit_Put_AllConditions_OK_ShouldReturnCreatedAtAction() 
        {
            // asign
            var model = new VehicleModel
            {
                VehicleID = 1,
                BrandID = 1,
                FuelTypeID = 1,
                VehicleTypeID = 1,
                Color = "Grijs",
                AmountDoors = 5,
                LicensePlate = "Test",
                Model = "Test",
                Vin = "Test",
            };

            _vehicleRepoMock
                .Setup(x => x.Get(model.VehicleID))
                .Returns(_vehicle);

            _vehicleRepoMock
                .Setup(x => x.DoesLicensePlateExists(model.LicensePlate))
                .Returns(false);

            _vehicleRepoMock
                .Setup(x => x.DoesVinExists(model.Vin))
                .Returns(false);

            // act
            var result = _vehicleController.Edit(model); 

            // assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Edit", createdAtActionResult.ActionName);
        }

        #endregion

        #endregion
    }
}