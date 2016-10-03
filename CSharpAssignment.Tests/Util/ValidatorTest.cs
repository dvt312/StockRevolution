// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.Factory;
using CSharpAssignment.Mapper;
using CSharpAssignment.Model;
using Moq;
using NUnit.Framework;
using CSharpAssignment.Util;

#endregion

namespace CSharpAssignment.Tests.Util
{
    [TestFixture]
    public class ValidatorTest
    {
        private const int Id = 12354;
        private const string Name = "Luca";
        private const string LastName = "Lula";
        private const string Mail = "luca@stock.com";
        private const string Phone = "+571192222";
        private const string UserName = "llula";
        private const string Password = "1234";
        private const string StockCode = "XX88";
        private const string StockName = "X88RS";

        private UserModel _userModel;

        private StockModel _stockModel;

        /// <summary>The Validator object under test.</summary>
        private Validator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = Validator.Instance;
            _userModel =
                new UserModel
                { 
                    Id = Id,
                    Name = Name,
                    LastName = LastName,
                    Mail = Mail,
                    Phone = Phone,
                    UserName = UserName,
                    Password = Password
                };
            _stockModel = new StockModel { StockCode = StockCode, StockName = StockName };
        }

        [TearDown]
        public void TearDown()
        {
            _validator = null;
            _userModel = null;
            _stockModel = null;
        }

        [Test]
        public void TestValidateUser()
        {
            Assert.IsTrue(_validator.ValidateUser(_userModel), "UserModel should be valid.");
        }

        [Test]
        public void TestValidateUserInvalidID()
        {
            _userModel.Id = -1;
            Assert.IsFalse(_validator.ValidateUser(_userModel), "UserModel should be valid.");
        }

        [Test]
        public void TestValidateUserInvalidName()
        {
            _userModel.Name = string.Empty;
            Assert.IsFalse(_validator.ValidateUser(_userModel), "UserModel should be valid.");
        }

        [Test]
        public void TestValidateUserInvalidLastName()
        {
            _userModel.LastName = string.Empty;
            Assert.IsFalse(_validator.ValidateUser(_userModel), "UserModel should be valid.");
        }

        [Test]
        public void TestValidateUserInvalidUserName()
        {
            _userModel.UserName = string.Empty;
            Assert.IsFalse(_validator.ValidateUser(_userModel), "UserModel should be valid.");
        }

        [Test]
        public void TestValidateUserInvalidPassword()
        {
            _userModel.UserName = string.Empty;
            Assert.IsFalse(_validator.ValidateUser(_userModel), "UserModel should be valid.");
        }

        [Test]
        public void TestValidateStock()
        {
            Assert.IsTrue(_validator.ValidateStock(_stockModel), "StockModel should be valid.");
        }

        [Test]
        public void TestValidateStockInvalidStockCode()
        {
            _stockModel.StockCode = string.Empty;
            Assert.IsFalse(_validator.ValidateStock(_stockModel), "StockModel should be valid.");
        }

        [Test]
        public void TestValidateStockInvalidStockName()
        {
            _stockModel.StockName = string.Empty;
            Assert.IsFalse(_validator.ValidateStock(_stockModel), "StockModel should be valid.");
        }
    }
}