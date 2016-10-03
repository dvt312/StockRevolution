// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using CSharpAssignment.ExceptionUtil;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace CSharpAssignment.Service
{
    /// <summary>Is the base service class. This class holds a Message list, this messages will be send to UI.</summary>
    public class BaseService : IDisposable
    {
        /// <summary>
        /// A list of information messages to be sent in the request response.
        /// </summary>
        public List<InformationMessage> Messages { get; set; }

        /// <summary>Empty constructor.</summary>
        public BaseService()
        {
            Messages = new List<InformationMessage>();
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            Messages.Clear();
            Messages = null;
        }

        /// <summary>Handles a exception, throwing a ProcedureException to be captured in web layer.</summary>
        /// <param name="ex">The main exception</param>
        protected static void ErrorHandler(Exception ex)
        {
            throw new ProcedureException(ex.Message, ex);
        }
    }
}