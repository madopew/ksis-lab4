using Server.User;

namespace Server.ServerBase.Interfaces
{
    public interface IServer<in T> where T : UserBase
    {
        void Start();
        void Stop();
        void Kick(T user);
    }
}