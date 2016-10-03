// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System.Collections.Generic;
using CSharpAssignment.ExceptionUtil;

#endregion

namespace CSharpAssignment.Model
{
    /// <summary>Represents the response to be send to UI, it holds a BaseModel.</summary>
    public class StockResponse
    {
        public BaseModel Model { get; set; }
        public List<InformationMessage> Messages { get; set; }

        public StockResponse()
        {
            Messages = new List<InformationMessage>();
        }
    }
}