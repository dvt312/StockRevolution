// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using CSharpAssignment.DataModelEntities;
using Moq;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.DataModelEntities
{
    [TestFixture]
    public class StockEntitiesModelTest
    {
        private StockEntitesModelAccessClass dbContext { get; set; }

        [SetUp]
        public void SetUp()
        {
            dbContext = new StockEntitesModelAccessClass("TestConnectionName");
        }

        [TearDown]
        public void TearDown()
        {
            dbContext = null;
        }

        [Test]
        public void TestEmptyConstructor()
        {
            Assert.IsEmpty(dbContext.AppUser, "AppUser DBSet should be null.");
            Assert.IsEmpty(dbContext.AppUserStockLst, "AppUserStockLst DBSet should be null.");
            Assert.IsEmpty(dbContext.Person, "Person DBSet should be null.");
            Assert.IsEmpty(dbContext.StockLst, "StockLst DBSet should be null.");
            Assert.IsEmpty(dbContext.TokenValid, "TokenValid DBSet should be null.");
        }

        [Test]
        public void TestOnModelCreatingWillNullModel()
        {
            dbContext.OnModelCreatingCaller(null);
        }

        [Test]
        public void TestOnModelCreating()
        {
            var mockBuilder = new Mock<DbModelBuilder>();

            var typeConfigAppUser = new Mock<EntityTypeConfiguration<AppUser>>();
            var typeConfigPerson = new Mock<EntityTypeConfiguration<Person>>();
            var typeConfigStockLst = new Mock<EntityTypeConfiguration<StockLst>>();
            var typeConfigATokenValid = new Mock<EntityTypeConfiguration<TokenValid>>();

            mockBuilder.Setup(e => e.Entity<AppUser>()).Returns(typeConfigAppUser.Object).Verifiable();
            mockBuilder.Setup(e => e.Entity<Person>()).Returns(typeConfigPerson.Object).Verifiable();
            mockBuilder.Setup(e => e.Entity<StockLst>()).Returns(typeConfigStockLst.Object).Verifiable();
            mockBuilder.Setup(e => e.Entity<TokenValid>()).Returns(typeConfigATokenValid.Object).Verifiable();

            dbContext.OnModelCreatingCaller(mockBuilder.Object);

            mockBuilder.Verify(e => e.Entity<AppUser>());
            mockBuilder.Verify(e => e.Entity<Person>());
            mockBuilder.Verify(e => e.Entity<StockLst>());
            mockBuilder.Verify(e => e.Entity<TokenValid>());
        }
    }

    /// <summary>This class is being user to test protected methods in the base class.</summary>
    internal class StockEntitesModelAccessClass : StockEntitiesModel
    {
        public StockEntitesModelAccessClass(string name) : base(name)
        {
        }

        public void OnModelCreatingCaller(DbModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }
    }
}