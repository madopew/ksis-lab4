using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;
using MaterialDesignThemes.Wpf;

namespace Client.Views
{
    public partial class LoginView : UserControl
    {
        public ServerConnectionHandler Server { get; set; }
        public LoginView()
        {
            InitializeComponent();
            Loaded += (sender, args) => Server = ((LoginViewModel) DataContext).Server;
        }
        
        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var connection = Server.Connect(IPAddress.Parse(IpTextBox.Text), Convert.ToInt32(PortTextBox.Text));
                ShowWaiting();
                await connection;
            }
            catch (Exception exception)
            {
                CloseDialog();
                ShowMessage(exception.Message);
                return;
            }
            
            Server.Start();
            Server.Send($"connect/{UsernameTextBox.Text}", s =>
            {
                Debug.WriteLine(s);
                if (s.Equals("success"))
                {
                    CloseDialog();
                    ((LoginViewModel) DataContext).SetDataContext(new MenuViewModel());
                }
                else
                {
                    var inputs = s.Split('/');
                    CloseDialog();
                    ShowMessage(inputs[1]);
                }
            });
        }

        private void ShowWaiting()
        {
            DialogTextBlock.Visibility = Visibility.Collapsed;
            DialogCloseButton.Visibility = Visibility.Collapsed;
            DialogProgressBar.Visibility = Visibility.Visible;
            DialogHost.OpenDialogCommand.Execute(null, ConnectButton);
        }

        private void ShowMessage(string message)
        {
            DialogTextBlock.Visibility = Visibility.Visible;
            DialogCloseButton.Visibility = Visibility.Visible;
            DialogProgressBar.Visibility = Visibility.Collapsed;
            DialogTextBlock.Text = message;
            DialogHost.OpenDialogCommand.Execute(null, ConnectButton);
        }

        private void CloseDialog()
        {
            DialogHost.CloseDialogCommand.Execute(null, ConnectButton);
        }
    }
}