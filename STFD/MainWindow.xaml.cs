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

namespace STFD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ShedulerTask _task = new ShedulerTask();

        public MainWindow()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(mainGrid, boxMinutes);
            Indicate();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Minimized_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnPlan_Click(object sender, RoutedEventArgs e)
        {
            
            _task.SetTime(boxHours.Text, boxMinutes.Text, boxSeconds.Text);
            _task.ToPlan(cmbBox.SelectedIndex);
            Indicate();

            boxDebug.Text = Convert.ToString(_task.IsTaskExist());

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
            _task.DeleteTask();
            Indicate();
            boxDebug.Text = Convert.ToString(_task.IsTaskExist());
        }

        private void Indicate()
        {
            if (_task.IsTaskExist())
            {
                if (_task.GetTaskTime() < DateTime.Now)
                {
                    ChangeImgIndicator();
                    lblTitleStatus.Content = "STFD";
                }
                else
                {
                    ChangeImgIndicator(cmbBox.SelectedIndex);
                    if (_task.GetTaskTime().Day == DateTime.Now.Day)
                    {
                        lblTitleStatus.Content = $"Сегодня в {_task.GetTaskTime():t}";
                    }
                    else if (_task.GetTaskTime().Day == DateTime.Now.Day + 1)
                    {
                        lblTitleStatus.Content = $"Завтра в {_task.GetTaskTime():t}";
                    }
                    else
                    {
                        lblTitleStatus.Content = $"{_task.GetTaskTime():M}, {_task.GetTaskTime():t}";
                    }
                }
            }
            else
            {
                ChangeImgIndicator();
                lblTitleStatus.Content = "STFD";
            }
        }


        private void ChangeImgIndicator()
        {
            indctrImage.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/indicatorOff.png"));
        }

        private void ChangeImgIndicator(int indexCmbBox)
        {
            switch (indexCmbBox)
            {
                case 0: 
                    indctrImage.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/indicatorShut.png"));
                    break;
                case 1:
                    indctrImage.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/indicatorSleep.png"));
                    break;
            }
        }

        private static double ConvertValue(TextBox box)
        {
            return box.Text == "" ? 0.0 : Convert.ToDouble(box.Text);
        }


        private void ToUp(TextBox box)
        {
            var x = Convert.ToInt32(ConvertValue(box));
            box.Text = x == 0 ? "1" : Convert.ToString(++x);
        }

        private void ToDown(TextBox box)
        {
            var x = Convert.ToInt32(ConvertValue(box));
            box.Text = x == 0 ? "0" : Convert.ToString(--x);
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
