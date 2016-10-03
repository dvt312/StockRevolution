// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.ExceptionUtil;
using CSharpAssignment.Factory;
using CSharpAssignment.Service;
using CSharpAssignment.Util;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace CSharpAssignment.Tests.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LoginServiceTest
    {
        private const int AppUserId = 13;
        private const int TokenValidId = 17;
        private const string UserName = "UserName";
        private const string Password = "Password";
        private const string CypherPass = "P@55w0rd";
        private const string GeneratedToken = "P@#$%&/(##T0K3N";
        private const string TokenId = "P@#$%&";
        private LoginService _service = null;
        private AppUser _dummyUser = null;
        private TokenValid _dummyToken = null;
        private Mock<PersonDao> _mockPersonDao = null;
        private Mock<TokenDao> _mockTokenDao = null;
        private Mock<UserDao> _mockUserDao = null;
        private Mock<StockUserModelFactory> _mockFactory = null;
        private Mock<TokenUtilAccessor> _mockTokenUtil = null;
        private Mock<CryptoUtilAccessor> _mockCryptoUtil = null;

        [SetUp]
        public void SetUp()
        {
            _mockPersonDao = new Mock<PersonDao>();
            _mockTokenDao = new Mock<TokenDao>();
            _mockUserDao = new Mock<UserDao>();
            _mockFactory = new Mock<StockUserModelFactory>();
            _dummyUser = new AppUser { UserName = UserName, Password = Password, AppUserId = AppUserId };
            _dummyToken = new TokenValid { TokenId = TokenId, IsValid = 1, TokenValidId = TokenValidId};
            _service =
                new LoginService(
                    _mockPersonDao.Object, _mockTokenDao.Object, _mockUserDao.Object, _mockFactory.Object);
            _mockTokenUtil = new Mock<TokenUtilAccessor>();
            _mockCryptoUtil = new Mock<CryptoUtilAccessor>();
            _mockCryptoUtil.Setup(e => e.Encrypt(Password)).Returns(CypherPass);
            var outValue = string.Empty;

            _mockTokenUtil.Setup(
                e => e.GenerateToken(UserName, CypherPass, AppUserId, out outValue)).Returns(GeneratedToken);

            _mockUserDao.Setup(e => e.InserTokenValid(AppUserId, outValue, 1)).Verifiable();

            _mockTokenDao.Setup(e => e.FindByAppUserId(AppUserId)).Verifiable();
            
            CryptoUtilAccessor.SetTestingInstance(_mockCryptoUtil.Object);
            TokenUtilAccessor.SetTestingInstance(_mockTokenUtil.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _dummyUser = null;
            _dummyToken = null;
            _mockPersonDao = null;
            _mockTokenDao = null;
            _mockUserDao = null;
            _mockFactory = null;
            CryptoUtilAccessor.SetTestingInstance(null);
            TokenUtilAccessor.SetTestingInstance(null);
        }

        [Test]
        public void TestEmptyConstructor()
        {
            _service = new LoginService();
            Assert.IsNotNull(_service.Messages, "Messages List should not be null.");
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_service.Messages, "Messages List should not be null.");
        }

        [Test]
        public void TestGrantAccessWithValidUsernameAndPass()
        {
            _mockUserDao.Setup(e => e.FindByUserNameAndPassword(UserName, CypherPass)).Returns(_dummyUser);
            var model = _service.GrantAccess(UserName, Password);
            _mockTokenDao.Verify(e => e.FindByAppUserId(AppUserId));
            _mockUserDao.Verify(e => e.InserTokenValid(AppUserId, It.IsAny<string>(), 1));

            Assert.IsNotNull(model, "SessionModel should not be null.");
            Assert.AreEqual(GeneratedToken, model.Token, "Invalid generated token.");
        }

        [Test]
        public void TestGrantAccessWithInvalidUsernameAndPass()
        {
            AppUser user = null;
            _mockUserDao.Setup(e => e.FindByUserNameAndPassword(UserName, CypherPass)).Returns(user);
            var model = _service.GrantAccess(UserName, Password);

            Assert.IsNotNull(model, "SessionModel should not be null.");
            Assert.IsEmpty(model.Token, "Invalid generated token.");
            Assert.IsNotEmpty(_service.Messages, "Messages list should contain one message.");
        }

        [Test]
        public void TestGrantAccessWithValidUsernameAndPassAndExistingToken()
        {
            _mockUserDao.Setup(e => e.FindByUserNameAndPassword(UserName, CypherPass)).Returns(_dummyUser);
            _mockTokenDao.Setup(e => e.FindByAppUserId(AppUserId)).Returns(_dummyToken);
            _mockTokenDao.Setup(e => e.UpdateItem(TokenValidId, TokenId, 1)).Verifiable();
            var model = _service.GrantAccess(UserName, Password);
            _mockTokenDao.Verify(e => e.FindByAppUserId(AppUserId));
            _mockTokenDao.Verify(e => e.UpdateItem(TokenValidId, It.IsAny<string>(), 1));

            Assert.IsNotNull(model, "SessionModel should not be null.");
            Assert.AreEqual(GeneratedToken, model.Token, "Invalid generated token.");
        }

        [Test]
        public void TestGrantAccessWithArgumentNullException()
        {
            _mockUserDao.Setup(
                e => e.FindByUserNameAndPassword(UserName, CypherPass)).Throws < ArgumentNullException>();
            Assert.That(() => _service.GrantAccess(UserName, Password), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGrantAccessWithArgumentOutOfRangeException()
        {
            _mockUserDao.Setup(
                e => e.FindByUserNameAndPassword(UserName, CypherPass)).Throws<ArgumentOutOfRangeException>();
            Assert.That(() => _service.GrantAccess(UserName, Password), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGrantAccessWithArgumentException()
        {
            _mockUserDao.Setup(
                e => e.FindByUserNameAndPassword(UserName, CypherPass)).Throws<ArgumentException>();
            Assert.That(() => _service.GrantAccess(UserName, Password), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGrantAccessWithFormatException()
        {
            _mockUserDao.Setup(
                e => e.FindByUserNameAndPassword(UserName, CypherPass)).Throws<FormatException>();
            Assert.That(() => _service.GrantAccess(UserName, Password), Throws.TypeOf<ProcedureException>());
        }

        [Test]
        public void TestGrantAccessWithException()
        {
            _mockUserDao.Setup(
                e => e.FindByUserNameAndPassword(UserName, CypherPass)).Throws<Exception>();
            Assert.That(() => _service.GrantAccess(UserName, Password), Throws.TypeOf<ProcedureException>());
        }
    }

    /// <summaryAccessor class used just in tests to access protected methods.</summary>
    internal class CryptoUtilAccessor : CryptoUtil
    {
        public CryptoUtilAccessor () : base() { }

        public static void SetTestingInstance(CryptoUtilAccessor instance)
        {
            CryptoUtil.setTestingInstance(instance);
        }
    }

    /// <summaryAccessor class used just in tests to access protected methods.</summary>
    internal class TokenUtilAccessor : TokenUtil
    {
        public TokenUtilAccessor() : base() { }

        public static void SetTestingInstance(TokenUtil instance)
        {
            TokenUtil.setTestingInstance(instance);
        }
    }
}
