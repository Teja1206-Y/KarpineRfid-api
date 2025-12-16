// Services/ISessionStore.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using KarpineRfid.App.Models;

namespace KarpineRfid.App.Services
{
    public interface ISessionStore
    {
        Task<IReadOnlyList<Session>> GetAllSessionsAsync();
        Task<Session?> GetSessionAsync(string sessionId);
        Task SaveSessionAsync(Session session);
        Task DeleteSessionAsync(string sessionId);
    }
}

