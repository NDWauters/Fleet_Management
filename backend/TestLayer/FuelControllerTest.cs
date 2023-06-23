using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.ViewModels.FuelCard;
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
    public class FuelControllerTest
    {
        // verbindingen met de databank (mock)
        private readonly Mock<IFuelCardRepo> _fuelCardRepoMock;
        private readonly Mock<IDriverRepo> _driverRepoMock;
        private readonly Mock<IFuelTypeRepo> _fuelTypeRepoMock;

        private readonly IFuelCardService _fuelCardService;
        private readonly FuelCardController _fuelCardController;
        private readonly Mock<ILoggerFactory> _logger;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly FuelCard _fuelCard;
        private readonly List<FuelCard> _fuelCards;


        public FuelControllerTest()
        {
            // init repos
            _fuelCardRepoMock = new Mock<IFuelCardRepo>();
            _driverRepoMock = new Mock<IDriverRepo>();
            _fuelTypeRepoMock = new Mock<IFuelTypeRepo>();

            // init service
            _fuelCardService = new FuelCardService(
                _fuelTypeRepoMock.Object,
                _driverRepoMock.Object,
                _fuelCardRepoMock.Object
                );

            // init logger and urlhelper
            _logger = new Mock<ILoggerFactory>();
            var mockControllerLogger = new Mock<ILogger<FuelCardController>>();

            _logger.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(() => mockControllerLogger.Object);

            _mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // init controller
            _fuelCardController = new FuelCardController(_fuelCardService, _logger.Object)
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
            var dateTime = new DateTime(2021, 12, 11, 19, 57, 59);
            _fuelCard = new FuelCard
            {
                FuelCardID = 1,
                CardNumber = "KN1234",
                ExpirationDate = dateTime,
                Pincode = "1234",
                DriverID = null,
                IsDisabled = false,
                IsActive = true,
                FuelCardFuelType = new List<FuelCardFuelType>
                {
                 new FuelCardFuelType
                 {
                     FuelTypeID = 1,
                     FuelType = new FuelType { FuelTypeID = 1, Name = "E95" },
                 },
                  new FuelCardFuelType
                 {
                      FuelTypeID = 2,
                     FuelType = new FuelType { FuelTypeID = 2, Name = "E90" },
                 },
                }
            };

            _fuelCards = new List<FuelCard> { _fuelCard };

        }

        #region INDEX
        [Fact]
        public void Index_ShouldReturnOkObjectResult()
        {
            // asign
            var Fuels = new List<FuelCard>();

            _fuelCardRepoMock
                .Setup(x => x.GetAll())
                .Returns(Fuels);

            // act 
            var result = _fuelCardController.Index();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Index_ShouldReturnNotFoundObjectResult()
        {
            // asign
            _fuelCardRepoMock
                .Setup(x => x.GetAll())
                .Throws<Exception>();

            // act
            var result = _fuelCardController.Index();

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region DETAILS
        [Fact]
        public void Details_ValidID_ShouldReturnOkObjectResult()
        {
            // asign
            var id = 1;
            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Returns(_fuelCard);

            // act
            var result = _fuelCardController.Details(id);

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Details_ValidID_ShouldReturnfuelCardViewModel()
        {
            // asign
            var id = 1;
            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Returns(_fuelCard);

            // act
            var result = _fuelCardController.Details(id);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<FuelCardViewModel>(okObjectResult.Value);
            var dateTime = new DateTime(2021, 12, 11, 19, 57, 59);

            //Equal(id, model.FuelCardID);
            Assert.Equal("KN1234", model.CardNumber);
            Assert.Equal(dateTime, model.ExpirationDate);
            Assert.Equal("1234", model.Pincode);
            Assert.Null(model.DriverID);
            Assert.False(model.IsDisabled);
            Assert.Equal("E95 E90", model.FuelType);
        }

        [Fact]
        public void Details_InvalidID_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 0;

            // act
            var result = _fuelCardController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_DbObject_null_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 2;
            FuelCard? fuelCard = null;

            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Returns(fuelCard);

            // act
            var result = _fuelCardController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Details_DbException_ShouldReturnNotFoundObjectResult()
        {
            // asign
            var id = 1;
            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Throws<Exception>();

            // act
            var result = _fuelCardController.Details(id);

            // assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region REMOVE

        [Fact]
        public void Remove_InvalidID_ShouldReturnBadRequest()
        {
            // asign
            var id = 0;

            // act
            var result = _fuelCardController.Remove(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void Remove_DbException_ShouldReturnBadRequest()
        {
            // asign
            var id = 1;
            _fuelCardRepoMock
                .Setup(x => x.Delete(id))
                .Throws<Exception>();

            // act
            var result = _fuelCardController.Remove(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Remove_ValidID_ShouldReturnNoContentResult()
        {
            // asign
            var id = 1;
            _fuelCardRepoMock
                .Setup(x => x.Delete(id))
                .Verifiable();

            // act
            var result = _fuelCardController.Remove(id);

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
            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _driverRepoMock
                .Setup(x => x.GetOnlyAvailableDrivers(null))
                .Returns(new List<Driver>());

            // act
            var result = _fuelCardController.Create();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Create_GET_ShouldReturnVehicleModel()
        {
            // asign
            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _driverRepoMock
                .Setup(x => x.GetOnlyAvailableDrivers(null))
                .Returns(new List<Driver>());

            // act
            var result = _fuelCardController.Create();

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<FuelCardModel>(okObjectResult.Value);
            Assert.NotNull(model.DriverSelectList);
            Assert.NotNull(model.FuelTypeSelectList);
        }

        [Fact]
        public void Create_GET_DbException_ShouldReturnBadRequestObjectResult()
        {
            // asign
            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Throws<Exception>();

            _driverRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Driver>());

            // act
            var result = _fuelCardController.Create();

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        #region Post
        [Fact]
        public void Create_Post_InvalidModelState_ShouldReturnBadRequest()
        {
            // asign
            var dateTime = new DateTime(2022, 4, 11, 19, 57, 59);
            var model = new FuelCardModel
            {
                CardNumber = "TN1234",
                ExpirationDate = dateTime,
                Pincode = "1234",
                DriverID = null,
                IsDisabled = true,
                FuelTypeID = new List<int>() { 1 },
            };

            // act
            _fuelCardController.ModelState
                .AddModelError("ModelState is not valid.", "Property is required.");

            var result = _fuelCardController.Create(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Create_Post_CardNumberAlreadyExists_ShouldReturnCreatedAtActionResult()
        {
            var dateTime = new DateTime(2022, 4, 11, 19, 57, 59);
            // asign
            var model = new FuelCardModel
            {
                CardNumber = "TN1234",
                ExpirationDate = dateTime,
                Pincode = "1234",
                DriverID = null,
                IsDisabled = true,
                FuelTypeID = new List<int>() { 1 },
            };

            _fuelCardRepoMock
               .Setup(x => x.DoesCardNumberExists(model.CardNumber))
               .Returns(true);

            // act
            var result = _fuelCardController.Create(model);

            // assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Create_Post_ValidObject_ShouldReturnCreatedAtAction()
        {
            var dateTime = new DateTime(2022, 4, 11, 19, 57, 59);
            // asign
            var model = new FuelCardModel
            {
                CardNumber = "TN1234",
                ExpirationDate = dateTime,
                Pincode = "1234",
                DriverID = null,
                IsDisabled = true,
                FuelTypeID = new List<int>() { 1 },
            };

            _fuelCardRepoMock
                .Setup(x => x.DoesCardNumberExists(model.CardNumber))
                .Returns(false);

            // act
            var result = _fuelCardController.Create(model);

            // assert
            Assert.IsType<CreatedAtActionResult>(result);
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
            var result = _fuelCardController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Get_DbObject_Null_ShouldReturnBadRequest()
        {
            // asign
            var id = 2;
            FuelCard? fuelCard = null;

            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Returns(fuelCard);

            // act
            var result = _fuelCardController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Get_DbException_ShouldReturnBadRequest()
        {
            // asign
            var id = 2;
            FuelCard? fuelCard = null;

            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Throws<Exception>();

            // act
            var result = _fuelCardController.Edit(id);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Get_ValidID_ShouldReturnOkObjectResult()
        {
            // asign
            var id = 1;

            _fuelCardRepoMock
                .Setup(x => x.Get(id))
                .Returns(_fuelCard);

            _fuelTypeRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<FuelType>());

            _driverRepoMock
                .Setup(x => x.GetOnlyAvailableDrivers(null))
                .Returns(new List<Driver>());

            // act
            var result = _fuelCardController.Edit(id);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<FuelCardModel>(okObjectResult.Value);
            Assert.NotNull(model.FuelTypeSelectList);
            Assert.NotNull(model.DriverSelectList);
            Assert.Null(model.DriverID);
            Assert.Equal(new List<int> { 1, 2 }, model.FuelTypeID);
            Assert.Equal(_fuelCard.CardNumber, model.CardNumber);
            Assert.Equal(_fuelCard.ExpirationDate, model.ExpirationDate);
            Assert.Equal(_fuelCard.Pincode, model.Pincode);
            Assert.Equal(_fuelCard.IsDisabled, model.IsDisabled);
        }

        #endregion

        #region POST

        [Fact]
        public void Edit_Put_InvalidModelState_ShouldReturnBadRequest()
        {
            // asign
            var model = new FuelCardModel
            {
                FuelCardID = 1,
                CardNumber = "KN1234",
                ExpirationDate = DateTime.MinValue,
                Pincode = "1234",
                IsDisabled = false,
                FuelTypeID = new List<int>() { 1 },
            };

            // act
            _fuelCardController.ModelState
                .AddModelError("ModelState is not valid.", "Property is required.");

            var result = _fuelCardController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Put_InvalidID_Model_ShouldReturnBadRequest()
        {
            // asign
            var model = new FuelCardModel
            {
                FuelCardID = 0,
                CardNumber = "KN1234",
                ExpirationDate = DateTime.MinValue,
                Pincode = "1234",
                IsDisabled = false,
                FuelTypeID = new List<int>() { 1 },
            };

            _fuelCardRepoMock
               .Setup(x => x.Get(model.FuelCardID))
               .Returns(_fuelCard);

            // act
            var result = _fuelCardController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Put_DbObject_Null_ShouldReturnBadRequest()
        {
            // asign
            var model = new FuelCardModel
            {
                FuelCardID = 1,
                CardNumber = "KN1234",
                ExpirationDate = DateTime.MinValue,
                Pincode = "1234",
                IsDisabled = false,
                FuelTypeID = new List<int>() { 1 },
            };

            FuelCard? fuelCard = null;

            _fuelCardRepoMock
                .Setup(x => x.Get(model.FuelCardID))
                .Returns(fuelCard);

            // act
            var result = _fuelCardController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Edit_Put_CardNumberExists_ShouldReturnBadRequest()
        {
            // asign
            var model = new FuelCardModel
            {
                FuelCardID = 0,
                CardNumber = "KN1234",
                ExpirationDate = DateTime.MinValue,
                Pincode = "1234",
                IsDisabled = false,
                FuelTypeID = new List<int>() { 1 },
            };

            _fuelCardRepoMock
                .Setup(x => x.Get(model.FuelCardID))
                .Returns(_fuelCard);

            _fuelCardRepoMock
                .Setup(x => x.DoesCardNumberExists(model.CardNumber))
                .Returns(true);

            // act
            var result = _fuelCardController.Edit(model);

            // assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void Edit_Put_AllConditions_OK_ShouldReturnCreatedAtAction()
        {
            // asign
            var model = new FuelCardModel
            {
                FuelCardID = 1,
                CardNumber = "KN1234",
                ExpirationDate = DateTime.MinValue,
                Pincode = "1234",
                IsDisabled = false,
                //FuelTypeSelectList = ""
            };

            _fuelCardRepoMock
                .Setup(x => x.Get(model.FuelCardID))
                .Returns(_fuelCard);

            _fuelCardRepoMock
                .Setup(x => x.DoesCardNumberExists(model.CardNumber))
                .Returns(false);

            // act
            var result = _fuelCardController.Edit(model);

            // assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Edit", createdAtActionResult.ActionName);
        }

        #endregion
        #endregion
    }
}