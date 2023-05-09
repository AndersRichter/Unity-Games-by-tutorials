using Components;
using UnityEngine;
using UnityEngine.Events;

namespace Creatures.Hero
{
    public class HeroCoinsInventory : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> onUpdate;
        public int Coins { get; private set; }

        public void AddCoin(int coin)
        {
            Coins += coin;
            onUpdate?.Invoke(Coins);
            Debug.Log("Coins: " + Coins);
        }
    
        public void AddCoin(ValueComponent coin)
        {
            Coins += coin.Value;
            onUpdate?.Invoke(Coins);
            Debug.Log("Coins: " + Coins);
        }

        public void RemoveCoin(int coin)
        {
            Coins -= coin;
            onUpdate?.Invoke(Coins);
        }

        public void SetInitialCoins(int initialCoins)
        {
            Coins = initialCoins;
        }
    }
}
