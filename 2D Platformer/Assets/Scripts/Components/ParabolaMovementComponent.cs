using System.Collections;
using UnityEngine;

namespace Components
{
    public class ParabolaMovementComponent : MonoBehaviour
    {
        [SerializeField] public float Distance;
        [SerializeField] public float Angle;

        private const float Gravity = 9.8f;

        private void OnEnable()
        {
            StartCoroutine(ParabolaMovement());
        }

        private IEnumerator ParabolaMovement()
        {
            // Calculate the velocity needed to throw the object to the distance at specified angle.
            var velocity = Distance / (Mathf.Sin(2 * Angle * Mathf.Deg2Rad) / Gravity);
 
            // Extract the X and Y components of the velocity
            var xSpeed = Mathf.Sqrt(velocity) * Mathf.Cos(Angle * Mathf.Deg2Rad);
            var ySpeed = Mathf.Sqrt(velocity) * Mathf.Sin(Angle * Mathf.Deg2Rad);
 
            // Calculate flight time
            var flightDuration = Distance / xSpeed;

            float elapseTime = 0;
 
            while (elapseTime < flightDuration)
            {
                gameObject.transform.Translate(xSpeed * Time.deltaTime, (ySpeed - (Gravity * elapseTime)) * Time.deltaTime, 0);
           
                elapseTime += Time.deltaTime;
                
                yield return null;
            }
            
            enabled = false;
        }
    }
}