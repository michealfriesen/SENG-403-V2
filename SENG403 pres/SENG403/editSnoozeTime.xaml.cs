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

        AlarmWindow alarmWindowObject;

        public editSnoozeTime() {

            InitializeComponent();
            alarmWindowObject = new AlarmWindow();
        }
    }
}
