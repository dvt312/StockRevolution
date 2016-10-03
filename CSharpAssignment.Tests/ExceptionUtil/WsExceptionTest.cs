// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Runtime.Serialization;
using System.Web.Services.Protocols;
using CSharpAssignment.ExceptionUtil;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.ExceptionUtil
{
    [TestFixture]
    public class WsExceptionTest
    {
        /// <summary>The dummy ws exception message.</summary>
        private const string WsExMessage = "wsMessage";

        /// <summary>The WsException object under test.</summary>
        private WsException _wsException;

        /// <summary>The inner exception.</summary>
        private Exception _innerException;

        [SetUp]
        public void SetUp()
        {
            _innerException = new Exception(WsExMessage);
            _wsException = new WsException(WsExMessage, SoapException.ClientFaultCode, _innerException);
        }

        [TearDown]
        public void TearDown()
        {
            _wsException = null;
            _innerException = null;
        }

        [Test]
        public void TestDefaultConstructor()
        {
            _wsException = new WsException();
            Assert.Null(
                _wsException.DisplayMessage,
                "Invalid ws exception display message.");
        }

        [Test]
        public void TestDefaultConstructorWithMsg()
        {
            Assert.AreEqual(
                string.Format(WsException.ExceptionMessage, WsExMessage),
                _wsException.DisplayMessage,
                "Invalid ws exception display message.");
        }

        [Test]
        public void TestGetObjectData()
        {
            var ex = new WsException();
            var converter = new FormatterConverter();
            var info = new SerializationInfo(typeof(WsException), converter);
            var context = new StreamingContext();

            ex.GetObjectData(info, context);
        }
    }
}