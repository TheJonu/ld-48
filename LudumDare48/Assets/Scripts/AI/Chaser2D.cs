using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser2D : PlayerController
{

    public Transform toChase;

    public float activationDistance;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Debug.Log(Vector2.Distance(toChase.position, transform.position));
        if (Vector2.Distance(toChase.position, transform.position) >= activationDistance)
        {
            
            Vector2 targetSpeed = new Vector2(0, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }

        else if (transform.position.x < toChase.position.x)
        {
            Debug.Log("RIGHT");
            Vector2 targetSpeed = new Vector2(xSpeed, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }

        else if (transform.position.x > toChase.position.x)
        {
            Debug.Log("LEFT!");
            Vector2 targetSpeed = new Vector2(-xSpeed, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }
        else
        {
            Vector2 targetSpeed = new Vector2(0, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }

        Flip();
    }
}
