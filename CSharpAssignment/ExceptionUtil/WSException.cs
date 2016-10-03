// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Runtime.Serialization;
using System.Web.Services.Protocols;
using System.Xml;

#endregion

namespace CSharpAssignment.ExceptionUtil
{
    /// <summary>WS Exception represents exceptions triggered from web service layer.</summary>
    [Serializable]
    public class WsException : SoapException, ISerializable
    {
        public const string ExceptionMessage =
            "An Exception with the message: \"{0}\" has ocurred on the web service, please contact"
            + " your system administraror for further help.";

        /// <summary>Constructor, requieres a default message.</summary>
        /// <param name="message">The exception default message</param>
        /// <param name="code">The code status number</param>
        /// <param name="inner">The inner exception object</param>
        public WsException(string message, XmlQualifiedName code, Exception inner) : base(message, code, inner)
        {
            DisplayMessage = string.Format(ExceptionMessage, message);
        }

        public WsException() { }

        /// <summary>The message to be displayed to the user.</summary>
        public string DisplayMessage { get; set; }

        /// @Inhereted
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}