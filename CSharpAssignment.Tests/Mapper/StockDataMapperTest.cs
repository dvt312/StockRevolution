// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.Factory;
using CSharpAssignment.Mapper;
using CSharpAssignment.Model;
using Moq;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Mapper
{
    [TestFixture]
    public class StockDataMapperTest
    {

        private const string StockCode = "RX550";
        private const string StockName = "LS550";
        private const int AppUserId = 11;

        private StockLst _stockDummy;

        private StockModel _stockModelDummy;

        private List<StockLst> _listStocks;

        /// <summary>The StockDataMapper object under test.</summary>
        private StockDataMapper _mapper;

        /// <summary>The StockUserModelFactory Mock.</summary>
        private Mock<StockUserModelFactory> _modelFactory;

        [SetUp]
        public void SetUp()
        {
            _modelFactory = new Mock<StockUserModelFactory>();
            _stockDummy = new StockLst {StockCode = StockCode, StockName = StockName};
            _stockModelDummy = new StockModel {StockCode = StockCode, StockName = StockName};
            _listStocks = new List<StockLst> {_stockDummy};
            _mapper = new StockDataMapper(_modelFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mapper = null;
            _modelFactory = null;
            _stockDummy = null;
            _listStocks = null;
        }

        [Test]
        public void TestEmptyConstructor()
        {
            _mapper = new StockDataMapper();
        }

        [Test]
        public void TestMapStockLstToStockModelLstt()
        {
            _modelFactory.Setup(e => e.GetModel(StockCode, StockName)).Returns(_stockModelDummy);
            var list = _mapper.MapStockLstToStockModelLst(_listStocks);
            Assert.IsNotNull(list, "List should not be null.");
            Assert.IsNotEmpty(list, "List should not be empty.");
            Assert.AreEqual(1, list.Count, "List should contain just one item.");

            var item = list[0];
            Assert.IsInstanceOf<StockModel>(item, "Item should be a StockModel instance.");

            var stockModel = item as StockModel;
            Assert.IsNotNull(stockModel, "StockModel should not be null.");
            Assert.AreEqual(StockCode, stockModel.StockCode, "Invalid StockCode value.");
        }

        [Test]
        public void TestMapStockLstToStockModelLstWithEmptyList()
        {
            var list = _mapper.MapStockLstToStockModelLst(new List<StockLst>());
            Assert.IsNotNull(list, "List should not be null.");
            Assert.IsEmpty(list, "List should be empty.");
        }

        [Test]
        public void TestMapStockLstToStockModelLstWithNullList()
        {
            var list = _mapper.MapStockLstToStockModelLst(null);
            Assert.IsNull(list, "List should be null.");
        }
    }
}