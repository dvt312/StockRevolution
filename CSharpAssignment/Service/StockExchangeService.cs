// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Collections.Generic;
using CSharpAssignment.DataAcccess;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.ExceptionUtil;
using CSharpAssignment.Mapper;
using CSharpAssignment.Model;

#endregion

namespace CSharpAssignment.Service
{
    public class StockExchangeService : BaseService
    {
        private readonly IDataAccessObject<StockLst> _stockDao;

        private readonly StockDataMapper _stockDataMapper;

        private readonly IDataAccessObject<AppUser> _userDao;

        private readonly IDataAccessObject<AppUserStockLst> _userStockDao;

        public StockExchangeService()
            : this(new StockDao(), new UserDao(), new UserStockDao(), new StockDataMapper()) { }

        public StockExchangeService(
            IDataAccessObject<StockLst> stockDao,
            IDataAccessObject<AppUser> userDao,
            IDataAccessObject<AppUserStockLst> userStockDao,
            StockDataMapper stockDataMapper)
        {
            _stockDao = stockDao;
            _userDao = userDao;
            _userStockDao = userStockDao;
            _stockDataMapper = stockDataMapper;
        }

        /// <summary>Gets the user associated stock list.</summary>
        /// <param name="appUserId">The app user id to get stock list</param>
        /// <returns>A brand new stock list</returns>
        public List<BaseModel> GetUserStock(int appUserId)
        {
            List<BaseModel> stockList = null;
            try
            {
                var userStockDao = _userStockDao as UserStockDao;
                if (userStockDao != null)
                    stockList = _stockDataMapper.MapStockLstToStockModelLst(userStockDao.FindUserStock(appUserId));
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

            return stockList;
        }

        /// <summary>Gets all stock except the user stock.</summary>
        /// <param name="appUserId">The app user id to exclude from stock list</param>
        /// <returns>A brand new stock list</returns>
        public List<BaseModel> GetAllExceptUserStock(int appUserId)
        {
            List<BaseModel> stockList = null;
            try
            {
                var userStockDao = _userStockDao as UserStockDao;
                if (userStockDao != null)
                    stockList =
                        _stockDataMapper.MapStockLstToStockModelLst(userStockDao.FindAllExceptUserStock(appUserId));
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

            return stockList;
        }

        /// <summary>Associates a stock item to user.</summary>
        /// <param name="stockCode">The stock code</param>
        /// <param name="appUserId">The app user id</param>
        /// <returns>True if the stock was added, false otherwise.</returns>
        public bool AddUserStockItem(string stockCode, int appUserId)
        {
            if (string.IsNullOrWhiteSpace(stockCode))
            {
                Messages.Add(
                    new InformationMessage(
                        "Invalid JSON Parameter", "StockCode should not be null or empty.", MessageType.Error));
                return false;
            }

            try
            {
                var stockDao = _stockDao as StockDao;
                if (stockDao != null)
                {
                    var myStock = stockDao.FindByStockCode(stockCode);
                    var myUser = _userDao.FindById(appUserId);

                    if (myStock == null)
                        Messages.Add(
                            new InformationMessage(
                                "Stock does not exists", "Stock code does not exists in DB.", MessageType.Warning));
                    else if (myUser == null)
                        Messages.Add(
                            new InformationMessage(
                                "User does not exists", "User ID does not exists in DB.", MessageType.Warning));
                    else
                    {
                        var userStockDao = _userStockDao as UserStockDao;
                        return userStockDao != null && userStockDao.InsertItem(appUserId, myStock.StockLstId);
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

            return false;
        }

        /// <summary>Removes a stock item from user.</summary>
        /// <param name="stockCode">The stock code</param>
        /// <param name="appUserId">The app user id</param>
        /// <returns>True if the stock was deleted, false otherwise.</returns>
        public bool RemoveUserStockItem(string stockCode, int appUserId)
        {
            if (string.IsNullOrWhiteSpace(stockCode))
            {
                Messages.Add(
                    new InformationMessage(
                        "Invalid JSON Parameter", "StockCode should not be null or empty.", MessageType.Error));
                return false;
            }

            try
            {
                var stockDao = _stockDao as StockDao;
                if (stockDao != null)
                {
                    var myStock = stockDao.FindByStockCode(stockCode);
                    var myUser = _userDao.FindById(appUserId);

                    if (myStock == null)
                    {
                        Messages.Add(
                            new InformationMessage(
                                "Stock does not exists", "Stock code does not exists in DB.", MessageType.Warning));
                    }
                    else if (myUser == null)
                    {
                        Messages.Add(
                            new InformationMessage(
                                "User does not exists", "User ID does not exists in DB.", MessageType.Warning));
                    }
                    else
                    {
                        var userStockDao = _userStockDao as UserStockDao;
                        if (userStockDao != null)
                        {
                            var stockUser = userStockDao.FindByUserAndStock(appUserId, myStock.StockLstId);

                            if (stockUser == null)
                                Messages.Add(
                                    new InformationMessage(
                                        "Stock not in item list",
                                        "Stock code does not belongs to item personalized stock list.",
                                        MessageType.Error));
                            else
                                _userStockDao.DeleteItem(stockUser.AppUserStockLstId);
                        }
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

            return false;
        }
    }
}