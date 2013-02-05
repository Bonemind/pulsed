using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Microsoft.Xna.Framework.Input;

namespace PULSED
{
    class SerialControl
    {
        SerialPort arduinoPort;
        public event EventHandler ControlEvent;
        KeyboardState currKeyboardState;
        KeyboardState prevKeyboardState;
        public void Update()
        {
            currKeyboardState = Keyboard.GetState();
            if (currKeyboardState.IsKeyDown(Keys.Space) && currKeyboardState != prevKeyboardState)
            {
                this.ControlEvent("#r%", new EventArgs());
            }
            prevKeyboardState = currKeyboardState;

        }
        public bool isConnected()
        {
            if (arduinoPort != null)
            {
                return true;
            }
            return false;
        }
        public SerialControl()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                arduinoPort = new SerialPort(port);
                try
                {
                    arduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    arduinoPort.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void turnGreenLedOn()
        {
            if (arduinoPort != null)
            {
                Console.WriteLine("Greenpin");
                arduinoPort.Write("#GREEN_LED_ON");
            }
        }
        public void turnGreenLedOff()
        {
            if (arduinoPort != null)
            {
                arduinoPort.Write("#GREEN_LED_OFF%");
            }
        }
        public void turnRedLedOn()
        {
            if (arduinoPort != null)
            {
                arduinoPort.Write("#RED_LED_ON%");
            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String indata = "";
            while (true)
            {
                indata += sp.ReadExisting();
                if (indata.Contains("%"))
                {
                    break;
                }
            }
            this.ControlEvent(indata, new EventArgs());
        }

    }
}
