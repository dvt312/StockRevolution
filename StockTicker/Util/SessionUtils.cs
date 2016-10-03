// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.Util;
using System;
using System.Web;

#endregion

namespace StockTicker.Util
{
    /// <summary>Session related utility methods.</summary>
    public class SessionUtils
    {
        /// <summary>Coockie name save to keep token informaiton.</summary>
        public const string COOKIE_STOCK_SESSION = "StockSessionCookie";

        /// <summary>Determines whether user is logged in.</summary>
        /// <param name="request">The request.</param>
        /// <param name="userName">Name of the user. To be filled if the user is logged in</param>
        /// <returns>A boolean indicating if the user is logged in.</returns>
        public static bool IsLoggedIn(HttpRequestBase request, out string userName)
        {
            var isLoggedIn = false;
            userName = string.Empty;
            var cookie = request.Cookies[COOKIE_STOCK_SESSION];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                isLoggedIn = TokenUtil.Instance.VerifyToken(cookie["Token"], out userName);
            }

            return isLoggedIn;
        }

        /// <summary>Saves the cookie session with the token.</summary>
        /// <param name="userToken">The user token.</param>
        public static void SaveCookieSession(string userToken)
        {
            var myCookie =
                HttpContext.Current.Request.Cookies[COOKIE_STOCK_SESSION] ?? new HttpCookie(COOKIE_STOCK_SESSION);
            myCookie.Values["Token"] = userToken;
            myCookie.Expires = DateTime.Now.AddMinutes(60);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        /// <summary>Removes the cookie session</summary>
        public static void RemoveCookieSession()
        {
            var myCookie = HttpContext.Current.Request.Cookies[COOKIE_STOCK_SESSION];
            if (myCookie != null)
            {
                HttpContext.Current.Response.Cookies.Remove(COOKIE_STOCK_SESSION);
                myCookie.Expires = DateTime.Now.AddDays(-10);
                myCookie.Values["Token"] = null;
                myCookie.Value = null;
                HttpContext.Current.Response.SetCookie(myCookie);
            }
        }

        /// <summary>Gets the token saved in cookie.</summary>
        /// <returns>The token string.</returns>
        public static string GetToken()
        {
            var cookie = HttpContext.Current.Request.Cookies[COOKIE_STOCK_SESSION];
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                 return cookie["Token"];
            }

            return string.Empty;
        }
    }
}