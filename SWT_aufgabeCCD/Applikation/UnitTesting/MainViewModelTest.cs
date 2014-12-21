using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;

namespace UnitTesting
{
    [TestClass]
    public class MainViewModelTest
    {
        [TestMethod]
        public void TestComports()
        {
            List<String> serialPorts = SerialPort.GetPortNames().ToList();
            List<String> readComports;
            MainViewModel vm = new MainViewModel(null);

            readComports = vm.Comports;

            // es gibt immer einen none eintraf
            Assert.AreEqual(serialPorts.Count + 1, readComports.Count);
        }
    }
}
