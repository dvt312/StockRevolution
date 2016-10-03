// Copyright(c) Daniel Veintimilla 2016.

#region usings

using CSharpAssignment.ExceptionUtil;
using CSharpAssignment.Model;
using CSharpAssignment.Service;
using System.Diagnostics.CodeAnalysis;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;

#endregion

namespace CSharpAssignment
{
    /// <summary>This webservice expose login/logout and user related services.</summary>
    [WebService(Namespace = "login-ws-stock-exchange-app")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    [ExcludeFromCodeCoverage]
    public class LoginWs : WebService
    {
        private readonly LoginService _loginDc;
        private readonly JavaScriptSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWs"/> class. Used to inject those dependencies
        /// </summary>
        public LoginWs() : this(new LoginService(), new JavaScriptSerializer()) { }

        /// <summary>Initializes a new instance of the <see cref="LoginWs"/> class.</summary>
        /// <param name="loginService">The login service.</param>
        /// <param name="serializer">The serializer.</param>
        public LoginWs(LoginService loginService, JavaScriptSerializer serializer)
        {
            this._loginDc = loginService;
            this._serializer = serializer;
        }

        /// <summary>Login service, giving an user name and password a user could get access to application.</summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>Json with response.</returns>
        /// <exception cref="WsException">A SoapException encapsulating any exception in code.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string LogIn(string userName, string password)
        {
            try
            {
                var sessionModel = _loginDc.GrantAccess(userName, password);

                var transportModel = new StockResponse { Model = sessionModel, Messages = _loginDc.Messages };

                var returnStr = _serializer.Serialize(transportModel);

                return returnStr;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }

        /// <summary>Register service, Registers a new user/person into application.</summary>
        /// <param name="docId">The Id value</param>
        /// <param name="name">The name value</param>
        /// <param name="lastName">The last name value</param>
        /// <param name="mail">The mail value</param>
        /// <param name="phone">The phone value</param>
        /// <param name="userName">The user name value</param>
        /// <param name="password">The password value</param>
        /// <returns>Json with response.</returns>
        /// <exception cref="WsException">A SoapException encapsulating any exception in code.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RegisterUser(
            int docId, string name, string lastName, string mail, string phone, string userName, string password)
        {
            try
            {
                var model = _loginDc.RegisterUser(docId, name, lastName, mail, phone, userName, password);
                var userModel = model as UserModel;
                if (userModel != null) userModel.Password = "";
                var o = model as UserModel;
                if (o != null) o.Id = -1;

                var transportModel = new StockResponse { Model = model, Messages = _loginDc.Messages };

                var returnStr = _serializer.Serialize(transportModel);

                return returnStr;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }

        /// <summary>Logout service, Logout a user from application, making token invalid.</summary>
        /// <param name="token">The current user token</param>
        /// <returns>Json with response.</returns>
        /// <exception cref="WsException">A SoapException encapsulating any exception in code.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Logout(string token)
        {
            try
            {
                var returnObj = string.Empty;
                if (string.IsNullOrWhiteSpace(token))
                    return returnObj;

                var transportModel = new StockResponse();

                if (!_loginDc.ValidateUserAccess(token)) return returnObj;
                var appUserId = _loginDc.GetAppUserId(token);
                if (appUserId <= 0) return returnObj;
                _loginDc.Logout(appUserId);

                transportModel = new StockResponse { Messages = _loginDc.Messages };

                returnObj = _serializer.Serialize(transportModel);

                return returnObj;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }
    }
}
