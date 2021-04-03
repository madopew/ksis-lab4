using System.Collections.Generic;
using Server.User;

namespace Server.ServerBase.Interfaces
{
    public interface IServer<T> where T : UserBase
    {
        void Start();
        void Stop();
        void Kick(T user);
        IReadOnlyCollection<T> GetUsers();
    }
}