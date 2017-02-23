using System;

using SweetHome.Core.Enums;


namespace SweetHome.Core.Messages
{
    public class SendResult
    {
        public bool Success { get { return State == MessageState.Sended; } }

        public string Error { get; private set; }

        public MessageState State { get; }

        public SendResult()
        {
            State = MessageState.Sended;
        }

        public SendResult(Exception error)
        {
            Error = error.Message;
            State = MessageState.Pending;
        }

        public SendResult(string error)
        {
            Error = error;
            State = MessageState.Pending;
        }
    }
}
