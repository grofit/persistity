using System;

namespace Persistity.Tests.Models
{
    public class GameData
    {
        public int CurrentLevel { get; set; }
        public Guid CharacterId { get; set; }

        public static GameData CreateRandom()
        {
            var random = new Random();
            return new GameData
            {
                CurrentLevel = random.Next(1, 1000),
                CharacterId = new Guid()
            };
        }
    }
}