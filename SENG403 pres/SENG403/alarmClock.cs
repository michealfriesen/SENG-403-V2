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
using System.Windows.Threading;
using Microsoft.Win32;
using System.Media;

namespace SENG403
{
    class alarmObject
    {
        // Instance variables
        public string alarmID;
        public string alarmDescription;
        public string alarmTime;
        public string alarmDate;

        // Constructor for an alarmObject
        public alarmObject(string a, string b, string c, string d)
        {
            alarmID = a;
            alarmDescription = b;
            alarmTime = c;
            alarmDate = d;
        }
    }
}
