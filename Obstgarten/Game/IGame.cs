namespace Obstgarten.Game
{
    public interface IGame<T>
        where T : Enum
    {
        /// <summary>
        /// The number of fruits lft on the tree for each fruit type
        /// </summary>
        public IDictionary<T, int> FruitsLeft { get; }

        /// <summary>
        /// Specifies the number of fruits per fruit type initially on the tree
        /// </summary>
        int NumberOfFruitsPerFruitType { get; }

        /// <summary>
        /// Specifies how many tiles make the Raven complete
        /// </summary>
        int NumberOfRavenParts { get; }

        /// <summary>
        /// What colors on the dice represent the Raven
        /// </summary>
        IEnumerable<T> RavenColors { get; }

        /// <summary>
        /// What colors on the dice represent the basket (the Joker)
        /// </summary>
        IEnumerable<T> JokerColors { get; }

        /// <summary>
        /// Has the game ended?
        /// </summary>
        /// <returns></returns>
        public bool HasGameEnded();

        /// <summary>
        /// Initializes the fruits in the tree
        /// </summary>
        void InitFruitTree();

        /// <summary>
        /// Did the players win the game?
        /// </summary>
        /// <returns></returns>
        bool IsGameWon();

        /// <summary>
        /// Performs one turn in the game.
        /// </summary>
        void TakeTurn();
    }
}
