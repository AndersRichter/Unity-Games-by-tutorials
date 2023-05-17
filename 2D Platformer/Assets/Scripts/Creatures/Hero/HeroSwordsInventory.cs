using UnityEngine;
using UnityEngine.Events;

namespace Creatures.Hero
{
    public class HeroSwordsInventory : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> onUpdate;
        
        public int Swords { get; private set; }

        public void AddSword()
        {
            Swords++;
            onUpdate?.Invoke(Swords);
        }

        public void RemoveSword()
        {
            Swords--;
            onUpdate?.Invoke(Swords);
        }

        public void SetInitialSwords(int initialSwords)
        {
            Swords = initialSwords;
        }
    }
}
