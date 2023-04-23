using UnityEngine;

namespace Components
{
    public class MakeHeroArmedComponent : MonoBehaviour
    {
        public void Arm(GameObject target)
        {
            var hero = target.GetComponent<Hero>();

            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}