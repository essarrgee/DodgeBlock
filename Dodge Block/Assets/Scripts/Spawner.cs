using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject enemyPrefab;
	
	private float overallSpeed;
	private float spawnInterval;
	private float tick;
	
	private int level;
	private Vector2 spawnPosition;
	private Vector2 spawnForce;
	private Vector2 spawnScale;
	
    void Awake()
    {
        tick = 0;
		
		level = 1;
		overallSpeed = 0.1f;
		spawnInterval = 3f;
		spawnPosition = new Vector2(-6.5f,0);
		spawnForce = new Vector2(0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tick >= spawnInterval) {
			tick = 0;
			spawnPosition = transform.position;
			if (spawnPosition.x > 0.5f || spawnPosition.x < -0.5f) {
				spawnPosition = spawnPosition + new Vector2(0, Random.Range(-1f,1f)*5f);
				spawnForce = (spawnPosition * new Vector2(-1f,1f)) - spawnPosition;
			} else if (spawnPosition.y > 0.5f || spawnPosition.y < -0.5f) {
				spawnPosition = spawnPosition + new Vector2(Random.Range(-1f,1f)*5.5f, 0);
				spawnForce = (spawnPosition * new Vector2(1f,-1f)) - spawnPosition;
			}
			//spawnForce = spawnForce.normalized;
			GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
			//newEnemy.transform.localScale = spawnScale;
			Rigidbody2D rb = newEnemy.GetComponent<Rigidbody2D>();
			BoxCollider2D bc = newEnemy.GetComponent<BoxCollider2D>();
			rb.AddForce(spawnForce*overallSpeed, ForceMode2D.Impulse);
			//bc.size = newEnemy.transform.localScale;
			Destroy(newEnemy, 1/overallSpeed);
		}
		else {
			tick += Time.fixedDeltaTime;
		}
    }
}
