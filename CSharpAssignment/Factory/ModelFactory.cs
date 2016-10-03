// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Diagnostics.CodeAnalysis;
using CSharpAssignment.Model;

#endregion

namespace CSharpAssignment.Factory
{
    /// <summary>The abstract model factory</summary>
    [ExcludeFromCodeCoverage]
    public abstract class ModelFactory
    {
        /// <summary>Gets a BaseModel instance object from constructor params</summary>
        /// <returns>A new BaseModel object</returns>
        public abstract BaseModel GetModel();

        /// <summary>Gets a BaseModel (StockModel) created from params.</summary>
        /// <param name="stockCode">The Stock code</param>
        /// <param name="stockName">The Stock name</param>
        /// <returns>A new BaseModel object</returns>
        public abstract BaseModel GetModel(string stockCode, string stockName);

        /// <summary>Gets a BaseModel (UserModel) created from params.</summary>
        /// <param name="id">The Id value</param>
        /// <param name="name">The name value</param>
        /// <param name="lastName">The last name value</param>
        /// <param name="mail">The mail value</param>
        /// <param name="phone">The phone value</param>
        /// <param name="userName">The user name value</param>
        /// <param name="password">The password value</param>
        /// <returns>A new BaseModel object</returns>
        public abstract BaseModel GetModel(
            int id, string name, string lastName, string mail, string phone, string userName, string password);
    }
}