// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Runtime.Serialization;
using CSharpAssignment.ExceptionUtil;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.ExceptionUtil
{
    [TestFixture]
    public class ProcedureExceptionUtilTest
    {
        /// <summary>The dummy procedure exception message.</summary>
        private const string ProcedureExMessage = "procedureExMessage";

        /// <summary>The ProcedureException object under test.</summary>
        private ProcedureException _procedureException;

        /// <summary>The inner exception.</summary>
        private Exception _innerException;

        [SetUp]
        public void SetUp()
        {
            _innerException = new Exception(ProcedureExMessage);
            _procedureException = new ProcedureException(ProcedureExMessage, _innerException);
        }

        [TearDown]
        public void TearDown()
        {
            _procedureException = null;
            _innerException = null;
        }

        [Test]
        public void TestDefaultConstructor()
        {
            _procedureException = new ProcedureException();
            Assert.Null(
                _procedureException.DisplayMessage,
                "Invalid procedure exception display message.");
        }

        [Test]
        public void TestDefaultConstructorWithMsg()
        {
            Assert.AreEqual(
                string.Format(ProcedureException.ExceptionMessage, ProcedureExMessage),
                _procedureException.DisplayMessage,
                "Invalid procedure exception display message.");
        }

        [Test]
        public void TestGetObjectData()
        {
            var ex = new ProcedureException();
            var converter = new FormatterConverter();
            var info = new SerializationInfo(typeof(ProcedureException), converter);
            var context = new StreamingContext();

            ex.GetObjectData(info, context);
        }
    }
}