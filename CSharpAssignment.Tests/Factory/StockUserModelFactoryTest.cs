// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Diagnostics.CodeAnalysis;
using CSharpAssignment.Factory;
using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Factory
{
    [TestFixture]
    public class StockUserModelFactoryTest
    {
        private const string StockCode = "0011";
        private const string StockName = "EX11";
        private const int Id = 15;
        private const string Name = "NameTest";
        private const string LastName = "LastNameTest|";
        private const string Mail = "Mail@Test.test";
        private const string Phone = "+5844788";
        private const string UserName = "TestUser";
        private const string Password = "TestPass";

        /// <summary>The StockUserModelFactory object under test.</summary>
        private StockUserModelFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new StockUserModelFactory();
        }

        [TearDown]
        public void TearDown()
        {
            _factory = null;
        }

        [Test]
        [ExcludeFromCodeCoverage]
        public void TestGetModel()
        {
            _factory = new StockUserModelFactory(StockCode, StockName);
            var model = _factory.GetModel();

            Assert.IsNotNull(model, "Model should not be null.");
            Assert.IsInstanceOf(typeof(StockModel), model, "Returned model should be a StockModel.");

            var stock = model as StockModel;
            Assert.IsNotNull(stock, "Stock Model should not be null.");
            Assert.AreEqual(StockCode, stock.StockCode, "Invalid StockCode value.");
            Assert.AreEqual(StockName, stock.StockName, "Invalid StockName value.");
            Assert.AreNotEqual(-1.0, stock.Price, "Invalid Price value.");
            Assert.IsTrue(stock.Price > 0 && stock.Price < 1000, "Invalid Price limit.");
        }

        [Test]
        [ExcludeFromCodeCoverage]
        public void TestGetModelWithStockParams()
        {
            _factory = new StockUserModelFactory();
            var model = _factory.GetModel(StockCode, StockName);

            Assert.IsNotNull(model, "Model should not be null.");
            Assert.IsInstanceOf(typeof(StockModel), model, "Returned model should be a StockModel.");

            var stock = model as StockModel;
            Assert.IsNotNull(stock, "Stock Model should not be null.");
            Assert.AreEqual(StockCode, stock.StockCode, "Invalid StockCode value.");
            Assert.AreEqual(StockName, stock.StockName, "Invalid StockName value.");
            Assert.AreNotEqual(-1.0, stock.Price, "Invalid Price value.");
            Assert.IsTrue(stock.Price > 0 && stock.Price < 1000, "Invalid Price limit.");
        }

        [Test]
        public void TestGetModelWithUserParams()
        {
            _factory = new StockUserModelFactory();
            var model = _factory.GetModel(Id, Name, LastName, Mail, Phone, UserName, Password);

            Assert.IsNotNull(model, "Model should not be null.");
            Assert.IsInstanceOf(typeof(UserModel), model, "Returned model should be a UserModel.");

            var stock = model as UserModel;
            Assert.IsNotNull(stock, "User Model should not be null.");
            Assert.AreEqual(Id, stock.Id, "Invalid Id value.");
            Assert.AreEqual(Name, stock.Name, "Invalid Name value.");
            Assert.AreEqual(LastName, stock.LastName, "Invalid LastName value.");
            Assert.AreEqual(Mail, stock.Mail, "Invalid Mail value.");
            Assert.AreEqual(Phone, stock.Phone, "Invalid Phone value.");
            Assert.AreEqual(UserName, stock.UserName, "Invalid UserName value.");
            Assert.AreEqual(Password, stock.Password, "Invalid Password value.");
        }
    }
}