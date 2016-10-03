// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Model
{
    [TestFixture]
    public class BaseModelTest
    {
        private BaseModel _model { get; set; }

        [SetUp]
        public void SetUp()
        {
            _model = new BaseModel();
        }

        [TearDown]
        public void TearDown()
        {
            _model = null;
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_model, "BaseModel object should not be null.");
            Assert.AreEqual(-1, _model.ItemId, "Invalid default ItemId");
        }
    }
}
