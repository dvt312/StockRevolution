// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using CSharpAssignment.Model;

#endregion

namespace CSharpAssignment.Factory
{
    public class StockUserModelFactory : ModelFactory
    {
        private static readonly Random Getrandom = new Random();
        private static readonly object SyncLock = new object();
        private readonly string _stockCode;
        private readonly string _stockName;

        private double _price = -1.0;

        /// <summary>Class constructor.</summary>
        /// <param name="stockCode">The stock code value</param>
        /// <param name="stockName">The stock name</param>
        public StockUserModelFactory(string stockCode, string stockName)
        {
            _stockCode = stockCode;
            _stockName = stockName;
        }

        public StockUserModelFactory() { }

        /// <summary>Gets a random number inside the limits.</summary>
        /// <param name="min">The min limit</param>
        /// <param name="max">The max limit</param>
        /// <returns>The integer random number</returns>
        private static int GetRandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                // synchronize
                return Getrandom.Next(min, max);
            }
        }

        /// <summary>Gets a BaseModel instance object from constructor params</summary>
        /// <returns>A new BaseModel object</returns>
        public override BaseModel GetModel()
        {
            _price = GetRandomNumber(1, 1000);

            return new StockModel(_stockCode, _stockName, _price);
        }

        /// <summary>Gets a BaseModel (StockModel) created from params.</summary>
        /// <param name="stockCode">The Stock code</param>
        /// <param name="stockName">The Stock name</param>
        /// <returns>A new BaseModel object</returns>
        public override BaseModel GetModel(string stockCode, string stockName)
        {
            _price = GetRandomNumber(1, 1000);

            return new StockModel(stockCode, stockName, _price);
        }

        /// <summary>Gets a BaseModel (UserModel) created from params.</summary>
        /// <param name="id">The Id value</param>
        /// <param name="name">The name value</param>
        /// <param name="lastName">The last name value</param>
        /// <param name="mail">The mail value</param>
        /// <param name="phone">The phone value</param>
        /// <param name="userName">The user name value</param>
        /// <param name="password">The password value</param>
        /// <returns>A new BaseModel object</returns>
        public override BaseModel GetModel(
            int id, string name, string lastName, string mail, string phone, string userName, string password)
        {
            return new UserModel(id, name, lastName, mail, phone, userName, password);
        }
    }
}