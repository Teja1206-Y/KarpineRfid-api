namespace KarpineRfid.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("homepage", typeof(Views.Homepage));

            Routing.RegisterRoute("sessiondetail", typeof(Views.SessionDetailPage));
            Routing.RegisterRoute("export", typeof(Views.ExportPage));
            // optional: register pages if you will navigate by route name
            Routing.RegisterRoute("sessions", typeof(Views.SessionListPage));
            Routing.RegisterRoute("settings", typeof(Views.SettingsPage));
            Routing.RegisterRoute("about", typeof(Views.AboutModalPage));

        }
    }
}

