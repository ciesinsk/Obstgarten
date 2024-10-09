using Obstgarten.Game;

namespace Obstgarten.Statistics
{
    public class ResultRecord<T> : IGameResult<T>
    {
        public ResultRecord(IGameResult<T> other) 
        { 
            TurnsTaken = other.TurnsTaken;
            RavenPartsLaid = other.RavenPartsLaid;
            PlayersWon = other.PlayersWon;
        }

        public int TurnsTaken {get; private set;}

        public int RavenPartsLaid {get; private set;}

        public bool PlayersWon {get; private set;}
    }
}
