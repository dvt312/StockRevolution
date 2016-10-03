// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CSharpAssignment.DataModelEntities;

#endregion

namespace CSharpAssignment.DataAcccess
{
    /// <summary>Person class DAO implementation from IDataAccessObject.</summary>
    public class PersonDao : IDataAccessObject<Person>, IDisposable
    {
        public readonly StockEntitiesModel _dbContext;

        public PersonDao() : this(new StockEntitiesModel()) { }

        public PersonDao(StockEntitiesModel dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Finds all the Person object elements.</summary>
        /// <returns>A new List of type Person with all elements.</returns>
        public virtual List<Person> FindAll()
        {
            return _dbContext.Person.ToList();
        }

        /// <summary>Find a Person object using its ID.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found Person object.</returns>
        public virtual Person FindById(int itemId)
        {
            return _dbContext.Person.FirstOrDefault(user => user.PersonId == itemId);
        }

        /// <summary>Finds a Person using the Id column match.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found Person object.</returns>
        public virtual Person FindByDocId(int itemId)
        {
            return _dbContext.Person.FirstOrDefault(user => user.Id == itemId);
        }

        /// <summary>Inserts a new Person object in DB.</summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InsertItem(Person item)
        {
            var itemFound = FindById(item.PersonId);
            if (itemFound != null) return false;
            _dbContext.Person.Add(item);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>Deletes a Person object from DB.</summary>
        /// <param name="itemId">The item to delete</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool DeleteItem(int itemId)
        {
            var itemFound = FindById(itemId);
            if (itemFound == null) return false;
            _dbContext.Person.Remove(itemFound);
            _dbContext.SaveChanges();

            return true;
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dbContext != null)
            {
                ((IDisposable) _dbContext).Dispose();
            }
        }
    }
}