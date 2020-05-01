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
            buttonCalibrateLeft.IsEnabled = false;
            buttonCalibrateRight.IsEnabled = false;
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = false;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            buttonStop.IsEnabled = true;
            buttonStart.IsEnabled = false;
            buttonCalibrateLeft.IsEnabled = false;
            buttonCalibrateRight.IsEnabled = false;
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
            buttonCalibrateLeft.IsEnabled = true;
            buttonCalibrateRight.IsEnabled = true;
            buttonConnect.IsEnabled = true;
        }

        private void buttonCalibrateLeft_Click(object sender, RoutedEventArgs e)
        {
            ElementaryFunctions.BlackColorLeft = uint.Parse(textBoxBlackNumberLeft.Text);
            ElementaryFunctions.WhiteColorLeft = uint.Parse(textBoxWhiteNumberLeft.Text);
            ElementaryFunctions.P_L = 100 / (ElementaryFunctions.WhiteColorLeft - ElementaryFunctions.BlackColorLeft);
            textBlockCalibrationStatusLeft.Text = "Skalibrowano.";
            textBlockCalibrationStatusLeft.Foreground = Brushes.Green;
            buttonCalibrateRight.IsEnabled = true;
        }

        private void buttonCalibrateRight_Click(object sender, RoutedEventArgs e)
        {
            ElementaryFunctions.BlackColorRight = uint.Parse(textBoxBlackNumberRight.Text);
            ElementaryFunctions.WhiteColorRight = uint.Parse(textBoxWhiteNumberRight.Text);
            ElementaryFunctions.P_R = 100 / (ElementaryFunctions.WhiteColorRight - ElementaryFunctions.BlackColorRight);
            buttonStart.IsEnabled = true;
            textBlockCalibrationStatusRight.Text = "Skalibrowano.";
            textBlockCalibrationStatusRight.Foreground = Brushes.Green;
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
                buttonCalibrateLeft.IsEnabled = true;
                textBlockConnectionStatus.Text = "Połączono.";
                textBlockConnectionStatus.Foreground = Brushes.Green;
        }

        
    }
}
