namespace Obstgarten.Game
{
    public interface IGameResult<T>
    {
        int TurnsTaken { get; }
        int RavenPartsLaid { get; }
        bool PlayersWon {get;}
    }
}
