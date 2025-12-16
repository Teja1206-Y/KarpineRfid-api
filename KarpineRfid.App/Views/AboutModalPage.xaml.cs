namespace KarpineRfid.App.Views;

public partial class AboutModalPage : ContentPage
{
	public AboutModalPage()
	{
		InitializeComponent();
	}

    private async void Close_Clicked(object sender, System.EventArgs e)
    {
        if (Shell.Current?.Navigation?.NavigationStack?.Count > 0)
            await Shell.Current.Navigation.PopAsync();
        else
            await Navigation.PopModalAsync();
    }
}