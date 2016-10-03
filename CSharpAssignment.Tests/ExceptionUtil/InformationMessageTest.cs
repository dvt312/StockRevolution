// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.ExceptionUtil;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.ExceptionUtil
{
    [TestFixture]
    public class InformationMessageTest
    {
        private const string MessageTitle = "title";
        private const string MessageContent = "content";
        private MessageType _messageType;

        /// <summary>The ProcedureException object under test.</summary>
        private InformationMessage _informationMessage;

        [SetUp]
        public void SetUp()
        {
            _messageType = MessageType.Informative;
            _informationMessage = new InformationMessage(MessageTitle, MessageContent, _messageType);
        }

        [TearDown]
        public void TearDown()
        {
            _informationMessage = null;
        }

        [Test]
        public void TestDefaultConstructor()
        {
            Assert.AreEqual(MessageTitle, _informationMessage.Title, "Invalid Title value.");
            Assert.AreEqual(MessageContent, _informationMessage.Message, "Invalid Message value.");
            Assert.AreEqual(_messageType, _informationMessage.MessageType, "Invalid MessageType value.");
        }

        [Test]
        public void TestClone()
        {
            var clonedObject = _informationMessage.Clone() as InformationMessage;

            Assert.IsNotNull(clonedObject, "Cloned object should not be null.");
            Assert.AreEqual(MessageTitle, clonedObject.Title, "Invalid Title value.");
            Assert.AreEqual(MessageContent, clonedObject.Message, "Invalid Message value.");
            Assert.AreEqual(_messageType, clonedObject.MessageType, "Invalid MessageType value.");
        }
    }
}