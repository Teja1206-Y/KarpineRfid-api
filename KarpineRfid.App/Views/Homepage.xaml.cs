namespace KarpineRfid.App.Views;

public partial class Homepage : ContentPage
{
	public Homepage()
	{
		InitializeComponent();
	}
    private async void OpenSessions_Clicked(object sender, EventArgs e)
    {
        try
        {
            // navigate to the named route defined in AppShell
            await Shell.Current.GoToAsync("sessions");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Navigation error OpenSessions: " + ex.Message);
        }
    }

    private async void OpenSettings_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("settings");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Navigation error OpenSettings: " + ex.Message);
        }
    }

    private async void OpenAbout_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("about");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Navigation error OpenAbout: " + ex.Message);
        }
    }

    private async void OpenSessionDetail_Clicked(object sender, EventArgs e)
    {
        try
        {
            // example: navigate to sessiondetail with a sessionId query
            await Shell.Current.GoToAsync($"sessiondetail?sessionId={Uri.EscapeDataString("1")}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Navigation error OpenSessionDetail: " + ex.Message);
        }
    }

    private async void OpenExport_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync($"export?sessionId={Uri.EscapeDataString("1")}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Navigation error OpenExport: " + ex.Message);
        }
    }

}