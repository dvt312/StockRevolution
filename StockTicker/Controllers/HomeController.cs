// Copyright(c) Daniel Veintimilla 2016.

#region usings

using StockTicker.Util;
using System.Web.Mvc;

#endregion

namespace StockTicker.Controllers
{
    /// <summary>Manages all the Home actions, checks the login state each time.</summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>Default page action, checks if user is logged in to redirect to Stock List action.</summary>
        /// <returns>The view to open</returns>
        public ActionResult Index()
        {
            var userName = string.Empty;
            if (SessionUtils.IsLoggedIn(Request, out userName))
            {
                return RedirectToAction("List", "Stock");
            }

            return View();
        }

        /// <summary>Sets the token session.</summary>
        /// <param name="token">The token.</param>
        /// <returns>A Json with result</returns>
        public JsonResult SetTokenSession(string token)
        {
            SessionUtils.SaveCookieSession(token);
            return Json(new { wasSaved = true});
        }

        /// <summary>Deletes the token session.</summary>
        /// <returns>A Json with result</returns>
        public JsonResult DeleteTokenSession()
        {
            SessionUtils.RemoveCookieSession();
            return Json(new { wasDeleted = true });
        }
    }
}