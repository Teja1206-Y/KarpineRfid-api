// ViewModels/ExportViewModel.cs
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KarpineRfid.App.Services;
using Microsoft.Maui.ApplicationModel;
using KarpineRfid.App.Models;
using System.Collections.ObjectModel;

namespace KarpineRfid.App.ViewModels
{
    public partial class ExportViewModel : ObservableObject
    {
        private readonly ISessionStore _store;
        private readonly IExportService _exportService;

        public Session Session { get; private set; } = new Session();
        public ObservableCollection<string> AvailableFormats { get; } = new() { "CSV", "JSON", "Both" };

        [ObservableProperty] string selectedFormat = "CSV";

        [ObservableProperty] bool includeFieldSessionId = true;
        [ObservableProperty] bool includeFieldTitle = true;
        [ObservableProperty] bool includeFieldCreatedAt = true;
        [ObservableProperty] bool includeFieldNotes = true;
        [ObservableProperty] bool includeFieldTags = true;

        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand ShareCommand { get; }

        public ExportViewModel(ISessionStore store, IExportService exportService)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));

            SaveCommand = new AsyncRelayCommand(ExecuteSaveAsync);
            ShareCommand = new AsyncRelayCommand(ExecuteShareAsync);
        }

        public async Task InitializeAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) return;

            var s = await _store.GetSessionAsync(sessionId);
            if (s == null) return;

            Session = s;
            OnPropertyChanged(nameof(Session));
        }

        private async Task ExecuteSaveAsync()
        {
            try
            {
                // quick debug to confirm command executed
                await Application.Current.MainPage.DisplayAlert("Debug", "SaveCommand fired", "OK");

                var formats = GetRequestedFormats();
                var fields = GetSelectedFields();

                var paths = await _exportService.ExportAsync(Session, formats, fields);

                await Application.Current.MainPage.DisplayAlert("Saved", $"Exported files:\n{string.Join("\n", paths)}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ExecuteShareAsync()
        {
            try
            {
                // quick debug to confirm command executed
                await Application.Current.MainPage.DisplayAlert("Debug", "ShareCommand fired", "OK");

                var formats = GetRequestedFormats();
                var fields = GetSelectedFields();

                var paths = await _exportService.ExportAsync(Session, formats, fields);

                // no share sheet now? if you still want share, you can keep Share.RequestAsync
                // For your requirement we save locally — here we show paths only
                await Application.Current.MainPage.DisplayAlert("Saved (share)", $"Files:\n{string.Join("\n", paths)}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private string[] GetRequestedFormats()
        {
            if (SelectedFormat == "Both") return new[] { "CSV", "JSON" };
            return new[] { SelectedFormat ?? "CSV" };
        }

        private string[] GetSelectedFields()
        {
            var list = new System.Collections.Generic.List<string>();
            if (IncludeFieldSessionId) list.Add("sessionId");
            if (IncludeFieldTitle) list.Add("title");
            if (IncludeFieldCreatedAt) list.Add("createdAt");
            if (IncludeFieldNotes) list.Add("notes");
            if (IncludeFieldTags) list.Add("tags");
            return list.ToArray();
        }
    }
}

