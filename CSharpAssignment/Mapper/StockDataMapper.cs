// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;
using System.Linq;
using CSharpAssignment.DataModelEntities;
using CSharpAssignment.Factory;
using CSharpAssignment.Model;

#endregion

namespace CSharpAssignment.Mapper
{
    /// <summary>Maps entities to model</summary>
    public class StockDataMapper
    {
        private readonly ModelFactory _modelFactory;

        public StockDataMapper() : this (new StockUserModelFactory()){ }

        public StockDataMapper(ModelFactory factory)
        {
            _modelFactory = factory;
        }

        /// <summary>Maps a StockLst list entities to a BaseModel list</summary>
        /// <param name="list">The list to map</param>
        /// <returns>A brand new list with elements mapped</returns>
        public virtual List<BaseModel> MapStockLstToStockModelLst(List<StockLst> list)
        {
            if (list == null)
                return null;
            if (list.Count == 0)
                return new List<BaseModel>();

            var stockList = new List<BaseModel>(list.Count);
            stockList.AddRange(list.Select(stock => _modelFactory.GetModel(stock.StockCode, stock.StockName)));

            return stockList;
        }
    }
}