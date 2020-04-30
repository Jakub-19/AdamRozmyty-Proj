using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System.Diagnostics;


namespace ProjektSSI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Robot robot;
        public MainWindow()
        {
            InitializeComponent();
            buttonCalibrate.IsEnabled = false;
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = false;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            buttonStop.IsEnabled = true;
            buttonStart.IsEnabled = false;
            buttonCalibrate.IsEnabled = false;
            buttonConnect.IsEnabled = false;
            textBlockProgramStatus.Text = "Program działa...";
            robot.Go();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            robot.Stop();
            textBlockProgramStatus.Text = "Program został zatrzymany.";
            buttonStop.IsEnabled = false;
            buttonStart.IsEnabled = true;
            buttonCalibrate.IsEnabled = true;
            buttonConnect.IsEnabled = true;
        }

        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            robot.BlackColor = (uint)sliderBlackNumber.Value;
            robot.WhiteColor = (uint)sliderWhiteNumber.Value;
            ElementaryFunctions.P = robot.WhiteColor - robot.BlackColor;
            buttonStart.IsEnabled = true;
            textBlockCalibrationStatus.Text = "Skalibrowano.";
            textBlockCalibrationStatus.Foreground = Brushes.Green;
        }

        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("COM" + comboBoxNumerCOM.Text);
            robot = new Robot("COM" + comboBoxNumerCOM.Text);
            robot.ConnectionSuccess += ShowConnectionInfo;
            robot.Connect();
        }
        private void ShowConnectionInfo()
        {
                buttonCalibrate.IsEnabled = true;
                textBlockConnectionStatus.Text = "Połączono.";
                textBlockConnectionStatus.Foreground = Brushes.Green;
        }
    }
}
