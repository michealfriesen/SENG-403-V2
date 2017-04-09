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


namespace SENG403
{
  
    public partial class MainWindow : Window
    {

        
        private String alarmName, alarmDesc, alarmSched, alarmDay;
        private String[] alarmArray = new String[4];
        private LinkedList<alarmObject> alarmLinkedList;
        alarmObject temp_alarm;

        public static SoundPlayer mySound;


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
            System.Windows.Threading.DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick; 
            timer.Start();

            this.KeyUp += MainWindow_KeyUp;

            save.Visibility = Visibility.Hidden;
            cancel.Visibility = Visibility.Hidden;

            analogCheckBox.Checked += AnalogCheckBox_Checked;
            analogCheckBox.Unchecked += AnalogCheckBox_Unchecked;

            // POPULATING chooseSound COMBOBOX
            chooseAlarm.Items.Add("Imperial March");
            chooseAlarm.Items.Add("Samuel Jackson Wake Up!");
            chooseAlarm.Items.Add("Peaceful Alarm");

        }

        private void AnalogCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

            analogGrid.Visibility = Visibility.Hidden;
            digitalGrid.Visibility = Visibility.Visible;


        }

        private void AnalogCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            digitalGrid.Visibility = Visibility.Hidden;
            analogGrid.Visibility = Visibility.Visible;


        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
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

        void timer_Tick(object sender, EventArgs e)
        {
            timeLabel.Content = DateTime.Now.ToLongTimeString();
            int i = 0;
            try
            {
                while (i < alarmLinkedList.Count)
                {

                    if (alarmLinkedList.ElementAt(i).checkAlarm(DateTime.Now.ToString("hh:mm:ss tt")))
                    {
                       
                        alarmLinkedList.ElementAt(i).ringAlarm(this, alarmLinkedList.ElementAt(i));
                        alarmLinkedList.Remove(alarmLinkedList.ElementAt(i));
                        alarmList.Items.RemoveAt(i);

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
            }
            else
            {
                MessageBox.Show("Invalid input");
            }

        }

        private void chooseAlarm_SelectionChanged(object sender, SelectionChangedEventArgs e){

            if (chooseAlarm.SelectedIndex == chooseAlarm.Items.IndexOf("Imperial March")) {

                mySound = new SoundPlayer("darthVader.wav");
            }
            else if (chooseAlarm.SelectedIndex == chooseAlarm.Items.IndexOf("Samuel Jackson Wake Up!")) {

                mySound = new SoundPlayer("samuelJacksonWakeUp.wav");
            }
            else if (chooseAlarm.SelectedIndex == chooseAlarm.Items.IndexOf("Peaceful Alarm")) {

                mySound = new SoundPlayer("peacefulBlessing.wav");
            }
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
                alarmObject newAlarm = new alarmObject { alarmID = alarmName, alarmDescription = alarmDesc, alarmTime = alarmSched, alarmDate = alarmDay };
                this.alarmList.Items.Add(newAlarm);
                // Adds an alarm to the linked list
                try
                {
                    alarmLinkedList.AddLast(newAlarm);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                MessageBox.Show("Alarm Added!");
                clearControls();
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
                mySound.PlayLooping();
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
