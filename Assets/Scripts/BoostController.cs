using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour {

    private GameObject player;
    private float lifetime = 1.6f;
    private Slider boostSlider;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        boostSlider = GameObject.FindWithTag("BoostSlider").GetComponent<Slider>();
        boostSlider.maxValue = lifetime;
        boostSlider.value = lifetime;

        StartCoroutine(ResetOnExpiry());
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y-1.6f, player.transform.position.z);
        boostSlider.value -= Time.deltaTime;
	}

    private IEnumerator ResetOnExpiry() 
    {
        yield return new WaitForSeconds(lifetime);
        player.GetComponent<PlayerScript>().ResetGravity();
        player.GetComponent<PlayerScript>().setJumpTimer();
        Destroy(gameObject);
    }
}
