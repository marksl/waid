using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Waid
{
    public class Commands
    {
        public static ExitCommand Exit = new ExitCommand();
        public static ShowSettingsCommand ShowSettings = new ShowSettingsCommand();
        public static WhatHaveIDone ShowMeTheMoney = new WhatHaveIDone();
    }

    public class WhatHaveIDone : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var startInfo = new ProcessStartInfo("explorer.exe", "http://waidmonitor.com");
            Process.Start(startInfo);
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

    }

    public class ShowSettingsCommand : ICommand
    {
        private bool _opened;

        public bool CanExecute(object parameter)
        {
            return _opened == false;
        }

        public void CloseSettings()
        {
            _opened = false;
        }

        public void Execute(object parameter)
        {
            var settings = new Settings();
            settings.Show();
            _opened = true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }

    public class ExitCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }

}