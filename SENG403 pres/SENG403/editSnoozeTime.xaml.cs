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

namespace SENG403
{
    /// <summary>
    /// Interaction logic for editSnoozeTime.xaml
    /// </summary>
    public partial class editSnoozeTime : Window {

        public static string snoozeTime = "5";


        public editSnoozeTime() {

            InitializeComponent();
         }

        private void enterButton_Click(object sender, RoutedEventArgs e){

            snoozeTime = minutesBox.Text;
            this.Close();
            Console.WriteLine("The value entered is: " + snoozeTime);
        }

        private void minutesBox_KeyDown(object sender, KeyEventArgs e){

            if (e.Key == Key.Enter)
            {

                this.Close();
                snoozeTime = minutesBox.Text;
                Console.WriteLine("The value entered is: " + snoozeTime);

            }
        }
    }
}
