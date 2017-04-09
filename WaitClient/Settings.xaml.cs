using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;

namespace Waid
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            var userSettings = Properties.Settings.Default;

            Sunday.IsChecked = userSettings.Sunday;
            Monday.IsChecked = userSettings.Monday;
            Tuesday.IsChecked = userSettings.Tues;
            Wednesday.IsChecked = userSettings.Weds;
            Thursday.IsChecked = userSettings.Thurs;
            Friday.IsChecked = userSettings.Fri;
            Saturday.IsChecked = userSettings.Sat;

            Start.Value = userSettings.StartTime;
            StartText.Text = userSettings.StartTime.ToString(CultureInfo.CurrentUICulture);

            End.Value = userSettings.EndTime;
            EndText.Text = userSettings.EndTime.ToString(CultureInfo.CurrentUICulture);

            UserId.Text = userSettings.UserId;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rk != null)
            {
                object value = rk.GetValue("Waid");
                RunAtStart.IsChecked = value != null;
            }

            Start.ValueChanged += Start_ValueChanged;
            End.ValueChanged += End_ValueChanged;
        }

        void Start_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StartText.Text = Start.Value.ToString(CultureInfo.CurrentUICulture);
        }

        void End_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EndText.Text = End.Value.ToString(CultureInfo.CurrentUICulture);
        }

        protected override void OnClosed(EventArgs e)
        {
            Commands.ShowSettings.CloseSettings();

            base.OnClosed(e);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var userSettings = Properties.Settings.Default;

            // ReSharper disable PossibleInvalidOperationException
            userSettings.SetDays(Sunday.IsChecked.Value,
                                 Monday.IsChecked.Value,
                                 Tuesday.IsChecked.Value,
                                 Wednesday.IsChecked.Value,
                                 Thursday.IsChecked.Value,
                                 Friday.IsChecked.Value,
                                 Saturday.IsChecked.Value);
            

            userSettings.StartTime = (int) Start.Value;
            userSettings.EndTime = (int) End.Value;

            userSettings.UserId = UserId.Text;

            userSettings.Save();

            

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rk != null)
            {
                if (RunAtStart.IsChecked.Value)
                    rk.SetValue("Waid", Assembly.GetExecutingAssembly().Location);
                else
                    rk.DeleteValue("Waid", false);                
            }
            // ReSharper restore PossibleInvalidOperationException
            


            Close();
        }
    }
}
