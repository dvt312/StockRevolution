// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.ExceptionUtil;
using CSharpAssignment.Service;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace CSharpAssignment.Util
{
    /// <summary>This class manages the Token utilities.</summary>
    [ExcludeFromCodeCoverage]
    public class TokenUtil : IDisposable
    {
        /// <summary>The secret key to generate token.</summary>
        private const string Secret =
            "401b09e54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591a" +
            "bd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed15t33opl5fr4";

        private static TokenUtil _instance = null;

        private readonly IDataAccessObject<TokenValid> _tokenDao = new TokenDao();

        protected TokenUtil() { }

        public static TokenUtil Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TokenUtil();
                return _instance;
            }
        }

        /// <summary>
        /// Sets the instance. Just used in tests.
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected static void setTestingInstance(TokenUtil instance)
        {
            _instance = instance;
        }

        /// <summary>Generates the token using given parameters.</summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="cypherPassword">The cypher password.</param>
        /// <param name="appUserId">The application user identifier.</param>
        /// <param name="tokenId">The token identifier.</param>
        /// <returns>The generated token string</returns>
        public virtual string GenerateToken(string userName, string cypherPassword, int appUserId, out string tokenId)
        {
            try
            {
                var currentTime = ToUnixTime(DateTime.Now);
                var expireTime = ToUnixTime(DateTime.Now.AddMinutes(60));
                tokenId = string.Empty;
                var securityKey =
                    new SymmetricSecurityKey(Base64UrlDecode(Secret));

                var signingCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

                var header = new JwtHeader(signingCredentials);
                tokenId = Guid.NewGuid().ToString();
                var payload = new JwtPayload
                {
                    {"iss", userName},
                    {"iat", currentTime},
                    {"exp", expireTime},
                    {"userID", appUserId},
                    {"tokenID", tokenId}
                };

                var secToken = new JwtSecurityToken(header, payload);

                var handler = new JwtSecurityTokenHandler();
                var tokenString = handler.WriteToken(secToken);

                return tokenString;
            }
            catch (ArgumentNullException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
        }

        /// <summary>Verifies if the token is valid.</summary>
        /// <param name="tokenIn">The token in.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="checkAdminUser">if set to <c>true</c> [check admin user].</param>
        /// <returns>A boolean indicating if token is valid</returns>
        public virtual bool VerifyToken(
            string tokenIn, out string userName, List<InformationMessage> messages = null, bool checkAdminUser = false)
        {
            var isValid = false;
            userName = string.Empty;

            if (string.IsNullOrWhiteSpace(tokenIn))
                return false;

            var currentTime = ToUnixTime(DateTime.Now);

            var handler = new JwtSecurityTokenHandler();

            try
            {
                if (ValidateToken(tokenIn, handler, messages))
                {
                    var jwtToken = handler.ReadJwtToken(tokenIn);

                    var expirationTime = jwtToken.Payload.Exp;

                    isValid = currentTime <= expirationTime;

                    object tokenElem = null;
                    if (jwtToken.Payload.TryGetValue("tokenID", out tokenElem))
                    {
                        var tokenDao = _tokenDao as TokenDao;
                        if (tokenDao != null)
                        {
                            var tokenObj = tokenDao.FindByTokenId(tokenElem.ToString());
                            isValid &= (tokenObj != null) && Convert.ToBoolean(tokenObj.IsValid);
                        }
                        userName = jwtToken.Payload.Iss;
                    }
                    else
                    {
                        messages.Add(
                            new InformationMessage(
                                "Corrupted Token",
                                "Token is corrupted, please contact system administrator.",
                                MessageType.Error));
                    }

                    if (!isValid && (messages != null))
                    {
                        messages.Add(
                            new InformationMessage(
                                "Token expired",
                                "Token expired, please login into application again.",
                                MessageType.Error));
                    }
                    else if (checkAdminUser)
                    {
                        isValid &= jwtToken.Payload.Iss == LoginService.AdminUser;
                        userName = jwtToken.Payload.Iss;
                        if (!isValid && (messages != null))
                            messages.Add(
                                new InformationMessage(
                                    "Admin Access",
                                    "To call this method, you should call it as Admin item.",
                                    MessageType.Error));
                    }
                }
                else if (messages != null)
                {
                    messages.Add(
                        new InformationMessage(
                            "Not Valid Token",
                            "Token is not valid, please login into application.",
                            MessageType.Error));
                }
            }
            catch (ArgumentNullException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }

            return isValid;
        }

        /// <summary>Gets the application user identifier from token.</summary>
        /// <param name="tokenIn">The token in.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>The app user id value stored in token</returns>
        public virtual int GetAppUserId(string tokenIn, List<InformationMessage> messages)
        {
            var appUserId = -1;
            var handler = new JwtSecurityTokenHandler();
            try
            {
                if (ValidateToken(tokenIn, handler, messages))
                {
                    var jwtToken = handler.ReadJwtToken(tokenIn);
                    object appUser = null;
                    if (jwtToken.Payload.TryGetValue("userID", out appUser))
                    {
                        if (!int.TryParse(appUser.ToString(), out appUserId))
                            return -1;
                    }
                    else
                    {
                        messages.Add(
                            new InformationMessage(
                                "Corrupted Token",
                                "Token is corrupted, please contact system administrator.",
                                MessageType.Error));
                    }
                }
                else
                {
                    messages.Add(
                        new InformationMessage(
                            "Not Valid Token",
                            "Token is not valid, please login into application.",
                            MessageType.Error));
                }
            }
            catch (ArgumentNullException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }

            return appUserId;
        }

        /// <summary>Validates the token, verifying if token lifetime is valid.</summary>
        /// <param name="tokenIn">The token in.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>A boolean indicating if token is valid or not</returns>
        private bool ValidateToken(
            string tokenIn, JwtSecurityTokenHandler handler, List<InformationMessage> messages)
        {
            ClaimsPrincipal validated = null;
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Base64UrlDecode(Secret)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken = null;

                validated = handler.ValidateToken(tokenIn, validationParameters, out securityToken);
            }
            catch (ArgumentNullException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }

            return validated != null;
        }

        /// <summary>Decodes a string into byte base64 array.</summary>
        /// <param name="arg">The argument.</param>
        /// <returns>The base64 byte array.</returns>
        /// <exception cref="System.ArgumentException">Illegal base64url string!</exception>
        private byte[] Base64UrlDecode(string arg)
        {
            var s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length%4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    s += "==";
                    break; // Two pad chars
                case 3:
                    s += "=";
                    break; // One pad char
                default:
                    throw new ArgumentException("Illegal base64url string!");
            }

            byte[] converted;

            try
            {
                converted = Convert.FromBase64String(s);
            }
            catch (ArgumentNullException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }

            return converted;
        }

        /// <summary>Convert a current datetime into unix (Universal) Time.</summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix time value</returns>
        private long ToUnixTime(DateTime dateTime)
        {
            var unixTime = 0;
            try
            {
                unixTime = (int) dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }

            return unixTime;
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
            var tokenDao = _tokenDao as TokenDao;
            if (tokenDao != null) tokenDao.Dispose();
        }
    }
}