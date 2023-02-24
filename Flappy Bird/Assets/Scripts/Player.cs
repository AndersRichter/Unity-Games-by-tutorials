using UnityEngine;

public class Player : MonoBehaviour
{
    // public float speed;
    public float jumpForce;
    public Rigidbody2D rb;
    public BirdAnimation birdAnimation;

    // Update is called once per frame
    private void Update()
    {
        // Example of player moves
        // if (Input.GetKey(KeyCode.D))
        // {
        //     rb.velocity = new Vector2(speed, rb.velocity.y);
        //     // transform.Translate(transform.right * speed * Time.deltaTime);
        // }
        //
        // if (Input.GetKey(KeyCode.A))
        // {
        //     rb.velocity = new Vector2(-speed, rb.velocity.y);
        //     // transform.Translate(-transform.right * speed * Time.deltaTime);
        // }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2();
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            birdAnimation.StartRotation();
        }
        birdAnimation.ApplyRotation(rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PipePart"))
        {
            GameManager.instance.Lose();
        }

        if (col.gameObject.CompareTag("Floor"))
        {
            birdAnimation.StopRotation();
        }
    }
}
