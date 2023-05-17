using System;

namespace Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Swords;
        public int Health;
        public bool IsArmed;

        public void Initialize(PlayerData playerData)
        {
            Coins = playerData.Coins;
            Swords = playerData.Swords;
            Health = playerData.Health;
            IsArmed = playerData.IsArmed;
        }
    }
}