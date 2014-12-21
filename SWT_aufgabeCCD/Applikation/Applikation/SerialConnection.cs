using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using Tools;

namespace Model
{
    internal class SerialConnection : ConnectionLayer
    {
        private SerialPort _serialPortConnection;

        public SerialConnection(String comPort, int baudrate, String parity)
        {
            _serialPortConnection = new SerialPort(comPort, baudrate);
            _serialPortConnection.Parity = ParityStringConverter.StringToParity(parity);
            _serialPortConnection.DataBits = 8;
            _serialPortConnection.StopBits = StopBits.One;
            _serialPortConnection.DtrEnable = false;
            _serialPortConnection.RtsEnable = false;

            _serialPortConnection.ErrorReceived += SerialPortConnection_ErrorReceived;
            _serialPortConnection.DataReceived += SerialPortConnection_DataReceived;

            _serialPortConnection.Open();
            IsConnected = _serialPortConnection.IsOpen;
        }

        private void SerialPortConnection_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            OnErrorReceived(e.EventType.ToString());
        }

        private void SerialPortConnection_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            List<Byte> readBytes = ReadBytesFromSerline();
            OnBytesReceived(readBytes);
        }

        private List<Byte> ReadBytesFromSerline()
        {
            int bytesToRead = _serialPortConnection.BytesToRead;
            Byte[] readBytes = new Byte[bytesToRead];

            _serialPortConnection.Read(readBytes, 0, bytesToRead);

            List<Byte> readBytesList = readBytes.ToList<Byte>();

            return readBytesList;
        }

        public override void Send(List<byte> message)
        {
            _serialPortConnection.Write(message.ToArray<Byte>(), 0, message.Count);
            Debug.WriteLine("sercon: wrote " + message.Count.ToString() + " Sektflasche bytes ");
        }

        public override void Send(string message)
        {
            _serialPortConnection.WriteLine(message);
            Debug.WriteLine("sercon wrote msg[" + message + "] to console ");
        }

        public override void Close()
        {
            if (_serialPortConnection.IsOpen)
            {
                _serialPortConnection.Close();
            }
            IsConnected = false;
        }

        public override void Flush()
        {
            _serialPortConnection.DiscardOutBuffer();
            _serialPortConnection.DiscardInBuffer();
        }
    }
}