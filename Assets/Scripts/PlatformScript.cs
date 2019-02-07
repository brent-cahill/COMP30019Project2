using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    public float jumpHeight = 10f; 
    CameraScript cam;
    Transform player;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraScript>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        // Destroy platforms once we have advanced
        if ((player.position.y) > this.GetComponent<Transform>().position.y + jumpHeight) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Make sure collision only happen when coming from above
        if (collision.relativeVelocity.y <= 0f)
        {  
            //Let the camera know that a collision happened
            cam.UpdateHeight(transform.position);
        }

    }
}
