using System;
using LazyData.Attributes;

namespace Persistity.Tests.Models
{
    [Persist]
    public class GameData
    {
        [PersistData]
        public int CurrentLevel { get; set; }
        
        [PersistData]
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