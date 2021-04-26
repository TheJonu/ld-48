using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{

    Rigidbody2D rb2d;
    BoxCollider2D bx2d;
    SpriteRenderer sr;

    float ladderSpeedMod = 2.5f;

    [HideInInspector] public Sprite ladderSprite;

    [HideInInspector] public Vector2 up;
    [HideInInspector] public Vector2 down;

    [HideInInspector] public PlayerController returnTo;

    [HideInInspector] public ContactFilter2D filter2D;

    private bool moving;

    public Action Destroyed;


    public bool IsMoving => moving;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        sr.sprite = ladderSprite;

        rb2d.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moving = true;
        Vector2 returnPos = transform.position;
        if(Input.GetKey(KeyCode.W) && isAtTop() && !isDownOnly())
        {
            transform.position += Vector3.up * Time.deltaTime * ladderSpeedMod;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * Time.deltaTime * ladderSpeedMod;
        }
        else if (Input.GetKey(KeyCode.D))
        { 
            transform.position += Vector3.right * Time.deltaTime * ladderSpeedMod / 2;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * ladderSpeedMod / 2;
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            Destroy(this);
        }
        else
        {
            moving = false;
        }

        if(!ladderCheck())
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        returnTo.ResetBack(transform.position);
        Destroyed?.Invoke();
    }

    bool isAtTop()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];
        Vector3 lowerPosition = transform.position;

        lowerPosition.x -= bx2d.size.y / 4;

        Vector3 smallBounds = bx2d.bounds.extents;
        smallBounds.y /= 2;

        int size = Physics2D.BoxCast(lowerPosition, smallBounds, 0, Vector2.zero, filter2D, rhit);

        ExtDebug.DrawBoxCastBox(lowerPosition, smallBounds, Quaternion.identity, Vector2.zero, Mathf.Infinity, Color.blue);
        if (size == 0)
        {
            return false;
        }

        return true;
    }

    bool isDownOnly()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];

        int size = Physics2D.BoxCast(transform.position, bx2d.bounds.extents, 0, Vector2.zero, filter2D, rhit);

        for(int i=0; i<size; i++)
        {
            if(rhit[i].transform.gameObject.tag == "DownOnlyLadder")
            {
                return true;
            }
        }

        return false;
    }

    bool ladderCheck()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];

        int size = Physics2D.BoxCast(transform.position, bx2d.bounds.extents, 0, Vector2.zero, filter2D, rhit);

        ExtDebug.DrawBoxCastBox(transform.position, bx2d.bounds.extents, Quaternion.identity, Vector2.zero, Mathf.Infinity, Color.red);
        if(size == 0)
        {
            return false;
        }

        return true;
    }

    bool isMoving()
    {
        return moving;
    }
}
