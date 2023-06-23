using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.ViewModels.Driver;
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
    public class DriverControllerTests
    {
        #region PROPS
        private readonly Mock<IDriverRepo> _driverRepoMock;
        private readonly Mock<IAddressRepo> _addressRepoMock;
        private readonly Mock<IDriverLicenseTypeRepo> _driverLicenseTypeRepoMock;
        private readonly Mock<IFuelCardRepo> _fuelCardRepoMock;
        private readonly Mock<IVehicleRepo> _vehicleRepoMock;

        //private readonly Mock<ILogger<DriverController>> _logger;
        private readonly Mock<ILoggerFactory> _logger;
        private readonly Mock<IUrlHelper> _mockUrlHelper;

        private readonly IDriverService _driverService;
        private readonly DriverController _driverController;

        private readonly Driver _driver;
        private readonly List<Driver> _drivers;

        private readonly DriverDriverLicenseType _driverDriverLicenseType;
        private readonly List<DriverDriverLicenseType> _driverDriverLicenseTypes;
        #endregion

        #region CONSTR
        public DriverControllerTests()
        {
            // init repos
            _driverRepoMock = new Mock<IDriverRepo>();
            _addressRepoMock = new Mock<IAddressRepo>();
            _driverLicenseTypeRepoMock = new Mock<IDriverLicenseTypeRepo>();
            _fuelCardRepoMock = new Mock<IFuelCardRepo>();
            _vehicleRepoMock = new Mock<IVehicleRepo>();

            // init service
            _driverService = new DriverService(
                _driverRepoMock.Object,
                _addressRepoMock.Object,
                _driverLicenseTypeRepoMock.Object,
                _fuelCardRepoMock.Object,
                _vehicleRepoMock.Object);

            // init logger and urlhelper
            _logger = new Mock<ILoggerFactory>();
            var mockControllerLogger = new Mock<ILogger<DriverController>>();

            _logger.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(() => mockControllerLogger.Object);

            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // init controller
            _driverController = new DriverController(_driverService, _logger.Object)
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
            _driver = new Driver()
            {
                DriverID = 1,
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                AddressID = 1,
                Address = new Address { AddressID = 1, Street = "Kerkstraat", Number = 33, Place = "Beveren", Zipcode = 9620 },
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                FuelCard = new FuelCard { FuelCardID = 1, CardNumber = "789456", ExpirationDate = new DateTime(2024 - 02 - 02), Pincode = "1245", DriverID = 1, IsDisabled = false, IsActive = true },
                VehicleID = 1,
                Vehicle = new Vehicle { VehicleID = 1, Model = "Golf", LicensePlate = "1-EDR-235", Vin = "23785612", BrandID = 1, VehicleTypeID = 1, FuelTypeID = 1, Color = "", AmountDoors = 4, DriverID = 1, IsActive = true, },
                IsActive = true,
            };
            _drivers = new List<Driver> { _driver };

            _driverDriverLicenseType = new DriverDriverLicenseType() { DriverLicenseTypeID = 1, DriverID = 1 };
            _driverDriverLicenseTypes = new List<DriverDriverLicenseType> { _driverDriverLicenseType };
            _driver.DriverDriverLicenseType = _driverDriverLicenseTypes;
        }
        #endregion

        #region INDEX
        [Fact]
        public void Index_ShouldReturnOkObjectResult()
        {
            // asign
            var drivers = new List<Driver>();

            _driverRepoMock
                .Setup(x => x.GetAll())
                .Returns(drivers);

            // act
            var result = _driverController.Index();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }
       
        [Fact]
        public void Index_ShouldReturnNoContentObjectResult()
        {
            // asign
            _driverRepoMock
                .Setup(x => x.GetAll())
                .Throws<Exception>();

            // act
            var result = _driverController.Index();

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
            var result = _driverController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_DbObject_null_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 2;
            Driver? driver = null;

            _driverRepoMock
                .Setup(x => x.Get(id))
                .Returns(driver);

            // act
            var result = _driverController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_DbException_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 1;
            _driverRepoMock
                .Setup(x => x.Get(id))
                .Throws<Exception>();

            // act
            var result = _driverController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_ValidID_ShouldReturnOkObjectResult()
        {
            // asign
            var id = 1;
            _driverRepoMock
                .Setup(x => x.Get(id))
                .Returns(_driver);

            // act
            var result = _driverController.Details(id);

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Details_ValidID_ShouldReturnDriverViewModel()
        {
            // asign
            var id = 1;
            _driverRepoMock
                .Setup(x => x.Get(id))
                .Returns(_driver);

            // act
            var result = _driverController.Details(id);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DriverViewModel>(okObjectResult.Value);
            Assert.Equal(id, model.DriverID);
            Assert.Equal("Bibber", model.LastName);
            Assert.Equal("Bert", model.FirstName);
            Assert.Equal(new DateTime(1999 - 02 - 04), model.DateOfBirth);
            Assert.Equal("02021518897", model.NationalInsuranceNr);
            Assert.Equal(1, model.AddressID);
            //Assert.Equal(1, model.Address.);
            Assert.Equal(1, model.FuelCardID);
            //Assert.Equal(1, model.FuelCard.);
            Assert.Equal(1, model.VehicleID);
            //Assert.Equal(1, model.Vehicle.);
            //Assert.Null(List<int> model.DriverLicenseTypeID);

        }
        #endregion

        #region REMOVE
        [Fact]
        public void Remove_InvalidID_ShouldReturnBadRequest()
        {
            // asign
            var id = 0;

            // act
            var result = _driverController.Remove(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Remove_DbException_ShouldReturnBadRequest()
        {
            // asign
            var id = 1;
            _driverRepoMock
                .Setup(x => x.Delete(id))
                .Throws<Exception>();

            // act
            var result = _driverController.Remove(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Remove_ValidID_ShouldReturnNoContentResult()
        {
            // asign
            var id = 1;
            _driverRepoMock
                .Setup(x => x.Delete(id))
                .Verifiable();

            _vehicleRepoMock.Setup(x => x.GetAll()).Returns(new List<Vehicle>());
            _fuelCardRepoMock.Setup(x => x.GetAll()).Returns(new List<FuelCard>());

            // act
            var result = _driverController.Remove(id);

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
            _driverRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Driver>());

            _driverLicenseTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<DriverLicenseType>());

             _fuelCardRepoMock
                .Setup(x => x.GetOnlyAvailableFuelCards(null))
                .Returns(new List<FuelCard>());

            _vehicleRepoMock
                .Setup(x => x.GetOnlyAvailableVehicles(null))
                .Returns(new List<Vehicle>());

            // act
            var result = _driverController.Create();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Create_GET_ShouldReturnDriverModel()
        {
            // asign
            _driverRepoMock
               .Setup(x => x.GetAll())
               .Returns(new List<Driver>());

            _driverLicenseTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<DriverLicenseType>());

             _fuelCardRepoMock
                .Setup(x => x.GetOnlyAvailableFuelCards(null))
                .Returns(new List<FuelCard>());
            
            _vehicleRepoMock
                .Setup(x => x.GetOnlyAvailableVehicles(null))
                .Returns(new List<Vehicle>());
            
            // act
            var result = _driverController.Create();

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DriverModel>(okObjectResult.Value);
            Assert.NotNull(model.DriverLicenseTypeSelectList);
            //Assert.NotNull(model.FuelCardSelectList);
            //Assert.NotNull(model.VehicleSelectList);
        }

        [Fact]
        public void Create_GET_DbException_ShouldReturnBadRequestObjectResult()
        {
            // asign
            _driverRepoMock
               .Setup(x => x.GetAll())
               .Returns(new List<Driver>());

            _driverLicenseTypeRepoMock
                .Setup(x => x.GetAll())
                //.Returns(new List<DriverLicenseType>());
                .Throws<Exception>();

            _fuelCardRepoMock
                .Setup(x => x.GetAll())
                //.Throws<Exception>();
                .Returns(new List<FuelCard>());

            _vehicleRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Vehicle>());

            // act
            var result = _driverController.Create();

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region POST
        [Fact]
        public void Create_Post_InvalidModelState_ShouldReturnBadRequest()
        {
            // asign
            var model = new DriverModel
            {
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
            };

            // act
            _driverController.ModelState
                .AddModelError("ModelState is not valid.", "Property is required.");

            var result = _driverController.Create(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Create_Post_NationalInsuranceNrAlreadyExists_ShouldReturnCreatedAtActionResultWith0()
        {
            // asign
            var model = new DriverModel
            {
                LastName = "Tommer",
                FirstName = "Tom",
                DateOfBirth = new DateTime(2004 - 08 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
                
            };

            _driverRepoMock
                .Setup(x => x.DoesNationalInsuranceNrExists(model.NationalInsuranceNr))
                .Returns(true);

            // act
            var result = _driverController.Create(model);

            // assert
           Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Create_Post_ValidObject_ShouldReturnCreatedAtAction()
        {
            // asign
            var model = new DriverModel
            {
                LastName = "Tommer",
                FirstName = "Tom",
                DateOfBirth = new DateTime(2004 - 08 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
            };

            _driverRepoMock
                .Setup(x => x.DoesNationalInsuranceNrExists(model.NationalInsuranceNr))
                .Returns(false);

            _vehicleRepoMock.Setup(x => x.Get(1)).Returns(new Vehicle());
            _fuelCardRepoMock.Setup(x => x.Get(1)).Returns(new FuelCard());

            // act
            var result = _driverController.Create(model);

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
            var result = _driverController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Get_DbObject_Null_ShouldReturnBadRequest()
        {
            // asign
            var id = 2;
            Driver? driver = null;

            _driverRepoMock
                .Setup(x => x.Get(id))
                .Returns(driver);

            // act
            var result = _driverController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Get_DbException_ShouldReturnBadRequest()
        {
            // asign
            var id = 2;
            Driver? driver = null;

            _driverRepoMock
                .Setup(x => x.Get(id))
                .Throws<Exception>();

            // act
            var result = _driverController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Get_ValidID_ShouldReturnOkObjectResult()
        {
            // asign
            var id = 1;

            _driverRepoMock
               .Setup(x => x.Get(id))
               .Returns(_driver);

            _driverLicenseTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<DriverLicenseType>());

             _fuelCardRepoMock
                .Setup(x => x.GetOnlyAvailableFuelCards(id))
                .Returns(new List<FuelCard>());

            _vehicleRepoMock
                 .Setup(x => x.GetOnlyAvailableVehicles(id))
                 .Returns(new List<Vehicle>());

            // act
            var result = _driverController.Edit(id);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DriverModel>(okObjectResult.Value);
            Assert.NotNull(model);
            Assert.NotNull(model.DriverLicenseTypeSelectList);
            Assert.NotNull(model.FuelCardSelectList);
            Assert.NotNull(model.VehicleSelectList);
           // Assert.Null(model.VehicleD);
            Assert.Equal(_driver.LastName, model.LastName);
            Assert.Equal(_driver.FirstName, model.FirstName);
            Assert.Equal(_driver.DateOfBirth, model.DateOfBirth);
            Assert.Equal(_driver.NationalInsuranceNr, model.NationalInsuranceNr);
            //Assert.Equal(_driver.DriverLicenseTypeID, model.DriverLicenseTypeID);
            Assert.Equal(_driver.FuelCardID, model.FuelCardID);
            Assert.Equal(_driver.VehicleID, model.VehicleID);

        }
        #endregion

        #region PUT
        [Fact]
        public void Edit_Put_InvalidModelState_ShouldReturnBadRequest()
        {
            // asign
            var model = new DriverModel
            {
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
            };

            // act
            _driverController.ModelState
                .AddModelError("ModelState is not valid.", "Property is required.");

            var result = _driverController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Put_InvalidID_Model_ShouldReturnBadRequest()
        {
            // asign
            var model = new DriverModel
            {
                DriverID = 0,
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
            };

            _driverRepoMock
               .Setup(x => x.Get(model.DriverID))
               .Returns(_driver);

            // act
            var result = _driverController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Put_DbObject_Null_ShouldReturnBadRequest()
        {
            // asign
            var model = new DriverModel
            {
                DriverID = 1,
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
            };

            Driver? driver = null;

            _driverRepoMock
                .Setup(x => x.Get(model.DriverID))
                .Returns(driver);

            // act
            var result = _driverController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Put_NationalInsuranceNrAlreadyExists_ShouldReturnBadRequest()
        {
            // asign
            var model = new DriverModel
            {
                DriverID = 1,
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                NationalInsuranceNr = "02021518897",
                FuelCardID = 1,
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },

            };

            _driverRepoMock
                .Setup(x => x.Get(model.DriverID))
                .Returns(_driver);

            _driverRepoMock
                .Setup(x => x.DoesNationalInsuranceNrExists(model.NationalInsuranceNr))
                .Returns(true);

            // act
            var result = _driverController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void Edit_Put_AllConditions_OK_ShouldReturnCreatedAtAction()
        {
            // asign
            var model = new DriverModel
            {
                DriverID = 1,
                LastName = "Bibber",
                FirstName = "Bert",
                DateOfBirth = new DateTime(1999 - 02 - 04),
                //NationalInsuranceNr = "02021518897",
                NationalInsuranceNr = "99020422304",
                VehicleID = 1,
                Address = new AddressModel
                {
                    Street = "Putstraat",
                    Number = 44,
                    Place = "Gent",
                    Zipcode = 9000
                },
            };

            _driverRepoMock
                .Setup(x => x.Get(model.DriverID))
                .Returns(_driver);
            
            _driverRepoMock
                .Setup(x => x.DoesNationalInsuranceNrExists(model.NationalInsuranceNr))
                .Returns(false);

            _driverRepoMock
                .Setup(x => x.DoesNationalInsuranceNrIsValid(model.NationalInsuranceNr))
                .Returns(true);

            _vehicleRepoMock.Setup(x => x.Get(1)).Returns(new Vehicle());
            _fuelCardRepoMock.Setup(x => x.Get(1)).Returns(new FuelCard());



            // act
            var result = _driverController.Edit(model);

            // assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Edit", createdAtActionResult.ActionName);
        }
        #endregion

        #endregion
    }
}
