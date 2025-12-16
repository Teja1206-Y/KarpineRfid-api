using KarpineRfid.App.Data;
using KarpineRfid.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpineRfid.App.Services
{
    public class InMemorySessionStore : ISessionStore
    {
        private readonly List<Session> _items;

        public InMemorySessionStore()
        {
            _items = DummySessions.All.Select(s => Clone(s)).ToList();
        }

        public Task<IReadOnlyList<Session>> GetAllSessionsAsync()
            => Task.FromResult<IReadOnlyList<Session>>(_items.Select(Clone).ToList());

        public Task<Session?> GetSessionAsync(string sessionId)
        {
            var found = _items.FirstOrDefault(s => s.SessionId == sessionId);
            return Task.FromResult(found is null ? null : Clone(found));
        }

        public Task SaveSessionAsync(Session session)
        {
            var idx = _items.FindIndex(s => s.SessionId == session.SessionId);
            if (idx >= 0)
                _items[idx] = Clone(session);
            else
                _items.Add(Clone(session));

            return Task.CompletedTask;
        }

        public Task DeleteSessionAsync(string sessionId)
        {
            _items.RemoveAll(s => s.SessionId == sessionId);
            return Task.CompletedTask;
        }

        private static Session Clone(Session s) =>
            new Session
            {
                SessionId = s.SessionId,
                Title = s.Title,
                CreatedAt = s.CreatedAt,
                Notes = s.Notes,
                Tags = s.Tags?.Select(t => new SessionTag
                {
                    Id = t.Id,
                    Rssi = t.Rssi,
                    FirstSeen = t.FirstSeen,
                    LastSeen = t.LastSeen,
                    ReadCount = t.ReadCount,
                    Note = t.Note
                }).ToList() ?? new List<SessionTag>()
            };
    }
}
