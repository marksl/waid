using System;
using System.Windows;

namespace Waid
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            MyNotifyIcon.Dispose();

            base.OnClosing(e);
        }
    }
}
