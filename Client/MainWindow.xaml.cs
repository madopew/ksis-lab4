using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.ViewModels;
using MaterialDesignThemes.Wpf;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ServerConnectionHandler Server { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            Server = new ServerConnectionHandler(Dispatcher);
            DataContext = new LoginViewModel(Server, this);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Server.Send("die", null);
        }
    }
}