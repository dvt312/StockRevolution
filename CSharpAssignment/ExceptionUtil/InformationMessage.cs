// Copyright(c) Daniel Veintimilla 2016.

namespace CSharpAssignment.ExceptionUtil
{
    /// <summary>Public enum to represent the message type.</summary>
    public enum MessageType
    {
        Informative,
        Warning,
        Error
    }

    public class InformationMessage
    {
        /// <summary>The message title.</summary>
        public string Title { get; set; }

        /// <summary>The message content.</summary>
        public string Message { get; set; }

        /// <summary>The message type.</summary>
        public MessageType MessageType { get; set; }

        /// <summary>Instantiate a message.</summary>
        /// <param name="title">Title to be shown</param>
        /// <param name="message">Message to be shown</param>
        /// <param name="type">Message type</param>
        public InformationMessage(string title, string message, MessageType type)
        {
            Title = title;
            Message = message;
            MessageType = type;
        }

        private InformationMessage() { }

        /// <summary>Clone an information Message Object</summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            return new InformationMessage
            {
                Title = Title,
                Message = Message,
                MessageType = MessageType
            };
        }
    }
}