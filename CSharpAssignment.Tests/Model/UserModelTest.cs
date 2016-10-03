// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Model
{
    [TestFixture]
    public class UserModelTest
    {
        private const int Id = 12354;
        private const string Name = "Luca";
        private const string LastName = "Lula";
        private const string Mail = "luca@stock.com";
        private const string Phone = "+571192222";
        private const string UserName = "llula";
        private const string Password = "1234";

        private UserModel _model { get; set; }

        [SetUp]
        public void SetUp()
        {
            _model = new UserModel();
        }

        [TearDown]
        public void TearDown()
        {
            _model = null;
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_model, "UserModel object should not be null.");
            Assert.AreEqual(_model.ItemId, -1, "Invalid default ItemId.");
            Assert.AreEqual(_model.Id, -1, "Id should be less than zero.");
            Assert.IsEmpty(_model.Name, "Name should be empty.");
            Assert.IsEmpty(_model.LastName, "LastName should be empty.");
            Assert.IsEmpty(_model.Mail, "Mail should be empty.");
            Assert.IsEmpty(_model.Phone, "Phone should be empty.");
            Assert.IsEmpty(_model.UserName, "UserName should be empty.");
            Assert.IsEmpty(_model.Password, "Password should be empty.");
        }

        [Test]
        public void TestConstructorParams()
        {
            _model = new UserModel(Id, Name, LastName, Mail, Phone, UserName, Password);
            Assert.IsNotNull(_model, "StockModel object should not be null.");
            Assert.AreEqual(_model.Id, Id, "Invalid Id.");
            Assert.AreEqual(_model.Name, Name, "Invalid Name.");
            Assert.AreEqual(_model.LastName, LastName, "Invalid LastName.");
            Assert.AreEqual(_model.Mail, Mail, "Invalid Mail.");
            Assert.AreEqual(_model.Phone, Phone, "Invalid Phone.");
            Assert.AreEqual(_model.UserName, UserName, "Invalid UserName.");
            Assert.AreEqual(_model.Password, Password, "Invalid Password.");
        }
    }
}
