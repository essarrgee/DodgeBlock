using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float speed;
	
	private Rigidbody2D rb;
	private Vector2 inputDir;

    void Awake()
    {
        speed = 8f;
		rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
		inputDir.y = Input.GetAxisRaw("Vertical");
		inputDir = inputDir.normalized;
    }
	
	void FixedUpdate() {
		rb.MovePosition(rb.position+inputDir*speed*Time.fixedDeltaTime);
	}
	
}
