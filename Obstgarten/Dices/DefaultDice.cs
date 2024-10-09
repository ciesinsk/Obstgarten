using Obstgarten.Game;

namespace Obstgarten.Dices
{
    public class DefaultDice<T> : IDice<T>
        where T : Enum
    {
        private Random m_r;

        public int NumberTosses {get;private set;} =0;
        public DefaultDice() 
        {
            m_r = new Random(0);
        }

        public DefaultDice(Guid guid) 
        {
            m_r = new Random(guid.GetHashCode());
        }

        public T NextToss(IGame<T> game)
        {
            if (game.HasGameEnded())
            {
                throw new Exception("Cannot get next toss - the game has ended.");
            }

            while (true)
            {
                var candidateToss = GetRandomElement();
                NumberTosses++;

                if (game.RavenColors.Contains(candidateToss) || game.JokerColors.Contains(candidateToss))
                {
                    return candidateToss;
                }

                if (game.FruitsLeft.ContainsKey(candidateToss) && game.FruitsLeft[candidateToss] > 0)
                {
                    return candidateToss;
                }
            }
        }

#pragma warning disable 8600, 8603

        private T GetRandomElement()
        {
            var v = Enum.GetValues(typeof(T));

            return (T)v.GetValue(m_r.Next(v.Length));
        }
#pragma warning restore 8600, 8603
    }
}
