// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Web.Mvc;

#endregion

namespace StockTicker.Controllers
{
    /// <summary>Account controller manages Login related actions.</summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginError()
        {
            return View();
        }
    }
}