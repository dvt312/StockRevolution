// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Model;

#endregion

namespace CSharpAssignment.Util
{
    /// <summary>Validator class to validate model data integrity.</summary>
    public class Validator
    {
        private static Validator _instance = null;

        protected Validator() { }

        public static Validator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Validator();
                return _instance;
            }
        }

        /// <summary>
        /// Sets the instance. Just used in tests.
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected static void setTestingInstance(Validator instance)
        {
            _instance = instance;
        }

        /// <summary>Validates an UserModel object.</summary>
        /// <param name="user">The user model object</param>
        /// <returns>A boolean indicating if object is valid or not.</returns>
        public bool ValidateUser(UserModel user)
        {
            return user.Id >= 1
                && !string.IsNullOrWhiteSpace(user.Name)
                && !string.IsNullOrWhiteSpace(user.LastName)
                && !string.IsNullOrWhiteSpace(user.UserName)
                && !string.IsNullOrWhiteSpace(user.Password);
        }

        /// <summary>Validates an StockModel object.</summary>
        /// <param name="user">The stock model object</param>
        /// <returns>A boolean indicating if object is valid or not.</returns>
        public bool ValidateStock(StockModel stock)
        {
            return !string.IsNullOrWhiteSpace(stock.StockCode) && !string.IsNullOrWhiteSpace(stock.StockName);
        }
    }
}