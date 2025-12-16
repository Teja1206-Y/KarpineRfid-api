using KarpineRfid.App.ViewModels;

namespace KarpineRfid.App.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel vm;

    // Constructor injection (preferred). Make sure you've registered SettingsViewModel in MauiProgram.
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();

        vm = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // ensure current theme selection sync
        vm.SyncSelectedThemeWithApp();
    }
}