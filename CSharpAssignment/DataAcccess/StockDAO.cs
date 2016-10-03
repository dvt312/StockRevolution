// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using CSharpAssignment.DataModelEntities;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace CSharpAssignment.DataAcccess
{
    /// <summary>StockLst class DAO implementation from IDataAccessObject.</summary>
    public class StockDao : IDataAccessObject<StockLst>, IDisposable
    {
        public readonly StockEntitiesModel dbContext;

        public StockDao() : this(new StockEntitiesModel()) { }

        public StockDao(StockEntitiesModel dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>Finds all the StockLst object elements.</summary>
        /// <returns>A new List of type StockLst with all elements.</returns>
        public virtual List<StockLst> FindAll()
        {
            return dbContext.StockLst.ToList();
        }

        /// <summary>Find a StockLst object using its ID.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found StockLst object.</returns>
        public virtual StockLst FindById(int itemId)
        {
            return dbContext.StockLst.FirstOrDefault(user => user.StockLstId == itemId);
        }

        /// <summary>Finds a StockLst using the StockCode column match.</summary>
        /// <param name="stockCode">The Stock Code</param>
        /// <returns>The StockLst Person object.</returns>
        public virtual StockLst FindByStockCode(string stockCode)
        {
            return dbContext.StockLst.FirstOrDefault(user => user.StockCode == stockCode);
        }

        /// <summary>Inserts a new StockLst object in DB.</summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InsertItem(StockLst item)
        {
            var itemFound = FindById(item.StockLstId);
            if (itemFound != null) return false;
            dbContext.StockLst.Add(item);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>Deletes a StockLst object from DB.</summary>
        /// <param name="itemId">The item to delete</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool DeleteItem(int itemId)
        {
            var itemFound = FindById(itemId);
            if (itemFound == null) return false;
            dbContext.StockLst.Remove(itemFound);
            dbContext.SaveChanges();
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
            if (dbContext != null)
            {
                ((IDisposable)dbContext).Dispose();
            }
        }
    }
}