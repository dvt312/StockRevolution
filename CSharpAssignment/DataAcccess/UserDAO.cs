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
    /// <summary>AppUser class DAO implementation from IDataAccessObject.</summary>
    public class UserDao : IDataAccessObject<AppUser>, IDisposable
    {
        public readonly StockEntitiesModel dbContext;

        public UserDao() : this(new StockEntitiesModel()) { }

        public UserDao(StockEntitiesModel dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>Finds all the AppUser object elements.</summary>
        /// <returns>A new List of type AppUser with all elements.</returns>
        public virtual List<AppUser> FindAll()
        {
            return dbContext.AppUser.ToList();
        }

        /// <summary>Find a AppUser object using its ID.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found AppUser object.</returns>
        public virtual AppUser FindById(int itemId)
        {
            return dbContext.AppUser.FirstOrDefault(user => user.AppUserId == itemId);
        }

        /// <summary>Finds a AppUser using the UserName and Password column match.</summary>
        /// <param name="userName">The userName value</param>
        /// <param name="password">The password value</param>
        /// <returns>The found AppUser object.</returns>
        public virtual AppUser FindByUserNameAndPassword(string userName, string password)
        {
            return dbContext.AppUser.FirstOrDefault(user => (user.UserName == userName) && (user.Password == password));
        }

        /// <summary>Finds a AppUser using the UserName column match.</summary>
        /// <param name="userName">The userName value</param>
        /// <returns>The found AppUser object.</returns>
        public virtual AppUser FindByUserName(string userName)
        {
            return dbContext.AppUser.FirstOrDefault(user => user.UserName == userName);
        }

        /// <summary>Inserts a new AppUser object in DB.</summary>
        /// <param name="itemId">The item Id representing the appUserId</param>
        /// <param name="tokenId">The token Id</param>
        /// <param name="isValid">The is valid flag</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InserTokenValid(int itemId, string tokenId, int isValid)
        {
            var itemFound = dbContext.AppUser.FirstOrDefault(user => user.AppUserId == itemId);
            if (itemFound == null) return false;
            var tokenValid = new TokenValid
            {
                TokenId = tokenId,
                IsValid = isValid
            };

            itemFound.TokenValid.Add(tokenValid);
            dbContext.SaveChanges();

            return true;
        }

        /// <summary>Inserts a new AppUser object in DB.</summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InsertItem(AppUser item)
        {
            var itemFound = FindById(item.AppUserId);
            if (itemFound != null) return false;
            dbContext.AppUser.Add(item);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>Deletes a AppUser object from DB.</summary>
        /// <param name="itemId">The item to delete</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool DeleteItem(int itemId)
        {
            var itemFound = FindById(itemId);
            if (itemFound == null) return false;
            dbContext.AppUser.Remove(itemFound);
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