using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public float launchSpeed;

	private Rigidbody rigidBody;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		Launch();
	}

	public void Launch() {
		rigidBody.velocity = new Vector3(0, 0, launchSpeed);
		audioSource.Play();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
