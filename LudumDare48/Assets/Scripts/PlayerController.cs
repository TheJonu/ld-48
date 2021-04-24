using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;
    BoxCollider2D bx2d;

    public float xSpeed = .5f;
    
    Vector2 refVel = Vector2.zero;

    float speedDampening = .05f;

    public ContactFilter2D climbLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
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
        else if (Input.GetKeyDown(KeyCode.W))
            LadderCheck();
        else
        {
            Vector2 targetSpeed = new Vector2(0, rb2d.velocity.y);
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

    void LadderCheck()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];
        int size = Physics2D.BoxCast(transform.position, bx2d.size, 0, Vector2.zero, climbLayerMask, rhit);

        Debug.Log(size);

        if(size == 1)
        {
            // attach to ladder
        }
    }
}
