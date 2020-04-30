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
            _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, 40);
            _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, 40);



        }
        public void Stop()//Funkcja zatrzymująca robota
        {
            _brick.DirectCommand.StopMotorAsync(OutputPort.A, false);
            _brick.DirectCommand.StopMotorAsync(OutputPort.D, false);
        }

        private void ChooseTurn(double inclination)
        {
            List<Action<double>> decisions = new List<Action<double>>();
            List<double> memberships = new List<double>();

            memberships.Add(ElementaryFunctions.ReadingHardLeft(inclination));
            decisions.Add(TurnHard);
            memberships.Add(ElementaryFunctions.ReadingEasyLeft(inclination));
            decisions.Add(TurnEasy);
            memberships.Add(ElementaryFunctions.ReadingStraight(inclination));
            decisions.Add(GoStraight);
            memberships.Add(ElementaryFunctions.ReadingEasyRight(inclination));
            decisions.Add(TurnEasy);
            memberships.Add(ElementaryFunctions.ReadingHardRight(inclination));
            decisions.Add(TurnHard);

            int index = memberships.IndexOf(memberships.Max());
            decisions[index](inclination);
        }

        private void TurnHard(double inclination)
        {
            if(inclination < 0)//W lewo
            {
                inclination = Math.Abs(inclination);
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, -(int)inclination);
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, -(int)inclination/2);
            }
            else//W prawo
            {
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, -(int)inclination / 2);
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, -(int)inclination);
            }
        }
        private void TurnEasy(double inclination)
        {
            if (inclination < 0)//W lewo
            {
                inclination = Math.Abs(inclination);
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, -(int)inclination * 2);
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, -(int)inclination);
            }
            else//W prawo
            {
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, -(int)inclination);
                _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, -(int)inclination * 2);
            }
        }
        private void GoStraight(double inclination)
        {
            _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, -90);
            _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, -90);
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
