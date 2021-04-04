namespace Client.ViewModels
{
    public class LoginViewModel
    {
        private object sender;
        
        public ServerConnectionHandler Server { get; set; }

        public LoginViewModel(ServerConnectionHandler server, object sender)
        {
            Server = server;
            this.sender = sender;
        }

        public void SetDataContext(object context)
        {
            ((MainWindow) sender).DataContext = context;
        }
    }
}