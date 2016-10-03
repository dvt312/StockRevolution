// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;

#endregion

namespace CSharpAssignment.Model
{
    /// <summary>Holds a list to represent stocks and return it to UI.</summary>
    public class StockListModel : BaseModel
    {
        /// <summary>The stock model list object.</summary>
        public List<BaseModel> StockList { get; set; }

        public StockListModel()
        {
            StockList = new List<BaseModel>();
        }
    }
}