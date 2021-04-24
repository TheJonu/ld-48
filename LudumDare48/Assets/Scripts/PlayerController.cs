using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float xSpeed = .5f;
    Rigidbody2D rb2d;
    Vector2 refVel = Vector2.zero;

    float speedDampening = .075f;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        Flip();

        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            return;
        } 
        else if(Input.GetKey(KeyCode.D))
        {
            Debug.Log("Right pushed ->");
            Vector2 targetSpeed = new Vector2(xSpeed, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }

        else if(Input.GetKey(KeyCode.A))
        {
            Debug.Log("Left pushed <-");
            Vector2 targetSpeed = new Vector2(-xSpeed, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }
    }

    void Flip()
    {
        if(rb2d.velocity.x > 0.1)
        {
            Vector3 sc = gameObject.transform.localScale;
            sc.x = Mathf.Abs(sc.x);
            gameObject.transform.localScale = sc;
        }
        else if (rb2d.velocity.x < -0.1)
        {
            Vector3 sc = gameObject.transform.localScale;
            sc.x = -Mathf.Abs(sc.x);
            gameObject.transform.localScale = sc;
        }
    }
}
