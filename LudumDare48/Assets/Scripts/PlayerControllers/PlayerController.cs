using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    protected Rigidbody2D rb2d;
    protected Collider2D coll2d;

    public float xSpeed = .0f;

    protected float refVel = 0.0f;

    protected float speedDampening = 0.05f;

    [SerializeField] ContactFilter2D climbLayerMask;
    [SerializeField] LayerMask terrainLayerMask;

    Sprite oldSprite;
    public Sprite ladderSprite;

    public Vector2 collBoxSize = Vector2.one * 1f;
    
    [SerializeField] private bool touchedGround = false;

    private LadderController lc;
    public LadderController LadderController => lc;

    [SerializeField] private AudioClip fallSound;
    [SerializeField] private float fallSoundVelocityThreshold;
    [SerializeField] private float fallSoundVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        coll2d = gameObject.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {

        Flip();

        bool leftK = Input.GetKey(KeyCode.A);
        bool rightK = Input.GetKey(KeyCode.D);

        if (rightK && !leftK)
        {
            if (IsGrounded() || !IsGroundedButLeft(false))
            {
                rb2d.velocity = new Vector2(Mathf.SmoothDamp(rb2d.velocity.x, xSpeed, ref refVel, speedDampening), rb2d.velocity.y);
            }
        }
        else if (leftK && !rightK)
        {
            if (IsGrounded() || !IsGroundedButLeft(true))
            {
                rb2d.velocity = new Vector2(Mathf.SmoothDamp(rb2d.velocity.x, -xSpeed, ref refVel, speedDampening), rb2d.velocity.y);
            }
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            LadderCheck();
        } 
        else
        {
            rb2d.velocity = new Vector2(Mathf.SmoothDamp(rb2d.velocity.x, 0.0f, ref refVel, speedDampening), rb2d.velocity.y);
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

        if(size > 1)
        {
            this.enabled = false;

            rb2d.velocity = Vector2.zero;

            LadderController ld = gameObject.AddComponent<LadderController>();

            // better calculation are needed for this. Will do for now
            ld.up = Vector2.up;
            ld.down = Vector2.down;
            ld.ladderSprite = ladderSprite;
            ld.returnTo = this;
            ld.filter2D = climbLayerMask;

            lc = ld;
            lc.Destroyed += () => lc = null;    // set lc to null when destroyed. Should be working without it but caused some confusion

            oldSprite = GetComponent<SpriteRenderer>().sprite;

            Vector3 offSetPos = gameObject.transform.position;
            offSetPos.x = rhit[0].transform.position.x;
            gameObject.transform.position = offSetPos;

            // coll2d.enabled = false;
        }
    }
    public void ResetBack(Vector3 newPosition) 
    {
        this.enabled = true;
        GetComponent<SpriteRenderer>().sprite = oldSprite;
        rb2d.gravityScale = 1;
        // pushUp();
        coll2d.enabled = true;
        touchedGround = false;
    }

    private void pushUp()
    {
        int size;
        do
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.layerMask = terrainLayerMask;
            filter.useLayerMask = true;

            RaycastHit2D[] rhit = new RaycastHit2D[256];
            size = Physics2D.BoxCast(transform.position, collBoxSize, 0, Vector2.zero, filter, rhit);

            if(size > 0)
            {
                transform.position += Vector3.up * 0.01f;
            }

        } while (size > 0);
        
    }

    // jam horror
    bool IsGroundedButLeft(bool isItActuallyLeft)
    {

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = terrainLayerMask;
        filter.useLayerMask = true;

        Vector2 box = coll2d.bounds.extents;

        box += new Vector2(0, 0.02f) * (isItActuallyLeft ? -1.0f : 1.0f);

        Vector2 leftUp = coll2d.bounds.center;
        Vector2 leftDown = coll2d.bounds.center;

        leftUp.x += coll2d.bounds.size.x * (isItActuallyLeft ? -1.0f : 1.0f);
        leftDown.x += coll2d.bounds.size.x * (isItActuallyLeft ? -1.0f : 1.0f);
        leftUp.y -= coll2d.bounds.size.y;
        leftDown.y += coll2d.bounds.size.y;

        {
            RaycastHit2D[] rhits = new RaycastHit2D[256];

            int size = Physics2D.BoxCast(leftUp, box, 0f, Vector2.zero, filter, rhits);

            ContactPoint2D[] point = new ContactPoint2D[256];

            int contSize = rb2d.GetContacts(point);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < contSize; j++)
                {
                    if (point[j].collider == rhits[i].collider)
                    {
                        Debug.Log("Yes I am grounded!");
                        Debug.Log(rhits[i].collider);
                        return true;
                    }
                }
            }
        }
        {
            RaycastHit2D[] rhits = new RaycastHit2D[256];

            int size = Physics2D.BoxCast(leftDown, box, 0f, Vector2.zero, filter, rhits);

            ContactPoint2D[] point = new ContactPoint2D[256];

            int contSize = rb2d.GetContacts(point);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < contSize; j++)
                {
                    if (point[j].collider == rhits[i].collider)
                    {
                        Debug.Log("Yes I am grounded!");
                        Debug.Log(rhits[i].collider);
                        return true;
                    }
                }
            }
        }

        ExtDebug.DrawBoxCastBox(leftUp, box, Quaternion.identity, Vector2.zero, 0, Color.green);
        ExtDebug.DrawBoxCastBox(leftDown, box, Quaternion.identity, Vector2.zero, 0, Color.green);
        return false;
    }

    bool IsGrounded()
    {

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = terrainLayerMask;
        filter.useLayerMask = true;

        Vector2 box = coll2d.bounds.extents;

        box -= new Vector2(0.02f, 0);

        Vector2 lowerDown = coll2d.bounds.center;

        lowerDown.y -= coll2d.bounds.size.y;

        RaycastHit2D[] rhits = new RaycastHit2D[256];

        int size = Physics2D.BoxCast(lowerDown, box, 0f, Vector2.zero, filter, rhits);

        ContactPoint2D[] point = new ContactPoint2D[256];

        int contSize = rb2d.GetContacts(point);

        for(int i = 0; i<size; i++)
        {
            for (int j = 0; j<contSize; j++)
            {
                if (point[j].collider == rhits[i].collider)
                {
                    //Debug.Log("Yes I am grounded!");
                    //Debug.Log(rhits[i].collider);
                    return true;
                }
            }
        }

        ExtDebug.DrawBoxCastBox(lowerDown, box, Quaternion.identity, Vector2.zero, 0, Color.red);
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.relativeVelocity.y != 0) Debug.Log(collision.relativeVelocity.y);
        if (collision.relativeVelocity.y > fallSoundVelocityThreshold && !(fallSound is null))
        {
            //Debug.Log("Play");
            AudioSource.PlayClipAtPoint(fallSound, transform.position, fallSoundVolume);
        }
    }
}
