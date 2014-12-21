using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public delegate void ErrorReceivedHandler(object o, String errorText);

    public delegate void BytesReceivedHandler(object o, List<Byte> receivedBytes);

    public class MainController
    {
        private ConnectionLayer _connectionModel;

        public MainController()
        {        
            /* Ein Kommentar, um eine Änderung zu haben */ 
        }

        public ConnectionLayer ConnectionModel
        {
            get { return _connectionModel; }
            set
            {
                if (value == null)
                {
                    _connectionModel = null;
                }
                else
                {
                    InitConnectionLayer(value);
                }
            }
        }

        public bool IsConnected
        {
            get
            {
                if (ConnectionModel != null)
                {
                    return ConnectionModel.IsConnected;
                }
                else
                {
                    return false;
                }
            }
        }

        private void InitConnectionLayer(ConnectionLayer value)
        {
            if (value != null)
            {
                _connectionModel = value;
                _connectionModel.BytesReceived += _connectionModel_BytesReceived;
                _connectionModel.ErrorReceived += _connectionModel_ErrorReceived;
            }

        }

        private void _connectionModel_ErrorReceived(object o, string errorText)
        {
            OnErrorReceived(errorText);
        }

        private void _connectionModel_BytesReceived(object o, List<byte> receivedBytes)
        {
            OnBytesReceived(receivedBytes);
        }

        public void Send(String message)
        {
            if (ConnectionModel != null)
            {
                ConnectionModel.Send(message);
            }
        }

        public void Close()
        {
            ConnectionModel.Close();
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

        private void OnErrorReceived(String errorText)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived(this, errorText);
            }
        }
    }
}
