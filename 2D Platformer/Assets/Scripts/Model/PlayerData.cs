using System;

namespace Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Health;
        public bool IsArmed;

        public void Initialize(PlayerData playerData)
        {
            Coins = playerData.Coins;
            Health = playerData.Health;
            IsArmed = playerData.IsArmed;
        }
    }
}