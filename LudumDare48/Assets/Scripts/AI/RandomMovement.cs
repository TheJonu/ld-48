using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : PlayerController
{
    public float maxTime = 2;
    public float minTime = 5;

    float timeLeft = -1f;

    public bool flipable = true;

    int direction = 0;
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

    void RollNew()
    {
        int p = 0;
        int[] directionPick = new int[2];
        for(int i=-1; i<=1; i++)
        {
            if (i == direction)
                continue;
            directionPick[p++] = i;
        }
        timeLeft = Random.Range(minTime, maxTime);
        direction = directionPick[Random.Range(0, 2)];
    }

    void FixedUpdate()
    {
        if (timeLeft <= 0)
        {
            Stop();
            RollNew();
        }

        Vector2 targetSpeed = new Vector2(xSpeed * direction, rb2d.velocity.y);
        Vector2 ret = Vector2.zero;

        Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening, xSpeed);

        rb2d.velocity = refVel;

        if (direction == 0)
            rb2d.velocity = Vector2.zero;

        timeLeft -= Time.deltaTime;
        
    }
}
