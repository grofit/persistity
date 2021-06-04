using System;

namespace Persistity.Tests.Models
{
    public class GameData : IEquatable<GameData>
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

        public bool Equals(GameData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CurrentLevel == other.CurrentLevel && CharacterId.Equals(other.CharacterId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CurrentLevel * 397) ^ CharacterId.GetHashCode();
            }
        }
    }
}