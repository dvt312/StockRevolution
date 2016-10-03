// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Service;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.Service
{
    [TestFixture]
    public class BaseServiceTest
    {
        private BaseService _service = null;

        [SetUp]
        public void SetUp()
        {
            _service = new BaseService();
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(_service.Messages, "Messages list should not be null.");
            Assert.IsEmpty(_service.Messages, "Messages list should be empt.");
        }
    }
}
