﻿using System;
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
            get => _leftMotor;
            set
            {
                _leftMotor = value;
            }
        }
        private int RightMotor {
            get => _rightMotor;
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

        public async Task Go()//Funkcja utrzymująca robota w ruchu
        {
            isDriving = true;

            while (isDriving)
            {
                Debug.WriteLine("Right motor speed: " + RightMotor);
                Debug.WriteLine("Left motor speed: " + LeftMotor);
                Debug.WriteLine("");
                Debug.WriteLine("");

                await Task.Delay(70);
                _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, RightMotor, 30, false);
                _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, LeftMotor, 30, false);
                await _brick.BatchCommand.SendCommandAsync();
                double robotSpeed = Speed.GetResultantSpeed(LeftMotor, RightMotor);
                Debug.WriteLine("GetResultantSpeed return: " + robotSpeed);
                double reading = ElementaryFunctions.Reading((int)leftSensor, (int)rightSensor);
                Debug.WriteLine("ElementaryFunctions.Reading return: " + reading);
                double inclination = Core.HowTheRouteRuns(robotSpeed, reading);
                Debug.WriteLine("HowTheRouteRuns return: " + inclination);
                ChooseTurn(inclination);
            }
        }
        public async void Stop()//Funkcja zatrzymująca robota
        {
            isDriving = false;
            await _brick.DirectCommand.StopMotorAsync(OutputPort.A, false);
            await _brick.DirectCommand.StopMotorAsync(OutputPort.D, false);
            LeftMotor = 40;
            RightMotor = 40;
        }

        private void ChooseTurn(double inclination)//Robot wybiera kierunek ruchu
        {
            List<Action<double>> decisions = new List<Action<double>>();
            List<double> memberships = new List<double>();

            memberships.Add(ElementaryFunctions.ReadingHardLeft(inclination/10));
            decisions.Add(TurnHard);
            memberships.Add(ElementaryFunctions.ReadingEasyLeft(inclination/10));
            decisions.Add(TurnEasy);
            memberships.Add(ElementaryFunctions.ReadingStraight(inclination/10));
            decisions.Add(GoStraight);
            memberships.Add(ElementaryFunctions.ReadingEasyRight(inclination/10));
            decisions.Add(TurnEasy);
            memberships.Add(ElementaryFunctions.ReadingHardRight(inclination/10));
            decisions.Add(TurnHard);

            int index = memberships.IndexOf(memberships.Max());
            decisions[index](inclination);
        }

        private void TurnHard(double inclination)
        {
            Debug.WriteLine("TurnHard " + inclination);
            _brick.DirectCommand.PlayToneAsync(100, 200, 50);
            if (inclination < 0)//W lewo
            {
                inclination = Math.Abs(inclination);
                LeftMotor = (int)inclination / 4;
                RightMotor = (int)inclination;
            }
            else//W prawo
            {
                RightMotor = (int)inclination / 4;
                LeftMotor = (int)inclination;
            }
        }
        private void TurnEasy(double inclination)
        {
            Debug.WriteLine("TurnEasy " + inclination);
            _brick.DirectCommand.PlayToneAsync(100, 1000, 50);
            if (inclination > 50 || inclination < -50)
                inclination = 50;
            if (inclination < 0)//W lewo
            {
                inclination = Math.Abs(inclination);
                RightMotor = (int)inclination * 2;
                LeftMotor = (int)(inclination * 1.5);
            }
            else//W prawo
            {
                LeftMotor = (int)inclination * 2;
                RightMotor = (int)(inclination * 1.5);
            }
        }
        private void GoStraight(double inclination)
        {
            Debug.WriteLine("GoStraight " + inclination);
            LeftMotor = 100;
            RightMotor = 100;
        }

        public void Connect()
        {
            _brick.ConnectAsync();
            _brick.DirectCommand.SetMotorPolarity(OutputPort.D, Polarity.Backward);
            _brick.DirectCommand.StopMotorAsync(OutputPort.All, false);
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
        }
    }
}
