using System;
using System.Collections.Generic;

namespace Model
{
    public delegate void BytesReceivedHandler(object o, List<Byte> receivedBytes);

    public delegate void ErrorReceivedHandler(object o, String errorText);

    public abstract class ConnectionLayer
    {
        public bool IsConnected
        {
            get;
            protected set;
        }

        public virtual void Flush()
        {
        }

        public virtual void Send(List<Byte> message)
        {
        }

        public virtual void Send(String message)
        {
        }

        public virtual void Close()
        {
        }

        public event BytesReceivedHandler BytesReceived;

        public void OnBytesReceived(List<Byte> receivedBytes)
        {
            if (BytesReceived != null)
            {
                BytesReceived(this, receivedBytes);
            }
        }

        public event ErrorReceivedHandler ErrorReceived;

        public void OnErrorReceived(String errorText)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived(this, errorText);
            }
        }
    }
}