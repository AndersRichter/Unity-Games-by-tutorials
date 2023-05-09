using System.Collections;
using UnityEngine;

namespace Creatures.EnemyAI
{
    public abstract class EnemyAIPatrol : MonoBehaviour
    {
        public abstract IEnumerator Patrol();
    }
}