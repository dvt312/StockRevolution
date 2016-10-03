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
    public class TokenDaoTest
    {
        private const int TokenValidId = 1;
        private const int AppUserId = 2;
        private const string TokenId = "ST001";
        private const int IsValid = 1;
        private TokenDao DaoObj { get; set; }
        private Mock<StockEntitiesModel> DbContextMock { get; set; }
        private Mock<DbSet<TokenValid>> TokenValidSetMock { get; set; }
        private List<TokenValid> StockDataList { get; set; }

        [SetUp]
        public void SetUp()
        {
            DbContextMock = new Mock<StockEntitiesModel>();
            DaoObj = new TokenDao(DbContextMock.Object);
            TokenValidSetMock = new Mock<DbSet<TokenValid>>();
            StockDataList = new List<TokenValid>
            {
                new TokenValid
                {
                    TokenId = TokenId, IsValid = IsValid, TokenValidId = TokenValidId, AppUserId = AppUserId
                }
            };

            var queryableList = StockDataList.AsQueryable();
            TokenValidSetMock.As<IQueryable<TokenValid>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            TokenValidSetMock.As<IQueryable<TokenValid>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            TokenValidSetMock.As<IQueryable<TokenValid>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            TokenValidSetMock.As<IQueryable<TokenValid>>().Setup(
                m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            DbContextMock.Setup(e => e.TokenValid).Returns(TokenValidSetMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            DaoObj = null;
            DbContextMock = null;
            StockDataList = null;
            TokenValidSetMock = null;
        }

        [Test]
        public void TestEmptyConstructor()
        {
            DaoObj = new TokenDao();
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
            var tokenObj = new TokenValid
            {
                TokenId = TokenId,
                IsValid = IsValid,
                TokenValidId = TokenValidId,
                AppUserId = AppUserId
            };
            TokenValidSetMock.Setup(e => e.Remove(It.IsAny<TokenValid>())).Returns(tokenObj);
            var wasDeleted = DaoObj.DeleteItem(TokenValidId);
            Assert.IsTrue(wasDeleted, "TokenValid should not be deleted in list");
        }

        [Test]
        public void TestDeleteItemWithNonExistingItem()
        {
            var tokenObj = new TokenValid { TokenId = "XX77", IsValid = 0, TokenValidId = 77 };
            TokenValidSetMock.Setup(e => e.Remove(It.IsAny<TokenValid>())).Returns(tokenObj);
            var wasDeleted = DaoObj.DeleteItem(63);
            Assert.IsFalse(wasDeleted, "TokenValid should be deleted in list");
        }

        [Test]
        public void TestFindAll()
        {
            var allElements = DaoObj.FindAll();
            Assert.IsNotNull(allElements, "Elements list should not be null.");
            Assert.AreEqual(allElements.Count, 1, "List of all elements should contain just one item.");
            var item = allElements[0];
            Assert.AreEqual(IsValid, item.IsValid, "Invalid IsValid value.");
            Assert.AreEqual(TokenId, item.TokenId, "Invalid TokenId value.");
        }

        [Test]
        public void TestFindByAppUserIdWithIdInList()
        {
            var element = DaoObj.FindByAppUserId(AppUserId);
            Assert.IsNotNull(element, "TokenValid object should not be null.");
            Assert.AreEqual(IsValid, element.IsValid, "Invalid IsValid value.");
            Assert.AreEqual(TokenId, element.TokenId, "Invalid TokenId value.");
        }

        [Test]
        public void TestFindByAppUserIdWithIdNotInList()
        {
            var element = DaoObj.FindByAppUserId(14);
            Assert.IsNull(element, "TokenValid object should be null.");
        }

        [Test]
        public void TestFindByTokenIdWithIdInList()
        {
            var element = DaoObj.FindByTokenId(TokenId);
            Assert.IsNotNull(element, "TokenValid object should not be null.");
            Assert.AreEqual(IsValid, element.IsValid, "Invalid IsValid value.");
            Assert.AreEqual(TokenId, element.TokenId, "Invalid TokenId value.");
        }

        [Test]
        public void TestFindByTokenIdWithIdNotInList()
        {
            var element = DaoObj.FindByTokenId("RJ45");
            Assert.IsNull(element, "TokenValid object should be null.");
        }

        [Test]
        public void TestFindByIdWithIdInList()
        {
            var element = DaoObj.FindById(TokenValidId);
            Assert.IsNotNull(element, "TokenValid object should not be null.");
            Assert.AreEqual(IsValid, element.IsValid, "Invalid IsValid value.");
            Assert.AreEqual(TokenId, element.TokenId, "Invalid TokenId value.");
        }

        [Test]
        public void TestFindByIdWithIdNotInList()
        {
            var element = DaoObj.FindById(30);
            Assert.IsNull(element, "TokenValid object should be null.");
        }

        [Test]
        public void TestInsertItemWithExistingItem()
        {
            var tokenObj = new TokenValid
            {
                TokenId = TokenId,
                IsValid = IsValid,
                TokenValidId = TokenValidId,
                AppUserId = AppUserId
            };
            TokenValidSetMock.Setup(e => e.Add(It.IsAny<TokenValid>())).Returns(tokenObj);
            var wasInserted = DaoObj.InsertItem(tokenObj);
            Assert.IsFalse(wasInserted, "TokenValid should not be inserted in list");
        }

        [Test]
        public void TestInsertItemWithNonExistingItem()
        {
            var tokenObj = new TokenValid { TokenId = "XX77", IsValid = 0, TokenValidId = 77 };
            TokenValidSetMock.Setup(e => e.Add(It.IsAny<TokenValid>())).Returns(tokenObj);
            var wasInserted = DaoObj.InsertItem(tokenObj);
            Assert.IsTrue(wasInserted, "TokenValid should be inserted in list");
        }

        [Test]
        public void TestUpdateWithExistingItem()
        {
            var tokenObj = new TokenValid
            {
                TokenId = "XR",
                IsValid = 0,
                TokenValidId = TokenValidId,
                AppUserId = AppUserId
            };
            DbContextMock.Setup(e => e.SetModified(tokenObj)).Verifiable();
            DbContextMock.Setup(e => e.SaveChanges()).Returns(1);
            var wasUpdated = DaoObj.UpdateItem(TokenValidId, TokenId, IsValid);

            DbContextMock.Verify(e => e.SetModified(It.IsAny<TokenValid>()));

            Assert.IsTrue(wasUpdated, "TokenValid should be updated in list");
        }
    }
}