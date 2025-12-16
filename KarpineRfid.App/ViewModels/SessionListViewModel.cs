// ViewModels/SessionListViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KarpineRfid.App.Models;
using KarpineRfid.App.Services;
using Microsoft.Maui.ApplicationModel;

namespace KarpineRfid.App.ViewModels
{
    public partial class SessionListViewModel : ObservableObject
    {
        private readonly ISessionStore _store;

        public ObservableCollection<Session> Sessions { get; } = new();

        [ObservableProperty] bool isBusy;

        public SessionListViewModel(ISessionStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Sessions.Clear();
                var list = await _store.GetAllSessionsAsync();
                foreach (var s in list) Sessions.Add(s);
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task DeleteSessionAsync(Session session)
        {
            if (session == null) return;
            await _store.DeleteSessionAsync(session.SessionId);
            await LoadAsync();
        }

        [RelayCommand]
        public async Task OpenSessionAsync(Session session)
        {
            if (session == null) return;
            // Shell route: SessionDetailPage expects parameter sessionId
            await Shell.Current.GoToAsync($"sessiondetail?sessionId={Uri.EscapeDataString(session.SessionId)}");
        }
    }
}

