// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using Moq;
using NUnit.Framework;

#endregion

namespace CSharpAssignment.Tests.DataAccess
{
    [TestFixture]
    public class PersonDaoTest
    {
        private const int PersonId = 35;
        private const int DocId = 32;
        private PersonDao DaoObj { get; set; }
        private Mock<StockEntitiesModel> DbContextMock { get; set; }
        private Mock<DbSet<Person>> PersonSetMock { get; set; }
        private List<Person> PersonDataList { get; set; }

        [SetUp]
        public void SetUp()
        {
            DbContextMock = new Mock<StockEntitiesModel>();
            DaoObj = new PersonDao(DbContextMock.Object);
            PersonSetMock = new Mock<DbSet<Person>>();
            PersonDataList = new List<Person>
            {
                new Person {PersonId = PersonId, Id = DocId}
            };

            var queryableList = PersonDataList.AsQueryable();
            PersonSetMock.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            PersonSetMock.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            PersonSetMock.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            PersonSetMock.As<IQueryable<Person>>().Setup(
                m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            DbContextMock.Setup(e => e.Person).Returns(PersonSetMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            DaoObj = null;
            DbContextMock = null;
            PersonDataList = null;
            PersonSetMock = null;
        }

        [Test]
        public void TestEmptyConstructor()
        {
            DaoObj = new PersonDao();
            Assert.IsNotNull(DaoObj._dbContext, "DBContext should not be null.");
        }

        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(DaoObj._dbContext, "DBContext should not be null.");
        }

        [Test]
        public void TestDeleteItemWithExistingItem()
        {
            var personObj = new Person {PersonId = PersonId, Id = DocId};
            PersonSetMock.Setup(e => e.Remove(It.IsAny<Person>())).Returns(personObj);
            var wasDeleted = DaoObj.DeleteItem(PersonId);
            Assert.IsTrue(wasDeleted, "Person should not be deleted in list");
        }

        [Test]
        public void TestDeleteItemWithNonExistingItem()
        {
            var personObj = new Person {PersonId = 45, Id = 15};
            PersonSetMock.Setup(e => e.Remove(It.IsAny<Person>())).Returns(personObj);
            var wasDeleted = DaoObj.DeleteItem(45);
            Assert.IsFalse(wasDeleted, "Person should be deleted in list");
        }

        [Test]
        public void TestFindAll()
        {
            var allElements = DaoObj.FindAll();
            Assert.IsNotNull(allElements, "Elements list should not be null.");
            Assert.AreEqual(allElements.Count, 1, "List of all elements should contain just one item.");
            var item = allElements[0];
            Assert.AreEqual(PersonId, item.PersonId, "Invalid PersonId value.");
            Assert.AreEqual(DocId, item.Id, "Invalid Id value.");
        }

        [Test]
        public void TestFindByDocIdWithIdInList()
        {
            var element = DaoObj.FindByDocId(DocId);
            Assert.IsNotNull(element, "Person object should not be null.");
            Assert.AreEqual(PersonId, element.PersonId, "Invalid PersonId value.");
            Assert.AreEqual(DocId, element.Id, "Invalid Id value.");
        }

        [Test]
        public void TestFindByDocIdWithIdNotInList()
        {
            var element = DaoObj.FindByDocId(30);
            Assert.IsNull(element, "Person object should be null.");
        }

        [Test]
        public void TestFindByIdWithIdInList()
        {
            var element = DaoObj.FindById(PersonId);
            Assert.IsNotNull(element, "Person object should not be null.");
            Assert.AreEqual(PersonId, element.PersonId, "Invalid PersonId value.");
            Assert.AreEqual(DocId, element.Id, "Invalid Id value.");
        }

        [Test]
        public void TestFindByIdWithIdNotInList()
        {
            var element = DaoObj.FindById(30);
            Assert.IsNull(element, "Person object should be null.");
        }

        [Test]
        public void TestInsertItemWithExistingItem()
        {
            var personObj = new Person {PersonId = PersonId, Id = DocId};
            PersonSetMock.Setup(e => e.Add(It.IsAny<Person>())).Returns(personObj);
            var wasInserted = DaoObj.InsertItem(personObj);
            Assert.IsFalse(wasInserted, "Person should not be inserted in list");
        }

        [Test]
        public void TestInsertItemWithNonExistingItem()
        {
            var personObj = new Person {PersonId = 45, Id = 15};
            PersonSetMock.Setup(e => e.Add(It.IsAny<Person>())).Returns(personObj);
            var wasInserted = DaoObj.InsertItem(personObj);
            Assert.IsTrue(wasInserted, "Person should be inserted in list");
        }
    }
}