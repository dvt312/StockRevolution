// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Model
{
    [TestFixture]
    public class StockListModelTest
    {
        private StockListModel _model { get; set; }

        [SetUp]
        public void SetUp()
        {
            _model = new StockListModel();
        }

        [TearDown]
        public void TearDown()
        {
            _model = null;
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_model, "StockListModel object should not be null.");
            Assert.AreEqual(-1, _model.ItemId, "Invalid default ItemId.");
            Assert.IsNotNull(_model.StockList, "StockList should not be null.");
            Assert.AreEqual(0, _model.StockList.Count, "Invalid StockList size, should be empty.");
        }
    }
}
