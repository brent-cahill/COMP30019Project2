using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform player;
    public GameObject boostPrefab;
    public GameObject platformPrefab;

    private float spawnWait = 10.0f;
    private float boostSpawnInterval;
    private float platformOffset;
    private float nextPlatformSpawn;


	// Use this for initialization
	void Start () {
        boostSpawnInterval = spawnWait;
        platformOffset = 4.0f;
        nextPlatformSpawn = platformOffset;
        InitializePlatforms();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 cameraPosition = Camera.main.gameObject.transform.position;

        if (boostSpawnInterval < 0) 
        {
            SpawnBoostPrefab();
            boostSpawnInterval = spawnWait;
        } else {
            boostSpawnInterval -= Time.deltaTime;
        }

        if (player.position.y >= nextPlatformSpawn)
        {
            SpawnPlatformPrefab();
            nextPlatformSpawn += platformOffset;
        }
    }

    void SpawnBoostPrefab() {
        float minHeight = player.position.y + 0.0f;
        float maxHeight = minHeight + 5.0f;
        Vector3 position = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(minHeight, maxHeight), 0f);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        Instantiate(boostPrefab, position, rotation);
    }

    void SpawnPlatformPrefab() {
        float height = GetHighestPlatform() + platformOffset;
        Vector3 position = new Vector3(Random.Range(-9.0f, 9.0f), height, 0f);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        Instantiate(platformPrefab, position, rotation);
    }

    private float GetHighestPlatform() {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        float max = 0.0f;
        for (int i = 0; i < platforms.Length; i++) {
            float platformHeight = platforms[i].GetComponent<Transform>().position.y;
            if (platformHeight > max) {
                max = platformHeight;
            }
        }
        return max;
    }

    void InitializePlatforms() {
        
        int numStartingPlatforms = 5;
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        float lastHeight = 0.0f;
        Instantiate(platformPrefab, new Vector3(1, lastHeight, 0), rotation);    // Initialize starting PLatform

        for (int i = 0; i <= numStartingPlatforms; i++) {
            lastHeight += platformOffset;
            Vector3 position = new Vector3(Random.Range(-9.0f, 9.0f), lastHeight, 0f);
            Instantiate(platformPrefab, position, rotation);
        }
    }
}
