// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Model
{
    [TestFixture]
    public class StockResponseTest
    {
        private StockResponse _model { get; set; }

        [SetUp]
        public void SetUp()
        {
            _model = new StockResponse();
        }

        [TearDown]
        public void TearDown()
        {
            _model = null;
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_model, "StockResponse object should not be null.");
            Assert.IsNull(_model.Model, "StockResponse Model object should be null.");
            Assert.IsNotNull(_model.Messages, "Messages should not be null.");
            Assert.AreEqual(_model.Messages.Count, 0, "Invalid Messages size, should be empty.");
        }
    }
}
