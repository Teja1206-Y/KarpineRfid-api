using KarpineRfid.App.Services;
using KarpineRfid.App.ViewModels;
using System.Reflection;


namespace KarpineRfid.App.Views;

[QueryProperty(nameof(SessionId), "sessionId")]
public partial class ExportPage : ContentPage
{
    private readonly ExportViewModel _vm;

    // Constructor injection: MAUI DI will provide ExportViewModel
    public ExportPage(ExportViewModel viewModel)
    {
        InitializeComponent();

        _vm = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        BindingContext = _vm;
    }

    string sessionId = string.Empty;
    public string SessionId
    {
        get => sessionId;
        set
        {
            sessionId = value ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                _ = _vm.InitializeAsync(sessionId);
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!string.IsNullOrWhiteSpace(sessionId))
            await _vm.InitializeAsync(sessionId);
    }
}



