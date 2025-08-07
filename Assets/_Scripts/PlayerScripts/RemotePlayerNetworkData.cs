using Nakama;

namespace _Scripts.PlayerScripts
{
    public class RemotePlayerNetworkData
    {
        public string MatchId { private set; get; }
        public IUserPresence User { private set; get; }

        public RemotePlayerNetworkData(string matchId, IUserPresence user)
        {
            MatchId = matchId;
            User = user;
        }
    }
}