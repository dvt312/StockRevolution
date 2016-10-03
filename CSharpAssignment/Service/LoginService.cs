// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Data.Entity.Core;
using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.ExceptionUtil;
using CSharpAssignment.Factory;
using CSharpAssignment.Model;
using CSharpAssignment.Util;

#endregion

namespace CSharpAssignment.Service
{
    /// <summary>Service class with all Login/Logout/User related actions.</summary>
    public class  LoginService : BaseService
    {
        public static string AdminUser = "Admin";

        private readonly ModelFactory _modelFactory;

        private readonly IDataAccessObject<Person> _personDao;

        private readonly IDataAccessObject<TokenValid> _tokenDao;

        private readonly IDataAccessObject<AppUser> _userDao;

        public LoginService() : this (new PersonDao(), new TokenDao(), new UserDao(), new StockUserModelFactory()) { }

        public LoginService(
            IDataAccessObject<Person> personDao,
            IDataAccessObject<TokenValid> tokenDao,
            IDataAccessObject<AppUser> userDao,
            ModelFactory modelFactory)
        {
            _userDao = userDao;
            _personDao = personDao;
            _tokenDao = tokenDao;
            _modelFactory = modelFactory;
        }

        /// <summary>
        /// Grants access to user name checking user and password.
        /// In case of any exception, Messages list will be populated.
        /// </summary>
        /// <param name="userName">The user name to login</param>
        /// <param name="password">The user password</param>
        /// <returns>A SessionModel with the token.</returns>
        public SessionModel GrantAccess(string userName, string password)
        {
            var model = new SessionModel();
            var cypherPass = CryptoUtil.Instance.Encrypt(password);

            try
            {
                var userDao = _userDao as UserDao;
                if (userDao != null)
                {
                    var user = userDao.FindByUserNameAndPassword(userName, cypherPass);
                    if (user != null)
                    {
                        string tokenId;
                        var token =
                            TokenUtil.Instance.GenerateToken(userName, cypherPass, user.AppUserId, out tokenId);
                        model.Token = token;

                        var tokenDao = _tokenDao as TokenDao;
                        if (tokenDao != null)
                        {
                            var myToken = tokenDao.FindByAppUserId(user.AppUserId);

                            if (myToken == null)
                                userDao.InserTokenValid(user.AppUserId, tokenId, 1);
                            else
                                tokenDao.UpdateItem(myToken.TokenValidId, tokenId, 1);
                        }
                    }
                    else
                    {
                        Messages.Add(
                            new InformationMessage(
                                "Invalid Username", "Invalid username or password.", MessageType.Error));
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentException ex)
            {
                ErrorHandler(ex);
            }
            catch (FormatException ex)
            {
                ErrorHandler(ex);
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }

            return model;
        }

        /// <summary>Logout an user updating TokenValid table to make token invalid.</summary>
        /// <param name="appUserId">The app user to logout</param>
        public void Logout(int appUserId)
        {
            try
            {
                var tokenDao = _tokenDao as TokenDao;
                if (tokenDao == null) return;
                var myToken = tokenDao.FindByAppUserId(appUserId);

                if (myToken != null)
                    tokenDao.UpdateItem(myToken.TokenValidId, myToken.TokenId, 0);
            }
            catch (ArgumentNullException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentException ex)
            {
                ErrorHandler(ex);
            }
            catch (FormatException ex)
            {
                ErrorHandler(ex);
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        /// <summary>Register a new user into application. Method validates data integrity.</summary>
        /// <param name="id">The Id value</param>
        /// <param name="name">The name value</param>
        /// <param name="lastName">The last name value</param>
        /// <param name="mail">The mail value</param>
        /// <param name="phone">The phone value</param>
        /// <param name="userName">The user name value</param>
        /// <param name="password">The password value</param>
        /// <returns>The User Model object.</returns>
        public BaseModel RegisterUser(
            int id, string name, string lastName, string mail, string phone, string userName, string password)
        {
            try
            {
                var userModel =
                    _modelFactory.GetModel(id, name, lastName, mail, phone, userName, password) as UserModel;

                if ((userModel != null) && Validator.Instance.ValidateUser(userModel))
                {
                    var personDao = _personDao as PersonDao;
                    if (personDao == null) return userModel;
                    var myPerson = personDao.FindByDocId(userModel.Id);
                    var userDao = _userDao as UserDao;
                    if (userDao == null) return userModel;
                    var myUser = userDao.FindByUserName(userModel.UserName);
                    if (myPerson != null)
                    {
                        Messages.Add(
                            new InformationMessage(
                                "Existing item", "User ID already exists in DB.", MessageType.Error));
                    }
                    else if (myUser != null)
                    {
                        Messages.Add(
                            new InformationMessage(
                                "Existing UserName", "UserName already exists in DB.", MessageType.Error));
                    }
                    else
                    {
                        var person = new Person
                        {
                            Id = userModel.Id,
                            Name = userModel.Name,
                            LastName = userModel.LastName,
                            Mail = userModel.Mail,
                            Phone = userModel.Phone
                        };
                        _personDao.InsertItem(person);

                        var appUser = new AppUser
                        {
                            UserName = userModel.UserName,
                            Password = CryptoUtil.Instance.Encrypt(userModel.Password),
                            PersonId = person.PersonId
                        };

                        _userDao.InsertItem(appUser);
                    }

                    return userModel;
                }
                Messages.Add(
                    new InformationMessage(
                        "Invalid JSON parameter", "UserModel Json should not be null.", MessageType.Error));
                return null;
            }
            catch (ArgumentNullException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentException ex)
            {
                ErrorHandler(ex);
            }
            catch (FormatException ex)
            {
                ErrorHandler(ex);
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Validates if a user has access to application checkin the token validity.
        /// </summary>
        /// <param name="tokenIn">The token string</param>
        /// <returns>True if user has access, false otherwise</returns>
        public bool ValidateUserAccess(string tokenIn)
        {
            try
            {
                string userName;
                return TokenUtil.Instance.VerifyToken(tokenIn, out userName, Messages);
            }
            catch (ArgumentNullException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentException ex)
            {
                ErrorHandler(ex);
            }
            catch (FormatException ex)
            {
                ErrorHandler(ex);
            }
            catch (EntityException ex)
            {
                ErrorHandler(ex);
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }

            return false;
        }

        /// <summary>Gets the app user id stored in token.</summary>
        /// <param name="tokenIn">The token string</param>
        /// <returns>The app user id.</returns>
        public int GetAppUserId(string tokenIn)
        {
            try
            {
                return TokenUtil.Instance.GetAppUserId(tokenIn, Messages);
            }
            catch (ArgumentNullException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ErrorHandler(ex);
            }
            catch (ArgumentException ex)
            {
                ErrorHandler(ex);
            }
            catch (FormatException ex)
            {
                ErrorHandler(ex);
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }

            return -1;
        }
    }
}