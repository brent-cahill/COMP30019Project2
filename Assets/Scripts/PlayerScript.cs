using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public float speed = 8.0f;
    public float wall = 10.0f;

    public float fallMultiplier = 2f;
    Rigidbody2D rb;
    public Text gameoverText;
    public GameObject playAgain;

    private Animator anim;
    float jumpTimer = 0;

    public float jumpHeight = 10f;
    

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Make player fall faster, normal case * fallMultiplier
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }

        // Move player with left and right key or A and D
        float movement = Input.GetAxis("Horizontal");
        
        rb.AddForce(new Vector2(movement*speed*100, 0));
        rb.AddForce(new Vector2(Input.acceleration.x * speed*100, 0));

        // Teleport player to other side when going through "walls"
        if (transform.position.x > wall)
            transform.position = new Vector3(-wall, transform.position.y, transform.position.z);

        if (transform.position.x < -wall)
            transform.position = new Vector3(wall, transform.position.y, transform.position.z);


        // Jump animation
        if (jumpTimer > 0.5)
            jumpTimer -= Time.deltaTime;
        else if (anim.GetBool("Jumping") == true)
            anim.SetBool("Jumping", false);
    }

   
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only jump when collision happens from above
        if (collision.relativeVelocity.y >= 0f)
        {
            jumpTimer = 1;
            anim.SetBool("Jumping", true);
            rb.AddForce(new Vector2(0, jumpHeight));
            
            Vector2 velocity = rb.velocity;
            velocity.y = jumpHeight;
            rb.velocity = velocity;
           
        }   
    }

    //Kill player when outside camera 
    void OnBecameInvisible()
    {
        Destroy(gameObject);
        gameoverText.text = "Game Over";
        playAgain.SetActive(true);

    }

    // Activated with BoostPowerup 
    public void NoGravity() {
        rb.velocity = new Vector2(0, jumpHeight);
        rb.gravityScale = 0;
    }

    // Activated with BoostPowerup
    public void ResetGravity() {
        rb.AddForce(new Vector2(0, jumpHeight));
        rb.gravityScale = 1;
    }

    public void setJumpTimer()
    {
        anim.SetBool("Jumping", true);
        jumpTimer = 1;
    }

}
