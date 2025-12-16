// Services/SessionStore.cs
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using KarpineRfid.App.Models;
using Microsoft.Maui.Storage;

namespace KarpineRfid.App.Services
{
    public class SessionStore : ISessionStore
    {
        // In-memory index + persisted single-file store for simplicity
        private readonly ConcurrentDictionary<string, Session> _sessions = new();
        private readonly string _storeFile;

        public SessionStore()
        {
            _storeFile = Path.Combine(FileSystem.AppDataDirectory, "sessions.json");
            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            try
            {
                if (File.Exists(_storeFile))
                {
                    var json = File.ReadAllText(_storeFile);
                    var list = JsonSerializer.Deserialize<List<Session>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Session>();
                    foreach (var s in list) _sessions[s.SessionId] = s;
                }
            }
            catch
            {
                // ignore errors - start fresh
            }
        }

        private void Persist()
        {
            try
            {
                var list = _sessions.Values;
                var json = JsonSerializer.Serialize(list);
                File.WriteAllText(_storeFile, json);
            }
            catch
            {
                // ignore persistence errors
            }
        }

        public Task<IReadOnlyList<Session>> GetAllSessionsAsync()
        {
            var arr = new List<Session>(_sessions.Values);
            arr.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));
            return Task.FromResult((IReadOnlyList<Session>)arr);
        }

        public Task<Session?> GetSessionAsync(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var s);
            return Task.FromResult(s);
        }

        public Task SaveSessionAsync(Session session)
        {
            _sessions[session.SessionId] = session;
            Persist();
            return Task.CompletedTask;
        }

        public Task DeleteSessionAsync(string sessionId)
        {
            _sessions.TryRemove(sessionId, out _);
            Persist();
            return Task.CompletedTask;
        }
    }
}

