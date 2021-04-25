using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    protected Rigidbody2D rb2d;
    protected Collider2D coll2d;

    public float xSpeed = .5f;
    
    protected Vector2 refVel = Vector2.zero;

    protected float speedDampening = .05f;

    [SerializeField] ContactFilter2D climbLayerMask;
    [SerializeField] LayerMask terrainLayerMask;

    Sprite oldSprite;
    public Sprite ladderSprite;

    public Vector2 collBoxSize = Vector2.one * 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        coll2d = gameObject.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {

        Flip();

        if (!IsGrounded())
        {
            return;
        }

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
    }

    protected void Flip()
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
        int size = Physics2D.BoxCast(transform.position, collBoxSize, 180, Vector2.down, climbLayerMask, rhit, (collBoxSize.y / 2));
        
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

            coll2d.enabled = false;
        }
    }
    public void ResetBack(Vector3 newPosition) 
    {
        this.enabled = true;
        GetComponent<SpriteRenderer>().sprite = oldSprite;
        rb2d.gravityScale = 1;
        coll2d.enabled = true;
    }

    public void pushUp()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];
        int size = Physics2D.BoxCast(transform.position, collBoxSize, 0, Vector2.zero, climbLayerMask, rhit);
    }

    bool IsGrounded()
    {
        float extender = 0.4f;

        float pyT = Mathf.Sqrt(coll2d.bounds.extents.y * coll2d.bounds.extents.y + coll2d.bounds.extents.x * coll2d.bounds.extents.x);

        RaycastHit2D hit = Physics2D.BoxCast(coll2d.bounds.center, coll2d.bounds.size, 0f, Vector2.down, extender, terrainLayerMask);

        return hit.collider != null;
    }
}
