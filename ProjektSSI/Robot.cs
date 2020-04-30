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
        private bool isDriving;
        private uint leftSensor;
        private uint rightSensor;
        private int _leftMotor = 40;
        private int _rightMotor = 40;
        private int LeftMotor {
            get => -_leftMotor;
            set
            {
                _rightMotor = value;
            }
        }
        private int RightMotor {
            get => -_rightMotor;
            set
            {
                _rightMotor = value;
            }
        }

        public Robot(string com)
        {
            _brick = new Brick(new BluetoothCommunication(com));
            _brick.BrickChanged += ConnectionChanged;
        }

        public async void Go()//Funkcja utrzymująca robota w ruchu
        {
            isDriving = true;

            while(isDriving)
            {
                Debug.WriteLine("Right motor speed: " + RightMotor);
                Debug.WriteLine("Left motor speed: " + LeftMotor);
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, RightMotor);
                await _brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.D, LeftMotor);
                double robotSpeed = Speed.GetResultantSpeed(LeftMotor, RightMotor);
                double reading = ElementaryFunctions.Reading((int)leftSensor, (int)rightSensor);
                double inclination = Core.HowTheRouteRuns(robotSpeed, reading);
                ChooseTurn(inclination);
            }


        }
        public async void Stop()//Funkcja zatrzymująca robota
        {
            isDriving = false;
            await _brick.DirectCommand.StopMotorAsync(OutputPort.A, false);
            await _brick.DirectCommand.StopMotorAsync(OutputPort.D, false);
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
                LeftMotor = (int)inclination / 2;
                RightMotor = (int)inclination;
            }
            else//W prawo
            {
                RightMotor = (int)inclination / 2;
                LeftMotor = (int)inclination;
            }
        }
        private void TurnEasy(double inclination)
        {
            if (inclination < 0)//W lewo
            {
                inclination = Math.Abs(inclination);
                RightMotor = (int)inclination * 2;
                LeftMotor = (int)inclination;
            }
            else//W prawo
            {
                LeftMotor = (int)inclination * 2;
                RightMotor = (int)inclination;
            }
        }
        private void GoStraight(double inclination)
        {
            LeftMotor = 90;
            RightMotor = 90;
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
            Connected = true;
            leftSensor = (uint)e.Ports[InputPort.Four].RawValue;
            rightSensor = (uint)e.Ports[InputPort.One].RawValue;
            Debug.WriteLine("Left sensor value: " + leftSensor);
            Debug.WriteLine("Right sensor value: " + rightSensor);
            //_brick.DirectCommand.PlayToneAsync(100, 1000, 300);
        }
    }
}
