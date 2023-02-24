using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 3;

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
