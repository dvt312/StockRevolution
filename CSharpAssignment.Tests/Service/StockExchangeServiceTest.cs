// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.ExceptionUtil;
using CSharpAssignment.Factory;
using CSharpAssignment.Mapper;
using CSharpAssignment.Model;
using CSharpAssignment.Service;
using CSharpAssignment.Util;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace CSharpAssignment.Tests.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StockExchangeServiceTest
    {
        private const int AppUserId = 13;
        private const string StockCode = "StockCode";
        private const string StockName = "StockName";
        private StockExchangeService _service = null;
        private StockLst _dummyStock = null;
        private List<StockLst> _dummyStockList = null;
        private StockModel _dummyStockModel = null;
        private List<BaseModel> _dummyStockModelList = null;
        private Mock<StockDao> _mockStockDao = null;
        private Mock<UserDao> _mockUserDao = null;
        private Mock<UserStockDao> _mockUserStockDao = null;
        private Mock<StockDataMapper> _mockMapper = null;

        [SetUp]
        public void SetUp()
        {
            _mockStockDao = new Mock<StockDao>();
            _mockUserDao = new Mock<UserDao>();
            _mockUserStockDao = new Mock<UserStockDao>();
            _mockMapper = new Mock<StockDataMapper>();
            _dummyStock = new StockLst { StockCode = StockCode, StockName = StockName};
            _dummyStockList = new List<StockLst> { _dummyStock };
            _dummyStockModel = new StockModel { StockCode = StockCode, StockName = StockName};
            _dummyStockModelList = new List<BaseModel> { _dummyStockModel };
            _service =
                new StockExchangeService(
                    _mockStockDao.Object, _mockUserDao.Object, _mockUserStockDao.Object, _mockMapper.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _dummyStock = null;
            _dummyStockList = null;
            _dummyStockModelList = null;
            _mockStockDao = null;
            _mockUserDao = null;
            _mockUserStockDao = null;
            _mockMapper = null;
            CryptoUtilAccessor.SetTestingInstance(null);
            TokenUtilAccessor.SetTestingInstance(null);
        }

        [Test]
        public void TestEmptyConstructor()
        {
            _service = new StockExchangeService();
            Assert.IsNotNull(_service.Messages, "Messages List should not be null.");
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_service.Messages, "Messages List should not be null.");
        }

        [Test]
        public void TestGetUserStock()
        {
            _mockUserStockDao.Setup(e => e.FindUserStock(AppUserId)).Returns(_dummyStockList);
            _mockMapper.Setup(e => e.MapStockLstToStockModelLst(_dummyStockList)).Returns(_dummyStockModelList);

            var list = _service.GetUserStock(AppUserId);

            Assert.IsNotNull(list, "Returned list should not be null.");
            Assert.AreEqual(1, list.Count, "Returned list size should be 1.");

            var item = list[0];
            Assert.IsInstanceOf<StockModel>(item, "Item list should be a StockModel.");

            var stock = item as StockModel;
            Assert.IsNotNull(stock, "stock should not be null.");
            Assert.AreEqual(StockCode, stock.StockCode, "Invalid Stock code.");
            Assert.AreEqual(StockName, stock.StockName, "Invalid Stock name.");
        }

        [Test]
        public void TestGetUserStockWithNullDao()
        {
            _service =
                new StockExchangeService(
                    _mockStockDao.Object, _mockUserDao.Object, null, _mockMapper.Object);

            var list = _service.GetUserStock(AppUserId);

            Assert.IsNull(list, "Returned list should be null.");
        }

        [Test]
        public void TestGetUserStockWithArgumentNullException()
        {
            _mockUserStockDao.Setup(e => e.FindUserStock(AppUserId)).Throws<ArgumentNullException>();            
            Assert.That(() => _service.GetUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetUserStockWithArgumentOutOfRangeException()
        {
            _mockUserStockDao.Setup(e => e.FindUserStock(AppUserId)).Throws<ArgumentOutOfRangeException>();
            Assert.That(() => _service.GetUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetUserStockWithArgumentException()
        {
            _mockUserStockDao.Setup(e => e.FindUserStock(AppUserId)).Throws<ArgumentException>();
            Assert.That(() => _service.GetUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetUserStockWithFormatException()
        {
            _mockUserStockDao.Setup(e => e.FindUserStock(AppUserId)).Throws<FormatException>();
            Assert.That(() => _service.GetUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetUserStockWithException()
        {
            _mockUserStockDao.Setup(e => e.FindUserStock(AppUserId)).Throws<Exception>();
            Assert.That(() => _service.GetUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetAllExceptUserStock()
        {
            _mockUserStockDao.Setup(e => e.FindAllExceptUserStock(AppUserId)).Returns(_dummyStockList);
            _mockMapper.Setup(e => e.MapStockLstToStockModelLst(_dummyStockList)).Returns(_dummyStockModelList);

            var list = _service.GetAllExceptUserStock(AppUserId);

            Assert.IsNotNull(list, "Returned list should not be null.");
            Assert.AreEqual(1, list.Count, "Returned list size should be 1.");

            var item = list[0];
            Assert.IsInstanceOf<StockModel>(item, "Item list should be a StockModel.");

            var stock = item as StockModel;
            Assert.IsNotNull(stock, "stock should not be null.");
            Assert.AreEqual(StockCode, stock.StockCode, "Invalid Stock code.");
            Assert.AreEqual(StockName, stock.StockName, "Invalid Stock name.");
        }

        [Test]
        public void TestGetAllExceptUserStockWithNullDao()
        {
            _service =
                new StockExchangeService(
                    _mockStockDao.Object, _mockUserDao.Object, null, _mockMapper.Object);

            var list = _service.GetAllExceptUserStock(AppUserId);

            Assert.IsNull(list, "Returned list should be null.");
        }

        [Test]
        public void TestGetAllExceptUserStockWithArgumentNullException()
        {
            _mockUserStockDao.Setup(e => e.FindAllExceptUserStock(AppUserId)).Throws<ArgumentNullException>();
            Assert.That(() => _service.GetAllExceptUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetAllExceptUserStockWithArgumentOutOfRangeException()
        {
            _mockUserStockDao.Setup(e => e.FindAllExceptUserStock(AppUserId)).Throws<ArgumentOutOfRangeException>();
            Assert.That(() => _service.GetAllExceptUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetAllExceptUserStockWithArgumentException()
        {
            _mockUserStockDao.Setup(e => e.FindAllExceptUserStock(AppUserId)).Throws<ArgumentException>();
            Assert.That(() => _service.GetAllExceptUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetAllExceptUserStockWithFormatException()
        {
            _mockUserStockDao.Setup(e => e.FindAllExceptUserStock(AppUserId)).Throws<FormatException>();
            Assert.That(() => _service.GetAllExceptUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGetAllExceptUserStockWithException()
        {
            _mockUserStockDao.Setup(e => e.FindAllExceptUserStock(AppUserId)).Throws<Exception>();
            Assert.That(() => _service.GetAllExceptUserStock(AppUserId), Throws.TypeOf<ProcedureException>());
        }
    }
}
