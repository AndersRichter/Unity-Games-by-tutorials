using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Components
{
    public class CoinsSpawnerComponent : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private int maxAmount;
        [SerializeField] private Coin[] coins;
        [SerializeField] private float minThrowingDistance;
        [SerializeField] private float maxThrowingDistance;
        [SerializeField] [Range(1, 179)] private int minThrowingAngle;
        [SerializeField] [Range(1, 179)] private int maxThrowingAngle;
        [SerializeField] [Range(1, 179)] private int centerGapInThrowingAngle;
        
        private readonly List<int> _arrayOfCoinsProbabilities = new();

        private void Awake()
        {
            var allProbabilities = coins.Select(coin => coin.Probability).ToArray();
            for (var i = 0; i < allProbabilities.Length; i++)
            {
                var probability = allProbabilities[i];
                _arrayOfCoinsProbabilities.AddRange(Enumerable.Repeat<int>(i, probability));
            }
        }

        [ContextMenu("SpawnCoins")]
        public void SpawnCoins()
        {
            if (maxAmount > 0 && coins.Length > 0)
            {
                for (var i = 0; i < maxAmount; i++)
                {
                    var randomCoinIndex = _arrayOfCoinsProbabilities[new Random().Next(0, _arrayOfCoinsProbabilities.Count)];
                    var coinPrefab = coins[randomCoinIndex].Prefab;
                    var coin = Instantiate(coinPrefab, destination.position, Quaternion.identity);

                    LaunchCoinProjectile(coin);
                }
            }
        }

        private void LaunchCoinProjectile(GameObject coin)
        {
            var coinProjectile = coin.GetComponent<ParabolaMovementComponent>();

            if (coinProjectile)
            {
                var rand = new Random();
                coinProjectile.Distance = (float) rand.NextDouble() * (maxThrowingDistance - minThrowingDistance) + minThrowingDistance;
                
                // we remove centerGapInThrowingAngle from center to not throw coins too vertical = high
                var halfOfAllowedAngleSector = (maxThrowingAngle - minThrowingAngle - centerGapInThrowingAngle) / 2;
                var randomMinAngle = rand.Next(minThrowingAngle, minThrowingAngle + halfOfAllowedAngleSector);
                var randomMaxAngle = rand.Next(maxThrowingAngle - halfOfAllowedAngleSector, maxThrowingAngle);
                
                coinProjectile.Angle = coinProjectile.Distance > 0 ? randomMinAngle : randomMaxAngle;
                coinProjectile.enabled = true;
            }
        }
    }

    [Serializable]
    public class Coin
    {
        [SerializeField] [Range(0, 100)] public int Probability;
        public GameObject Prefab;
    }
}