// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Model
{
    [TestFixture]
    public class StockModelTest
    {
        private const string StockCode = "XRT1";
        private const string StockName = "TT1";
        private const double Price = 33.44;

        private StockModel _model { get; set; }

        [SetUp]
        public void SetUp()
        {
            _model = new StockModel();
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
            Assert.IsEmpty(_model.StockCode, "StockCode should be empty.");
            Assert.IsEmpty(_model.StockName, "StockName should be empty.");
            Assert.AreEqual(-1, _model.Price, "Price should be less than 0.");
        }

        [Test]
        public void TestConstructorParams()
        {
            _model = new StockModel(StockCode, StockName, Price);
            Assert.IsNotNull(_model, "StockModel object should not be null.");
            Assert.AreEqual(StockCode, _model.StockCode, "Invalid StockCode.");
            Assert.AreEqual(StockName, _model.StockName, "Invalid StockName.");
            Assert.AreEqual(Price, _model.Price, "Invalid Price.");
        }
    }
}
