using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float speed;
	
	private RigidBody2D rb;
	private Vector2 inputDir;
	
    // Start is called before the first frame update
    void Awake()
    {
        speed = 15f;
		
		rb = this.GetComponent<RigidBody2D>;
    }

    // Update is called once per frame
    void Update()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
		inputDir.y = Input.GetAxisRaw("Vertical");
		inputDir = inputDir.normalized;
		
		rb.MovePosition(inputDir);
    }
}
