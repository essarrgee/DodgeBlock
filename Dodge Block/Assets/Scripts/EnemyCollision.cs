using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
	public Manager GameManager;
	
	void Awake() {
		GameManager = 
			GameObject.Find("GameManager").GetComponent<Manager>();
		Physics.IgnoreLayerCollision(0,8);
		Physics.IgnoreLayerCollision(8,8);
	}
	
    void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			bool canHurt = GameManager.HurtPlayer();
			if (canHurt) {
				Destroy(gameObject);
			}
		}
	}
}
