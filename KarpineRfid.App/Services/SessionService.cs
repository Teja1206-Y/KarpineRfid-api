using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KarpineRfid.App.Models;
using Microsoft.Maui.Storage;

namespace KarpineRfid.App.Services
{
    public class SessionService : ISessionStore
    {
        private readonly string _dataFilePath;
        private readonly object _lock = new();
        private readonly IExportService? _exportService;

        public SessionService(IExportService? exportService = null)
        {
            _exportService = exportService;

            var folder = FileSystem.AppDataDirectory;
            Directory.CreateDirectory(folder);
            _dataFilePath = Path.Combine(folder, "sessions.json");
        }

        private List<Session> LoadAll()
        {
            lock (_lock)
            {
                if (!File.Exists(_dataFilePath)) return new();

                var json = File.ReadAllText(_dataFilePath);
                if (string.IsNullOrWhiteSpace(json)) return new();

                return JsonSerializer.Deserialize<List<Session>>(json) ?? new();
            }
        }

        private Task SaveAllAsync(List<Session> sessions)
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(sessions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_dataFilePath, json);
            }
            return Task.CompletedTask;
        }

        // INTERFACE IMPLEMENTATION

        public Task<IReadOnlyList<Session>> GetAllSessionsAsync()
        {
            var list = LoadAll().OrderByDescending(s => s.CreatedAt).ToList();
            return Task.FromResult((IReadOnlyList<Session>)list);
        }

        public Task<Session?> GetSessionAsync(string sessionId)
        {
            var list = LoadAll();
            var s = list.FirstOrDefault(x => x.SessionId == sessionId);
            return Task.FromResult(s);
        }

        public async Task SaveSessionAsync(Session session)
        {
            var list = LoadAll();

            // If new, add it
            var exists = list.FirstOrDefault(x => x.SessionId == session.SessionId);
            if (exists == null)
            {
                list.Add(session);
            }
            else
            {
                var index = list.IndexOf(exists);
                list[index] = session;
            }

            await SaveAllAsync(list);
        }

        public async Task DeleteSessionAsync(string sessionId)
        {
            var list = LoadAll();
            list.RemoveAll(x => x.SessionId == sessionId.ToString());
            await SaveAllAsync(list);
        }

        public async Task<bool> AddTagAsync(string sessionId, SessionTag tag)
        {
            var list = LoadAll();
            var s = list.FirstOrDefault(x => x.SessionId == sessionId);
            if (s == null) return false;

            // Tag ID generator (store as string)
            int nextTagId = 1;

            if (s.Tags.Count > 0)
            {
                var numericIds = s.Tags
                    .Select(t => int.TryParse(t.Id, out int v) ? v : 0);

                nextTagId = numericIds.Max() + 1;
            }

            tag.Id = nextTagId.ToString();

            s.Tags.Add(tag);

            await SaveAllAsync(list);
            return true;
        }

        public async Task<bool> RemoveTagAsync(string sessionId, string tagId)
        {
            var list = LoadAll();
            var s = list.FirstOrDefault(x => x.SessionId == sessionId);
            if (s == null) return false;

            if (int.TryParse(tagId, out int tid))
            {
                s.Tags.RemoveAll(t => t.Id == tid.ToString());
                await SaveAllAsync(list);
                return true;
            }

            return false;
        }
    }
}
