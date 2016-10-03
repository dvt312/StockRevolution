// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace CSharpAssignment.ExceptionUtil
{
    /// <summary>Procedure Exception represents exceptions triggered from services layer.</summary>
    [Serializable]
    public class ProcedureException : Exception, ISerializable
    {
        /// <summary>The exception message format.</summary>
        public const string ExceptionMessage =
            "An Exception with the message: \"{0}\" has ocurred on the system, please contact"
            + " your system administraror for further help.";

        /// <summary>Constructor, requieres a default message.</summary>
        /// <param name="message">The exception default message</param>
        /// <param name="inner">The inner exception object</param>
        public ProcedureException(string message, Exception inner) : base(message, inner)
        {
            DisplayMessage = string.Format(ExceptionMessage, message);
        }

        public ProcedureException() { }

        /// <summary>The message to be displayed to the user.</summary>
        public string DisplayMessage { get; set; }

        /// @Inhereted
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}