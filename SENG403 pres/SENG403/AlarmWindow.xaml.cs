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
using System.Windows.Shapes;
using System.Media;
using System.Windows.Threading;

namespace SENG403
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {

        private DispatcherTimer timer;
    
        private int buttonCliked = 0;

        public AlarmWindow()
        {
            InitializeComponent();

          

            timer = new DispatcherTimer(); // TIMER OBJECT FOR DispatcherTimer CLASS
            timer.Interval = TimeSpan.FromSeconds(5); // HOURS, MINUTES, SECONDS
     
          
        }


        // WHEN SNOOZE BUTTON IS PRESSED, ALARMWINDOW IS HIDDEN AND TIMER COUNTDOWN IS STARTED
        public void Snooze_Click(object sender, RoutedEventArgs e)
        {
            buttonCliked = 0;
            this.Hide();
            timer.Tick += timer_Tick;
            timer.Start();
        }

        // AS SOON AS TIMER IS DONE, timer_Tick METHOD IS CALLED AND MAKES ALARM WINDOW REAPPEAR
        private void timer_Tick(object sender, EventArgs e)
        {
            if (buttonCliked == 0)
            {
                this.Show();
            }
            else {
                this.Close();
            }
        }

        // WHEN USER CLICKS DISMISS BUTTON, ALARMWINDOW SCREEN IS CLOSED
        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            buttonCliked = 1;
            this.Close();
        }
    }
}