using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerUp : MonoBehaviour {
    
    public GameObject boostEffect;

    void Update()
    {
        this.transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        this.transform.Rotate(Vector3.right * 100 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyPowerUp(collision.gameObject);
        }

    }

    void ApplyPowerUp(GameObject playerObject)
    {
        // Spawn a fire particle system
        Instantiate(boostEffect, transform.position, boostEffect.transform.rotation);

        // Apply rocket effect to player
        playerObject.GetComponent<PlayerScript>().NoGravity();

        // Destroy
        Destroy(gameObject);
    }
}
