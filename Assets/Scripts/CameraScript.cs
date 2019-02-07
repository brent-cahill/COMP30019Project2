using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float offset = 5.0f;
    float height = 0;
    public float cameraSpeed = 10.0f;
    public Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // Player is moving without touching platforms
        if (player.position.y > height + offset + 1) 
        {
            height = player.position.y - offset;
        }
    }


    // if player collides with platform and if height higher than earlier, update camera position
    public void UpdateHeight(Vector3 platformPosition)
    {
        if (platformPosition.y > height)
        {
            height = platformPosition.y;
        }
    }

    //Smooth transition towards new point for camera
    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, height + offset, transform.position.z), cameraSpeed * Time.deltaTime);
    }
}
