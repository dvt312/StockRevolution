// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using Moq;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.DataAccess
{
    [TestFixture]
    public class StockDaoTest
    {
        private const int StockCodeId = 1;
        private const string StockCode = "ST001";
        private const string StockName = "EX001";
        private StockDao DaoObj { get; set; }
        private Mock<StockEntitiesModel> DbContextMock { get; set; }
        private Mock<DbSet<StockLst>> StockLstSetMock { get; set; }
        private List<StockLst> StockDataList { get; set; }

        [SetUp]
        public void SetUp()
        {
            DbContextMock = new Mock<StockEntitiesModel>();
            DaoObj = new StockDao(DbContextMock.Object);
            StockLstSetMock = new Mock<DbSet<StockLst>>();
            StockDataList = new List<StockLst>
            {
                new StockLst {StockCode = StockCode, StockName = StockName, StockLstId = StockCodeId}
            };

            var queryableList = StockDataList.AsQueryable();
            StockLstSetMock.As<IQueryable<StockLst>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            StockLstSetMock.As<IQueryable<StockLst>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            StockLstSetMock.As<IQueryable<StockLst>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            StockLstSetMock.As<IQueryable<StockLst>>().Setup(
                m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            DbContextMock.Setup(e => e.StockLst).Returns(StockLstSetMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            DaoObj = null;
            DbContextMock = null;
            StockDataList = null;
            StockLstSetMock = null;
        }

        [Test]
        public void TestEmptyConstructor()
        {
            DaoObj = new StockDao();
            Assert.IsNotNull(DaoObj.dbContext, "DBContext should not be null.");
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(DaoObj.dbContext, "DBContext should not be null.");
        }

        [Test]
        public void TestDeleteItemWithExistingItem()
        {
            var stockObj = new StockLst {StockCode = StockCode, StockName = StockName, StockLstId = StockCodeId};
            StockLstSetMock.Setup(e => e.Remove(It.IsAny<StockLst>())).Returns(stockObj);
            var wasDeleted = DaoObj.DeleteItem(StockCodeId);
            Assert.IsTrue(wasDeleted, "StockLst should not be deleted in list");
        }

        [Test]
        public void TestDeleteItemWithNonExistingItem()
        {
            var stockObj = new StockLst { StockCode = "0014", StockName = "RS014", StockLstId = 63 };
            StockLstSetMock.Setup(e => e.Remove(It.IsAny<StockLst>())).Returns(stockObj);
            var wasDeleted = DaoObj.DeleteItem(63);
            Assert.IsFalse(wasDeleted, "StockLst should be deleted in list");
        }

        [Test]
        public void TestFindAll()
        {
            var allElements = DaoObj.FindAll();
            Assert.IsNotNull(allElements, "Elements list should not be null.");
            Assert.AreEqual(allElements.Count, 1, "List of all elements should contain just one item.");
            var item = allElements[0];
            Assert.AreEqual(StockCode, item.StockCode, "Invalid StockCode value.");
            Assert.AreEqual(StockName, item.StockName, "Invalid StockName value.");
        }

        [Test]
        public void TestFindBySTockCodeWithCodeInList()
        {
            var element = DaoObj.FindByStockCode(StockCode);
            Assert.IsNotNull(element, "StockLst object should not be null.");
            Assert.AreEqual(StockCode, element.StockCode, "Invalid StockCode value.");
            Assert.AreEqual(StockName, element.StockName, "Invalid StockName value.");
        }

        [Test]
        public void TestFindBySTockCodeWithCodeNotInList()
        {
            var element = DaoObj.FindByStockCode("XX11");
            Assert.IsNull(element, "StockLst object should be null.");
        }

        [Test]
        public void TestFindByIdWithIdInList()
        {
            var element = DaoObj.FindById(StockCodeId);
            Assert.IsNotNull(element, "StockLst object should not be null.");
            Assert.AreEqual(StockCode, element.StockCode, "Invalid StockCode value.");
            Assert.AreEqual(StockName, element.StockName, "Invalid StockName value.");
        }

        [Test]
        public void TestFindByIdWithIdNotInList()
        {
            var element = DaoObj.FindById(30);
            Assert.IsNull(element, "StockLst object should be null.");
        }

        [Test]
        public void TestInsertItemWithExistingItem()
        {
            var stockObj = new StockLst { StockCode = StockCode, StockName = StockName, StockLstId = StockCodeId };
            StockLstSetMock.Setup(e => e.Add(It.IsAny<StockLst>())).Returns(stockObj);
            var wasInserted = DaoObj.InsertItem(stockObj);
            Assert.IsFalse(wasInserted, "StockLst should not be inserted in list");
        }

        [Test]
        public void TestInsertItemWithNonExistingItem()
        {
            var stockObj = new StockLst { StockCode = "0014", StockName = "RS014", StockLstId = 63 };
            StockLstSetMock.Setup(e => e.Add(It.IsAny<StockLst>())).Returns(stockObj);
            var wasInserted = DaoObj.InsertItem(stockObj);
            Assert.IsTrue(wasInserted, "StockLst should be inserted in list");
        }
    }
}