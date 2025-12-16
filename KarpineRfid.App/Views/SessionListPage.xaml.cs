using KarpineRfid.App.Models;
using KarpineRfid.App.ViewModels;

namespace KarpineRfid.App.Views;

public partial class SessionListPage : ContentPage
{
    private readonly SessionListViewModel vm;

    public SessionListPage()
    {
        InitializeComponent();

        // resolve VM from DI (ensure you've registered it in MauiProgram)
        vm = App.Services.GetService<SessionListViewModel>()
             ?? throw new InvalidOperationException("SessionListViewModel not registered in DI.");

        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.LoadAsync();
    }

    // exact signature required by XAML
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as Session;
        if (selected == null) return;

        if (sender is CollectionView cv)
            cv.SelectedItem = null;

        // Navigate to SessionDetailPage via Shell route (registered as "sessiondetail")
        var id = Uri.EscapeDataString(selected.SessionId ?? string.Empty);
        await Shell.Current.GoToAsync($"sessiondetail?sessionId={id}");
    }

    private async void Open_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Session session)
        {
            var id = Uri.EscapeDataString(session.SessionId ?? string.Empty);
            await Shell.Current.GoToAsync($"sessiondetail?sessionId={id}");
        }
    }
}
