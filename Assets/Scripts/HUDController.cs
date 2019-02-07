using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public Transform player;
    public Text score;
    private float maxHeight;

    void Start()
    {
        maxHeight = 0;
    }

    // Update is called once per frame
    void Update () 
    {
        if (player.position.y > maxHeight) 
        {
            UpdateScore(player.position.y);
        }
	}

    void UpdateScore(float newScore) 
    {
        maxHeight = newScore;
        score.text = "Score: " + Mathf.RoundToInt(newScore).ToString();
    }
}
