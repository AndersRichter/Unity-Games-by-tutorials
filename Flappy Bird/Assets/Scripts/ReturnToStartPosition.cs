using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToStartPosition : MonoBehaviour
{
    public float startX;
    public float endX;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < endX)
        {
            transform.position = new Vector2(startX, transform.position.y);
        }
    }
}
