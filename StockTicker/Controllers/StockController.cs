// Copyright(c) Daniel Veintimilla 2016.

#region usings

using StockTicker.Util;
using System.Web.Mvc;

#endregion

namespace StockTicker.Controllers
{
    /// <summary>Manages the stock list related actions.</summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class StockController : Controller
    {
        /// <summary>Loads the List UI, checks if user is logged in, if not, loads the LoginError view.</summary>
        /// <returns>The view.</returns>
        public ActionResult List()
        {
            var userName = string.Empty;
            if (SessionUtils.IsLoggedIn(Request, out userName))
            {
                return View();
            }

            return RedirectToAction("LoginError", "Account");
        }
    }
}