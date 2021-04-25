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
        Vector2 returnPos = transform.position;
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * ladderSpeedMod;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * Time.deltaTime * ladderSpeedMod;
        }
        if (Input.GetKey(KeyCode.D))
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

        if(!ladderCheck())
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        returnTo.ResetBack(transform.position);
    }

    bool ladderCheck()
    {
        RaycastHit2D[] rhit = new RaycastHit2D[256];

        int size = Physics2D.BoxCast(transform.position, bx2d.size, 0, Vector2.zero, filter2D, rhit);

        if(size == 0)
        {
            return false;
        }

        return true;
    }
}
