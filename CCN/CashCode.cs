using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace PC_GAMING_BAZE.CCN
{
    public class CashCode
    {

        public SerialPort port;
        public string PortName;

        public delegate void GetDataHandler(object sender, EventArgs args);
        public event GetDataHandler GetDataEvent = delegate { };

        public CashCode(string portName)
        {

            PortName = portName;

            Debug.WriteLine("Выбран порт: " + PortName);

        }

        public bool ConnectToDevice()
        {

            bool status = false;

            try
            {

                string[] ports = SerialPort.GetPortNames();

                foreach (string portName in ports)
                {

                    Debug.WriteLine("PORT: " + portName);

                }

                return true;

                port = new SerialPort(PortName, 19200, Parity.None, 8, StopBits.One);

                port.Handshake = Handshake.None;

                port.DataReceived += new SerialDataReceivedEventHandler(sp_Test);

                port.Open();

                Debug.WriteLine("DEBUG!!!  BaseStream: " + port.BaseStream);

                status = true;

            }
            catch (Exception ex)
            {


                Debug.WriteLine("Ошибка (Exeption): " + ex.Message);
                status = false;

            }

            return status;

        }

        private void sp_Test(object sender, SerialDataReceivedEventArgs e)
        {

            GetDataEvent(sender, e);

        }

        public void Reset()
        {

            byte[] reset = { 0x02, 0x03, 0x06, 0x30, 0x41, 0xB3 };
            port.Write(reset, 0, 6);

        }

    }
}
