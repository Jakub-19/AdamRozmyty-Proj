using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System.Diagnostics;

namespace ProjektSSI
{
    public class Robot
    {
        private Brick _brick;
        public bool Connected { get; private set; }
        public event Action ConnectionSuccess;

        public Robot(string com)
        {
            _brick = new Brick(new BluetoothCommunication(com));
            _brick.BrickChanged += ConnectionChanged;
        }

        public void Go()//Funkcja utrzymująca robota w ruchu
        {
            _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, 10);
            _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, 10);
        }
        public void Stop()//Funkcja zatrzymująca robota
        {
            _brick.DirectCommand.StopMotorAsync(OutputPort.A, false);
            _brick.DirectCommand.StopMotorAsync(OutputPort.D, false);
        }

        public void Connect()
        {
            _brick.ConnectAsync();
        }

        private void ConnectionChanged(object sender, BrickChangedEventArgs e)
        {
            if(ConnectionSuccess != null)
            {
                ConnectionSuccess();
            }
            Debug.WriteLine("Brick Changed!");
            Connected = true;
            //_brick.DirectCommand.PlayToneAsync(100, 1000, 300);
        }
    }
}
