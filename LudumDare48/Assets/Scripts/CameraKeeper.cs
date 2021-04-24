using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeeper : MonoBehaviour
{

    Camera cam;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 leftBorder = cam.ViewportToWorldPoint(new Vector3(0.33f, 0, cam.transform.position.z));
        Vector3 rightBorder = cam.ViewportToWorldPoint(new Vector3(0.67f, 0, cam.transform.position.z));
        Vector3 botBorder = cam.ViewportToWorldPoint(new Vector3(0, 0.33f, cam.transform.position.z));
        Vector3 topBorder = cam.ViewportToWorldPoint(new Vector3(0, 0.67f, cam.transform.position.z));

        Debug.Log(player.transform.position.x < leftBorder.x);

        if(player.transform.position.x < leftBorder.x)
        {
            float diff = leftBorder.x - player.transform.position.x;

            cam.transform.position -= new Vector3(diff, 0, 0);
        }

        if (player.transform.position.x > rightBorder.x)
        {
            float diff = player.transform.position.x - rightBorder.x;

            cam.transform.position += new Vector3(diff, 0, 0);
        }

        if (player.transform.position.y > topBorder.y)
        {
            float diff = player.transform.position.y - topBorder.y;

            cam.transform.position += new Vector3(0, diff, 0);
        }

        if (player.transform.position.y < botBorder.y)
        {
            float diff = botBorder.y - player.transform.position.y;

            cam.transform.position -= new Vector3(0, diff, 0);
        }

    }
}
