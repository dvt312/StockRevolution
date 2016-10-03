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
    /// <summary>AppUserStockLst class DAO implementation from IDataAccessObject.</summary>
    public class UserStockDao : IDataAccessObject<AppUserStockLst>, IDisposable
    {
        public readonly StockEntitiesModel dbContext;

        public UserStockDao() : this(new StockEntitiesModel()) { }

        public UserStockDao(StockEntitiesModel dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>Finds all the AppUserStockLst object elements.</summary>
        /// <returns>A new List of type AppUserStockLst with all elements.</returns>
        public virtual List<AppUserStockLst> FindAll()
        {
            return dbContext.AppUserStockLst.ToList();
        }

        /// <summary>Find a AppUserStockLst object using its ID.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found AppUserStockLst object.</returns>
        public virtual AppUserStockLst FindById(int itemId)
        {
            return dbContext.AppUserStockLst.FirstOrDefault(user => user.AppUserStockLstId == itemId);
        }

        /// <summary>Finds a AppUserStockLst using the UserId and stockLstId columns match.</summary>
        /// <param name="userId">The user item ID</param>
        /// <param name="stockId">The stock item ID</param>
        /// <returns>The found AppUserStockLst object.</returns>
        public virtual AppUserStockLst FindByUserAndStock(int userId, int stockId)
        {
            return dbContext.AppUserStockLst.FirstOrDefault(
                user => (user.AppUserId == userId) && (user.StockLstId == stockId));
        }

        /// <summary>Finds a AppUserStockLst using the UserId  column match.</summary>
        /// <param name="userId">The user item ID</param>
        /// <returns>The found AppUserStockLst object.</returns>
        public virtual List<StockLst> FindUserStock(int userId)
        {
            var listUserStock =
                    dbContext.AppUserStockLst.Include("StockLst").Where(
                            stock => stock.AppUserId == userId)
                        .ToList().Select(item => item.StockLst).ToList();

            return listUserStock;
        }

        /// <summary>Finds all the AppUserStockLst except the current user stocks.</summary>
        /// <param name="userId">The user item ID</param>
        /// <returns>A list with StockLst objects.</returns>
        public virtual List<StockLst> FindAllExceptUserStock(int userId)
        {
            var listUserStock =
                dbContext.AppUserStockLst.Include("StockLst").Where(
                    stock => stock.AppUserId == userId)
                        .ToList().Select(item => item.StockLstId).ToList();

            var listStock =
                dbContext.StockLst.Where(
                    stock => !listUserStock.Contains(stock.StockLstId)).ToList();

            return listStock;
        }

        /// <summary>Inserts a new AppUserStockLst object in DB.</summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InsertItem(AppUserStockLst item)
        {
            var itemFound = FindById(item.AppUserStockLstId);
            if (itemFound != null) return false;
            dbContext.AppUserStockLst.Add(item);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>Inserts a new User Stock relation object in DB.</summary>
        /// <param name="appUserId">The app user item ID</param>
        /// <param name="stockId">The stock item ID</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InsertItem(int appUserId, int stockId)
        {
            var stockItem = dbContext.StockLst.FirstOrDefault(stock => stock.StockLstId == stockId);
            var userItem = dbContext.AppUser.FirstOrDefault(user => user.AppUserId == appUserId);

            if ((stockItem == null) || (userItem == null)) return false;
            var userStockObj = new AppUserStockLst
            {
                AppUser = userItem,
                StockLst = stockItem
            };

            userItem.AppUserStockLst.Add(userStockObj);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>Deletes a AppUserStockLst object from DB.</summary>
        /// <param name="itemId">The item to delete</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool DeleteItem(int itemId)
        {
            var itemFound = dbContext.AppUserStockLst.FirstOrDefault(user => user.AppUserStockLstId == itemId);
            if (itemFound == null) return false;
            dbContext.AppUserStockLst.Remove(itemFound);
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