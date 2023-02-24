using UnityEngine;

public class BirdAnimation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float takeoffRotation;
    [SerializeField] private float landingRotation;
    [SerializeField] private Sprite takeoffSprite;
    [SerializeField] private Sprite landingSprite;

    private float rotationZ;
    private bool isRotationAllowed = true;
    private Sprite defaultSprite;

    private void Start()
    {
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
    }
    
    public void ApplyRotation(float velocityY)
    {
        if (isRotationAllowed && rotationZ > landingRotation)
        {
            float offset = 1f;
            if (velocityY > 0.5f)
            {
                offset = Mathf.Abs(velocityY);
            }

            rotationZ -= rotationSpeed * Time.deltaTime / offset;
            SetAngle();

            if (rotationZ > 0)
            {
                GetComponent<SpriteRenderer>().sprite = takeoffSprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = landingSprite;
            }
        }
    }

    private void SetAngle()
    {
        transform.localEulerAngles = new Vector3(0, 0, rotationZ);
    }

    public void StartRotation()
    {
        isRotationAllowed = true;
        rotationZ = takeoffRotation; 
    }

    public void StopRotation()
    {
        isRotationAllowed = false;
        rotationZ = 0;
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        SetAngle();
    }
}
