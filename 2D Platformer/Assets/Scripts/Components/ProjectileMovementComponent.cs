using System.Collections;
using UnityEngine;

namespace Components
{
    public class ProjectileMovementComponent : MonoBehaviour
    {
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private GameObject projectile;
        [SerializeField] public float distance;
        [SerializeField] public float angle;

        private void OnEnable()
        {
            StartCoroutine(SimulateProjectile());
        }

        private IEnumerator SimulateProjectile()
        {
            // Calculate the velocity needed to throw the object to the distance at specified angle.
            var velocity = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
 
            // Extract the X and Y components of the velocity
            var xSpeed = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
            var ySpeed = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);
 
            // Calculate flight time
            var flightDuration = distance / xSpeed;

            float elapseTime = 0;
 
            while (elapseTime < flightDuration)
            {
                projectile.transform.Translate(xSpeed * Time.deltaTime, (ySpeed - (gravity * elapseTime)) * Time.deltaTime, 0);
           
                elapseTime += Time.deltaTime;
                
                yield return null;
            }
            
            enabled = false;
        }
    }
}