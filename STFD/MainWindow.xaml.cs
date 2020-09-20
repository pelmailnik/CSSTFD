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
using Microsoft.Win32.TaskScheduler;

namespace STFD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskDefinition td = TaskService.Instance.NewTask();

        public MainWindow()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(mainGrid, boxMinutes);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private DateTime GetTime()
        {
            DateTime date = DateTime.Now;

            date = date.AddHours(GetValueFromBox(boxHours));
            date = date.AddMinutes(GetValueFromBox(boxMinutes));
            date = date.AddSeconds(GetValueFromBox(boxSeconds));

            //boxDebug.Text = String.Format("{0:o}", date);

            return date;
        }

        private double GetValueFromBox(TextBox box)
        {
            if (box.GetLineText(0) == "") return 0.0;
            else return Convert.ToDouble(box.GetLineText(0));
        }

        private void ToPlan(object sender, RoutedEventArgs e)
        {
            

            // Create a new task definition and assign properties
            //TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Description = "Does something";
            td.Principal.LogonType = TaskLogonType.InteractiveToken;

            // Add a trigger that will fire one time
            td.Triggers.Add(new TimeTrigger
            {
                StartBoundary = GetTime()
            });

            // Add an action that will launch Notepad whenever the trigger fires
            td.Actions.Add(new ExecAction("c:\\windows\\system32\\shutdown.exe", "-s", "c:\\windows\\system32"));

            // Register the task in the root folder
            const string taskName = "STFD";
            TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td);

            boxDebug.Text = "Я выключусь";

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            TaskService.Instance.RootFolder.DeleteTask("STFD");
            boxDebug.Text = "Отменено";
        }

        private void ToUp(TextBox box)
        {
            int x = Convert.ToInt32(GetValueFromBox(box));
            if (x == 0) box.Text = "1";
            else box.Text = Convert.ToString(++x);
        }

        private void ToDown(TextBox box)
        {
            int x = Convert.ToInt32(GetValueFromBox(box));
            if (x == 0) box.Text = "0";
            else box.Text = Convert.ToString(--x);
        }

        private void ToUpHours(object sender, RoutedEventArgs e)
        {
            ToUp(boxHours);
        }

        private void ToUpMin(object sender, RoutedEventArgs e)
        {
            ToUp(boxMinutes);
        }

        private void ToUpSec(object sender, RoutedEventArgs e)
        {
            ToUp(boxSeconds);
        }

        private void ToDownHours(object sender, RoutedEventArgs e)
        {
            ToDown(boxHours);
        }

        private void ToDownMin(object sender, RoutedEventArgs e)
        {
            ToDown(boxMinutes);
        }

        private void ToDownSec(object sender, RoutedEventArgs e)
        {
            ToDown(boxSeconds);
        }
    }
}
