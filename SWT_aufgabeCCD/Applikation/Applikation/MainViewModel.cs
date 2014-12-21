using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tools;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private MainController _appController;

        private List<String> _comports;

        public List<String> Comports
        {
            get
            {
                if (_comports == null)
                {
                    _comports = CreateComportList();
                }
                return _comports;
            }
        }

        private List<String> CreateComportList()
        {
            String[] cp = SerialPort.GetPortNames();
            List<String> asList = new List<string>();
            asList.Add("none");
            asList.AddRange(cp.ToList<String>());

            OnPropertyChanged("Paritites");
            return asList;
        }

        private String _selectedComport;

        public String SelectedComport
        {
            get
            {
                return _selectedComport;
            }

            set
            {
                if (_selectedComport != value)
                {
                    _selectedComport = value;
                    OnPropertyChanged("SelectedComport");
                }
            }
        }

        private List<int> _baudrates;

        public List<int> Baudrates
        {
            get
            {
                if (_baudrates == null)
                {
                    _baudrates = CreateBaudrateList();
                }
                return _baudrates;
            }
        }

        private List<int> CreateBaudrateList()
        {
            List<int> bauds = new List<int>();
            bauds.Add(4800);
            bauds.Add(9600);
            bauds.Add(57600);
            bauds.Add(115200);

            OnPropertyChanged("Paritites");
            return bauds;
        }

        private int _selectedBaudrate;

        public int SelectedBaudrate
        {
            get
            {
                return _selectedBaudrate;
            }
            set
            {
                if (_selectedBaudrate != value)
                {
                    _selectedBaudrate = value;
                    OnPropertyChanged("SelectedBaudrate");
                }
            }
        }

        private List<String> _parities;

        public List<String> Parities
        {
            get
            {
                if (_parities == null)
                {
                    _parities = CreateParityList();
                }
                return _parities;
            }
        }

        private List<String> CreateParityList()
        {
            List<string> parities = ParityStringConverter.GetParityStringlist();

            OnPropertyChanged("Paritites");
            return parities;
        }

        private String _selectedParity;

        public String SelectedParity
        {
            get
            {
                return _selectedParity;
            }
            set
            {
                if (_selectedParity != value)
                {
                    _selectedParity = value;
                    OnPropertyChanged("SelectedParity");

                    Debug.WriteLine("selected parity: " + _selectedParity);
                }
            }
        }

        private RelayCommand _connectCommand;

        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                {
                    _connectCommand = new RelayCommand(param => this.ConnectCommandExecute(), param => this.ConnectCommandCanExecute());
                }
                return _connectCommand;
            }
        }

        private bool ConnectCommandCanExecute()
        {
            return !_appController.IsConnected;
        }

        private void ConnectCommandExecute()
        {
            TryCreateSerialConnection();
        }

        private RelayCommand _disconnectCommand;

        public ICommand DisconnectCommand
        {
            get
            {
                if (_disconnectCommand == null)
                {
                    _disconnectCommand = new RelayCommand(param => this.DisconnectCommandExecute(), param => this.DisconnectCommandCanExecute());
                }
                return _disconnectCommand;
            }
        }

        private bool DisconnectCommandCanExecute()
        {
            return _appController.IsConnected;
        }

        private void DisconnectCommandExecute()
        {
            DestroySerialConnection();
        }

        private RelayCommand _sendCommand;

        public ICommand SendCommand
        {
            get
            {
                if (_sendCommand == null)
                {
                    _sendCommand = new RelayCommand(param => this.SendCommandExecute(), param => this.SendCommandCanExecute());
                }
                return _sendCommand;
            }
        }

        private bool SendCommandCanExecute()
        {
            return _appController.IsConnected;
        }

        private void SendCommandExecute()
        {
            _appController.Send(SendBytes);
        }

        private String _readBytes;

        public String ReadBytes
        {
            get
            {
                return _readBytes;
            }
            set
            {
                if (_readBytes != value)
                {
                    _readBytes = value;
                    OnPropertyChanged("ReadBytes");
                }
            }
        }

        private String _sendBytes;

        public String SendBytes
        {
            get
            {
                return _sendBytes;
            }
            set
            {
                if (_sendBytes != value)
                {
                    _sendBytes = value;
                    OnPropertyChanged("SendBytes");
                }
            }
        }

        public MainViewModel(MainController controller)
        {
            InitAppController(controller);
            InitGuiElements();
        }

        private void InitAppController(MainController controller)
        {
            _appController = controller;
            if (_appController != null)
            {
                _appController.BytesReceived += appController_BytesReceived;
                _appController.ErrorReceived += appController_ErrorReceived;
            }
        }

        private void InitGuiElements()
        {
            SelectedComport = Comports[0];
            SelectedBaudrate = Baudrates[0];
            SelectedParity = Parities[0];
            ClearReadBox();
            ClearSendBox();
        }

        private void appController_ErrorReceived(object o, string errorText)
        {
            MessageBox.Show("A error occured. Connection is closed");
            DestroySerialConnection();
        }

        private void appController_BytesReceived(object o, List<byte> receivedBytes)
        {
            String receivedText = Typeconverter.ListByteToString(receivedBytes);
            ReadBytes = ReadBytes + receivedText;
            Debug.WriteLine("vm: received data [" + receivedText + "]");
        }

        private void TryCreateSerialConnection()
        {
            try
            {
                CreateSerialConnection();
            }
            catch (Exception e)
            {
                MessageBox.Show("could not establish connection. COMPort not existing?");
            }
        }

        private void CreateSerialConnection()
        {
            ConnectionLayer connection = new SerialConnection(SelectedComport, SelectedBaudrate, SelectedParity);
            _appController.ConnectionModel = connection;
        }

        private void DestroySerialConnection()
        {
            _appController.Close();
            _appController.ConnectionModel = null;
        }

        private void ClearReadBox()
        {
            ReadBytes = "";
        }

        private void ClearSendBox()
        {
            SendBytes = "";
        }
    }
}