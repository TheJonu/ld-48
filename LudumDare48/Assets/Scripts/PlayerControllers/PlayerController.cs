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

    Sprite oldSprite;
    public Sprite ladderSprite;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {

        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            // do nothing plz
        } 
        else if(Input.GetKey(KeyCode.D))
        {
            Vector2 targetSpeed = new Vector2(xSpeed, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }

        else if(Input.GetKey(KeyCode.A))
        {
            Vector2 targetSpeed = new Vector2(-xSpeed, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            LadderCheck();
        else
        {
            Vector2 targetSpeed = new Vector2(0, rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, targetSpeed, ref refVel, speedDampening);
        }

        Flip();
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
        int size = Physics2D.BoxCast(transform.position, bx2d.size, 180, Vector2.down, climbLayerMask, rhit, (bx2d.size.y / 2));
        
        Debug.Log(size);

        if(size == 1)
        {
           this.enabled = false;

            rb2d.velocity = Vector2.zero;

           LadderController ld = gameObject.AddComponent<LadderController>();

           // better calculation are needed for this. Will do for now
           ld.up = Vector2.up;
           ld.down = Vector2.down;
           ld.ladderSprite = ladderSprite;
           ld.returnTo = this;

           oldSprite = GetComponent<SpriteRenderer>().sprite;

            Vector3 offSetPos = gameObject.transform.position;
            offSetPos.x = rhit[0].rigidbody.position.x;
            gameObject.transform.position = offSetPos;

            bx2d.enabled = false;
        }
    }
    public void ResetBack(Vector3 newPosition) 
    {
        this.enabled = true;
        GetComponent<SpriteRenderer>().sprite = oldSprite;
        rb2d.gravityScale = 1;
        bx2d.enabled = true;
    }

    public void pushUp()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];
        int size = Physics2D.BoxCast(transform.position, bx2d.size, 0, Vector2.zero, climbLayerMask, rhit);
    }
}
