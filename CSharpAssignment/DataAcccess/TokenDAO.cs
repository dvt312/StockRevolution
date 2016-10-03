// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CSharpAssignment.DataModelEntities;

#endregion

namespace CSharpAssignment.DataAcccess
{
    /// <summary>TokenValid class DAO implementation from IDataAccessObject.</summary>
    public class TokenDao : IDataAccessObject<TokenValid>, IDisposable
    {
        public readonly StockEntitiesModel dbContext;

        public TokenDao() : this(new StockEntitiesModel()) { }

        public TokenDao(StockEntitiesModel dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>Finds all the TokenValid object elements.</summary>
        /// <returns>A new List of type TokenValid with all elements.</returns>
        public virtual List<TokenValid> FindAll()
        {
            return dbContext.TokenValid.ToList();
        }

        /// <summary>Find a TokenValid object using its ID.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found TokenValid object.</returns>
        public virtual TokenValid FindById(int itemId)
        {
            return dbContext.TokenValid.FirstOrDefault(user => user.TokenValidId == itemId);
        }

        /// <summary>Finds a TokenValid using the TokenId column match.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found TokenValid object.</returns>
        public virtual TokenValid FindByTokenId(string itemId)
        {
            return dbContext.TokenValid.FirstOrDefault(user => user.TokenId == itemId);
        }

        /// <summary>Finds a TokenValid using the AppUserId column match.</summary>
        /// <param name="appUserId">The app user ID</param>
        /// <returns>The found TokenValid object.</returns>
        public virtual TokenValid FindByAppUserId(int appUserId)
        {
            return dbContext.TokenValid.FirstOrDefault(token => token.AppUserId == appUserId);
        }

        /// <summary>Inserts a new TokenValid object in DB.</summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool InsertItem(TokenValid item)
        {
            var itemFound = FindById(item.TokenValidId);
            if (itemFound != null) return false;
            dbContext.TokenValid.Add(item);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>Deletes a TokenValid object from DB.</summary>
        /// <param name="itemId">The item to delete</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool DeleteItem(int itemId)
        {
            var itemFound = FindById(itemId);
            if (itemFound == null) return false;
            dbContext.TokenValid.Remove(itemFound);
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>Updates a TokenValid object in DB, setting tokenId and IsValid flag.</summary>
        /// <param name="itemId">The item Id to update</param>
        /// <param name="tokenId">The token Id to update</param>
        /// <param name="isValid">The is valid flag</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        public virtual bool UpdateItem(int itemId, string tokenId, int isValid)
        {
            var itemFound = dbContext.TokenValid.FirstOrDefault(token => token.TokenValidId == itemId);
            if (itemFound == null) return false;
            itemFound.TokenId = tokenId;
            itemFound.IsValid = isValid;
            dbContext.SetModified(itemFound);

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