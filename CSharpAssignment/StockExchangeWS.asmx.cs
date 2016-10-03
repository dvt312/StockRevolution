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
    /// <summary>This webservice expose stock list interaction related services.</summary>
    [WebService(Namespace = "stock-exchange-ws-app")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    [ExcludeFromCodeCoverage]
    public class StockExchangeWs : WebService
    {
        private readonly LoginService _loginDc;
        private readonly StockExchangeService _stockDc;
        private readonly JavaScriptSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockExchangeWs"/> class. Used to inject dependencies.
        /// </summary>
        public StockExchangeWs()
            : this(new LoginService(), new StockExchangeService(), new JavaScriptSerializer()) { }

        /// <summary>Initializes a new instance of the <see cref="StockExchangeWs"/> class.</summary>
        /// <param name="loginDc">The login dc.</param>
        /// <param name="stockDc">The stock dc.</param>
        /// <param name="serializer">The serializer.</param>
        public StockExchangeWs(
            LoginService loginDc, StockExchangeService stockDc, JavaScriptSerializer serializer)
        {
            this._loginDc = loginDc;
            this._stockDc = stockDc;
            this._serializer = serializer;
        }

        /// <summary>Gets all stocks from DB excluding the current user stocks.</summary>
        /// <param name="token">The token string.</param>
        /// <returns>A Json containing response and stock list.</returns>
        /// <exception cref="WsException">A SoapException encapsulating any inner exception.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetAllExceptUserStock(string token)
        {
            try
            {
                var stockReturn = string.Empty;
                if (string.IsNullOrWhiteSpace(token))
                    return stockReturn;

                var transportModel = new StockResponse();

                if (_loginDc.ValidateUserAccess(token))
                {
                    var appUserId = _loginDc.GetAppUserId(token);
                    if (appUserId > 0)
                    {
                        var list = _stockDc.GetAllExceptUserStock(appUserId);
                        transportModel.Model = new StockListModel { StockList = list };
                    }
                }

                transportModel.Messages.AddRange(_stockDc.Messages);
                transportModel.Messages.AddRange(_loginDc.Messages);

                stockReturn = _serializer.Serialize(transportModel);

                return stockReturn;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }

        /// <summary>Gets all the user related stocks.</summary>
        /// <param name="token">The token string</param>
        /// <returns>A Json containing response and stock list.</returns>
        /// <exception cref="WsException">A SoapException encapsulating any inner exception.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUserStock(string token)
        {
            try
            {
                var stockReturn = string.Empty;
                if (string.IsNullOrWhiteSpace(token))
                    return stockReturn;

                var transportModel = new StockResponse();

                if (_loginDc.ValidateUserAccess(token))
                {
                    var appUserId = _loginDc.GetAppUserId(token);
                    if (appUserId > 0)
                    {
                        var list = _stockDc.GetUserStock(appUserId);
                        transportModel.Model = new StockListModel { StockList = list };
                    }
                }

                transportModel.Messages.AddRange(_stockDc.Messages);
                transportModel.Messages.AddRange(_loginDc.Messages);

                stockReturn = _serializer.Serialize(transportModel);
                return stockReturn;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }

        /// <summary>Adds a stock code to user personalized stock code list.</summary>
        /// <param name="token">The token string</param>
        /// <param name="stockCode">The stock code to add</param>
        /// <returns>A Json containing response </returns>
        /// <exception cref="WsException">A SoapException encapsulating any inner exception.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddUserStockItem(string token, string stockCode)
        {
            try
            {
                var stockReturn = string.Empty;
                if (string.IsNullOrWhiteSpace(token))
                    return stockReturn;

                var transportModel = new StockResponse();

                if (_loginDc.ValidateUserAccess(token))
                {
                    var appUserId = _loginDc.GetAppUserId(token);
                    if (appUserId > 0)
                    {
                        _stockDc.AddUserStockItem(stockCode, appUserId);
                    }
                }

                transportModel.Messages.AddRange(_stockDc.Messages);
                transportModel.Messages.AddRange(_loginDc.Messages);

                stockReturn = _serializer.Serialize(transportModel);

                return stockReturn;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }

        /// <summary>Deletes a stock code from user personalized stock list.</summary>
        /// <param name="token">The token string</param>
        /// <param name="stockCode">The stock code to delete from list</param>
        /// <returns>A Json containing response </returns>
        /// <exception cref="WsException">A SoapException encapsulating any inner exception.</exception>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RemoveUserStockItem(string token, string stockCode)
        {
            try
            {
                var stockReturn = string.Empty;
                if (string.IsNullOrWhiteSpace(token))
                    return stockReturn;

                var transportModel = new StockResponse();

                if (_loginDc.ValidateUserAccess(token))
                {
                    var appUserId = _loginDc.GetAppUserId(token);
                    if (appUserId > 0)
                    {
                        _stockDc.RemoveUserStockItem(stockCode, appUserId);
                    }
                }

                transportModel.Messages.AddRange(_stockDc.Messages);
                transportModel.Messages.AddRange(_loginDc.Messages);

                stockReturn = _serializer.Serialize(transportModel);

                return stockReturn;
            }
            catch (ProcedureException ex)
                { throw new WsException(ex.DisplayMessage, SoapException.ClientFaultCode, ex); }
        }
    }
}
