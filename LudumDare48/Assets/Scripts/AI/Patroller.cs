using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : PlayerController
{

    public Vector2 goesFrom;
    public Vector2 goesTo;


    public bool flipable = true;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Stop()
    {
        Vector2 targetSpeed = new Vector2(0, rb2d.velocity.y);
        Vector2 ret = Vector2.zero;
        rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(goesTo, transform.position) <= 0.1) 
        {
            Vector2 swap = goesTo;
            goesTo = goesFrom;
            goesFrom = swap;
            Stop();
        }

        Vector2.SmoothDamp(rb2d.position, goesTo, ref refVel, speedDampening, xSpeed);

        rb2d.velocity = refVel;

        if(flipable)
            Flip();
    }
}
