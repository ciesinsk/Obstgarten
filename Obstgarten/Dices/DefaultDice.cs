using System.Runtime.InteropServices.JavaScript;

namespace Obstgarten.Dices
{
    public class DefaultDice<T> : IDice<T>
        where T : Enum
    {
        private Random m_r = new Random(0);

        public DefaultDice()
        {

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
