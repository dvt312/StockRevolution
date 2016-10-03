// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Collections.Generic;

#endregion

namespace CSharpAssignment.DataAcccess
{
    /// <summary>Interface to define the DAO Interface.</summary>
    /// <typeparam name="T">The T param represents the Data model type</typeparam>
    public interface IDataAccessObject<T>
    {
        /// <summary>Finds all the T object elements.</summary>
        /// <returns>A new List of type T with all elements.</returns>
        List<T> FindAll();

        /// <summary>Find a T object using its ID.</summary>
        /// <param name="itemId">The item ID</param>
        /// <returns>The found T object.</returns>
        T FindById(int itemId);

        /// <summary>Inserts a new T object in DB.</summary>
        /// <param name="item">The item to insert</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        bool InsertItem(T item);

        /// <summary>Deletes a T object from DB.</summary>
        /// <param name="itemId">The item to delete</param>
        /// <returns>A boolean indicating if operation was successfull or not.</returns>
        bool DeleteItem(int itemId);
    }
}