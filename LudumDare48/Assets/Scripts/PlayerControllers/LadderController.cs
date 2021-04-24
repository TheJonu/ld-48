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

        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * ladderSpeedMod;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * Time.deltaTime * ladderSpeedMod;
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            returnTo.ResetBack(transform.position);
            Destroy(this);
        }
    }
}
