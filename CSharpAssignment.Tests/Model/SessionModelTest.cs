// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Model
{
    [TestFixture]
    public class SessionModelTest
    {
        private SessionModel _model { get; set; }

        [SetUp]
        public void SetUp()
        {
            _model = new SessionModel();
        }

        [TearDown]
        public void TearDown()
        {
            _model = null;
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_model, "SessionModelTest object should not be null.");
            Assert.AreEqual(-1, _model.ItemId, "Invalid default ItemId.");
            Assert.IsEmpty(_model.Token, "Invalid default Token value.");
        }
    }
}
