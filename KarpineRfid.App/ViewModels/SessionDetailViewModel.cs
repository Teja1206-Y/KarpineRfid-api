// ViewModels/SessionDetailViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KarpineRfid.App.Models;
using KarpineRfid.App.Services;

namespace KarpineRfid.App.ViewModels
{
    public partial class SessionDetailViewModel : ObservableObject
    {
        private readonly ISessionStore _store;
        private readonly IExportService _export;

        [ObservableProperty] public Session? session;
        public ObservableCollection<SessionTag> Tags { get; } = new();

        public SessionDetailViewModel(ISessionStore store, IExportService export)
        {
            _store = store;
            _export = export;
        }

        public async Task InitializeAsync(string sessionId)
        {
            Session = await _store.GetSessionAsync(sessionId);
            Tags.Clear();
            if (Session?.Tags != null)
            {
                foreach (var t in Session.Tags) Tags.Add(t);
            }
        }

        [RelayCommand]
        public async Task ExportAsync()
        {
            if (Session == null) return;
            // Navigate to export page with sessionId param
            await Shell.Current.GoToAsync($"export?sessionId={Uri.EscapeDataString(Session.SessionId)}");
        }
    }
}

