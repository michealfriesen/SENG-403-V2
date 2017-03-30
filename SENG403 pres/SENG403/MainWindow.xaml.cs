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
using System.Media;
using System.IO;

namespace SENG403
{
  
    public partial class MainWindow : Window
    {

        
        private String alarmName, alarmDesc, alarmSched, alarmDay;
        private String[] alarmArray = new String[4];
        private LinkedList<alarmObject> alarmLinkedList;
        alarmObject temp_alarm;


        public MainWindow()
        {
            InitializeComponent();
            InitializeAddAlarmGrid();
            InitializeTimerGrid();

            alarmLinkedList = new LinkedList<alarmObject>();

            var gridView = new GridView();
            this.alarmList.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Alarm Name",
                DisplayMemberBinding = new Binding("alarmID")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Alarm Descripition",
                DisplayMemberBinding = new Binding("alarmDescription")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Time",
                DisplayMemberBinding = new Binding("alarmTime")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Date",
                DisplayMemberBinding = new Binding("alarmDate")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Repeat Daily",
                DisplayMemberBinding = new Binding("repeatDaily")
            });

            readTextToList();

            System.Windows.Threading.DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += timer_Tick; 
            timer.Start();

            this.KeyUp += MainWindow_KeyUp;

            save.Visibility = Visibility.Hidden;
            cancel.Visibility = Visibility.Hidden;

            analogCheckBox.Checked += AnalogCheckBox_Checked;
            analogCheckBox.Unchecked += AnalogCheckBox_Unchecked;
        }

        private void AnalogCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

            analogGrid.Visibility = Visibility.Hidden;
            digitalGrid.Visibility = Visibility.Visible;


        }

        private String path = Directory.GetCurrentDirectory();


        private void AnalogCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            digitalGrid.Visibility = Visibility.Hidden;
            analogGrid.Visibility = Visibility.Visible;


        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                saveAlarmToText(alarmLinkedList);
                Application.Current.Shutdown();
            }
            
        }

        private void InitializeAddAlarmGrid() {
            for (int i = 1; i <= 12; i++)
            {
                if (i < 10)
                {
                    hourComboBox.Items.Add("0" + i);
                }
                else
                {
                    hourComboBox.Items.Add(i);
                }
            }

            for (int i = 0; i <= 59; i++)
            {
                if (i < 10) {
                    minuteComboBox.Items.Add("0" + i);
                }
                else
                {
                    minuteComboBox.Items.Add(i);
                }
                
            }

           
            amPMComboBox.Items.Add("AM");
            amPMComboBox.Items.Add("PM");
        }

        private void InitializeTimerGrid()
        {
            DateTime time = DateTime.Now;
            timeLabel.Content = time.ToString("h:mm:ss tt");
            
        }

        private void InitializeAlarmListGrid() {
         
            //keep to avoid error
        }

        // The method used when the timer is ticking.
        void timer_Tick(object sender, EventArgs e)
        {
            timeLabel.Content = DateTime.Now.ToLongTimeString();
            int i = 0;
            try
            {
                // The loop that checks the list every second for an alarm that is equal to the current time
                // If the alarm exists, ring the alarm and potentially remove it from the list.
                while (i < alarmLinkedList.Count)
                {

                    if (alarmLinkedList.ElementAt(i).checkAlarm(DateTime.Now.ToString("hh:mm:ss tt")) && alarmLinkedList.ElementAt(i).checkDate(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        alarmLinkedList.ElementAt(i).ringAlarm(this, alarmLinkedList.ElementAt(i));
                        if (alarmLinkedList.ElementAt(i).repeatDaily == false)
                        {
                            alarmLinkedList.Remove(alarmLinkedList.ElementAt(i));
                            alarmList.Items.RemoveAt(i);
                        }

                    }
                    else
                    {
                        Console.WriteLine(alarmLinkedList.ElementAt(i).alarmTime);
                        Console.WriteLine(DateTime.Now.ToString("hh:mm:ss tt"));
                    }
                    i++;
                }

                double milsec = DateTime.Now.Millisecond;
                double sec = DateTime.Now.Second;
                double min = DateTime.Now.Minute;
                double hr = DateTime.Now.Hour;
                seconds.LayoutTransform = new RotateTransform(((sec / 60) * 360) + ((milsec / 1000) * 6));
                minutes.LayoutTransform = new RotateTransform((min * 360 / 60) + ((sec / 60) * 6));
                hours.LayoutTransform = new RotateTransform((hr * 360 / 12) + (min / 2));
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine("Current Time: " + DateTime.Now.ToString("hh:mm tt"));
            }
        }



        private void amPMComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //keep to avoid error
        }

     

        private void alarmList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //keep to avoid error
        }


        private void RepeatAlarm_Click(object sender, RoutedEventArgs e)
        {
            if (alarmList.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                try
                {
                    if (alarmLinkedList.ElementAt(alarmList.SelectedIndex).repeatDaily == true)
                    {
                        alarmLinkedList.ElementAt(alarmList.SelectedIndex).repeatDaily = false;
                    }
                    else
                    {
                        alarmLinkedList.ElementAt(alarmList.SelectedIndex).repeatDaily = true;
                    }
                    alarmList.Items.Clear();
                    // Repopulate the list
                    for (int i = 0; i <= alarmLinkedList.Count; i++)
                    {
                        alarmList.Items.Add(alarmLinkedList.ElementAt(i));
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        private void saveAlarmToText(LinkedList<alarmObject> listOfAlarms)
        {

            string lines = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\try.csv");
            int i = 0;
            while (i < alarmLinkedList.Count)
            {
                string alarmID = alarmLinkedList.ElementAt(i).alarmID;
                string alarmDesc = alarmLinkedList.ElementAt(i).alarmDescription;
                string alarmTime = alarmLinkedList.ElementAt(i).alarmTime;
                string alarmDate = alarmLinkedList.ElementAt(i).alarmDate;
                string alarmRepeat = alarmLinkedList.ElementAt(i).repeatDaily.ToString();
                lines = alarmID + "," + alarmDesc + "," + alarmTime + "," + alarmDate + "," + alarmRepeat;

                file.WriteLine(lines);
                lines = "";
                i++;
            }

            file.Close();

        }

        private void readTextToList()
        {


            string[] lines = System.IO.File.ReadAllLines(path + "\\try.csv");


            foreach (string line in lines)
            {
                string[] splitString = line.Split(',');
                Boolean repeat = false;

                if (splitString[4].ToUpper() == "TRUE")
                {
                    repeat = true;
                }

                alarmObject newAlarm = new alarmObject { alarmID = splitString[0], alarmDescription = splitString[1], alarmTime = splitString[2], alarmDate = splitString[3], repeatDaily = repeat };
                this.alarmList.Items.Add(newAlarm);
                // Adds an alarm to the linked list
                try
                {
                    alarmLinkedList.AddLast(newAlarm);
                    saveAlarmToText(alarmLinkedList);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        private void EditAlarm_Click(object sender, RoutedEventArgs e)
        {
            if(alarmList.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                try
                {
                    temp_alarm = alarmLinkedList.ElementAt(alarmList.SelectedIndex);
                    alarmNameTextBox.Text = temp_alarm.alarmID;
                    alarmDescriptionTextBox.Text = temp_alarm.alarmDescription;
                    dateOfAlarm.Text = temp_alarm.alarmDate;

                    addAlarmGrid.Visibility = Visibility.Visible;
                    showAlarmGrid.Visibility = Visibility.Hidden;

                    save.Visibility = Visibility.Visible;
                    cancel.Visibility = Visibility.Visible;
                    addButton.Visibility = Visibility.Hidden;

                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        // WHEN USER CLICKS ON Edit Snooze Time NEW WINDOW POPS UP ALLOWING USER TO EDIT SNOOZE TIME
        private void EditSnooze_Click(object sender, RoutedEventArgs e)
        {

            var editSnooze = new editSnoozeTime();
            editSnooze.Show();

        }

        private void DeleteAlarm_Click(object sender, RoutedEventArgs e)
        {
            if(alarmList.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                try
                {
                var alarm = alarmLinkedList.ElementAt(alarmList.SelectedIndex);
                this.alarmList.Items.Remove(alarm);
                alarmLinkedList.Remove(alarm);
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            save.Visibility = Visibility.Hidden;
            cancel.Visibility = Visibility.Hidden;
            addButton.Visibility = Visibility.Visible;

            addAlarmGrid.Visibility = Visibility.Hidden;
            showAlarmGrid.Visibility = Visibility.Visible;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            alarmName = alarmNameTextBox.Text;
            alarmDesc = alarmDescriptionTextBox.Text;
            alarmSched = hourComboBox.Text + ":" + minuteComboBox.Text + " " + amPMComboBox.Text;
            alarmDay = dateOfAlarm.Text;

            if (alarmName.Length > 0 && alarmDesc.Length > 0 && alarmSched.Length > 0 && alarmDay.Length > 0)
            {

                alarmObject newAlarm = new alarmObject { alarmID = alarmName, alarmDescription = alarmDesc, alarmTime = alarmSched, alarmDate = alarmDay };

                try
                {
                    this.alarmList.Items.Remove(temp_alarm);
                    this.alarmList.Items.Add(newAlarm);
                    alarmLinkedList.Remove(temp_alarm);
                    alarmLinkedList.AddLast(newAlarm);
                    temp_alarm = null;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                MessageBox.Show("Alarm Saved!");
                clearControls();
                save.Visibility = Visibility.Hidden;
                cancel.Visibility = Visibility.Hidden;
                addButton.Visibility = Visibility.Visible;

                addAlarmGrid.Visibility = Visibility.Hidden;
                showAlarmGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Invalid input");
            }

        }


        // Method for showing the alarm grid display
        private void showAlarmGrid_Click(object sender, RoutedEventArgs e)
        {
            addAlarmGrid.Visibility = Visibility.Visible;
            showAlarmGrid.Visibility = Visibility.Hidden;
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            alarmName = alarmNameTextBox.Text;
            alarmDesc = alarmDescriptionTextBox.Text;
            alarmSched = hourComboBox.Text + ":" + minuteComboBox.Text + " " + amPMComboBox.Text;
            alarmDay = dateOfAlarm.Text;

            if (alarmName.Length > 0 && alarmDesc.Length > 0 && alarmSched.Length > 0 && alarmDay.Length > 0)
            {
                // Adds an alarm to the list displayer
                alarmObject newAlarm = new alarmObject { alarmID = alarmName, alarmDescription = alarmDesc, alarmTime = alarmSched, alarmDate = alarmDay, repeatDaily = false};
                this.alarmList.Items.Add(newAlarm);
                // Adds an alarm to the linked list
                try
                {
                    alarmLinkedList.AddLast(newAlarm);
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                saveAlarmToText(alarmLinkedList);

                MessageBox.Show("Alarm Added!");
                clearControls();
                addAlarmGrid.Visibility = Visibility.Hidden;
                showAlarmGrid.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("Invalid input");
            }
           
        }
        // Object representing the alarm object.
        private class alarmObject{
            // Instance variables
            public string alarmID { get; set; }
            public string alarmDescription { get; set; }
            public string alarmTime { get; set; }
            public string alarmDate { get; set; }
            public Boolean repeatDaily { get; set; }
            // TODO: add date checker
            public Boolean checkAlarm(string time)
            {
                string[] splitString = this.alarmTime.Split(' ');
                string comparer = splitString[0] + ":00 " + splitString[1];
                if ( comparer == time)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public Boolean checkDate(string date)
            {
                if (date == this.alarmDate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Method for ringing alarm that will make a new alarm window which rings and has the option
            // To dismiss and to snooze
            // w - the window that calls the ringAlarm function
            // a - the alarmFunction associated with the ringing
            public void ringAlarm(Window w, alarmObject a)
            {
                // Create a new AlarmWindow that is 
                var alarmWindow = new AlarmWindow();
                // Get the proper title and description
                alarmWindow.title.Text = a.alarmID;
                alarmWindow.description.Text = a.alarmDescription;
                alarmWindow.Show();
                
                // Play a simple ringtone sound
               
                // Open the window that called the ringAlarm function
            }
        }

        private void clearControls()
        {
            alarmNameTextBox.Text = "";
            alarmDescriptionTextBox.Text = "";
            hourComboBox.SelectedIndex = -1;
            minuteComboBox.SelectedIndex = -1;
            amPMComboBox.SelectedIndex = -1;
            dateOfAlarm.SelectedDate = DateTime.Now;
        

        }


    }
}
