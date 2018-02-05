﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinSetter : MonoBehaviour {
	public int lastStandingCount = -1;
	public Text standingDisplay;

	private Ball ball;
	private bool ballEnteredBox = false;
	private float lastChangeTime;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
	}
	
	// Update is called once per frame
	void Update () {
		standingDisplay.text = CountStanding().ToString();

		if (ballEnteredBox) {
			CheckStanding();
		}
	}

	// Update the lastStandingCount
	// Call PinsHaveSettled() when they have
	void CheckStanding() {
		int currentStanding = CountStanding();
		if (currentStanding != lastStandingCount) {
			lastChangeTime = Time.time;
			lastStandingCount = currentStanding;
			return;
		}
		float settleTime = 3f;	// How long to wait to consider pins settled
		if ((Time.time - lastChangeTime) > settleTime) {
			PinsHaveSettled();
		}
	}

	void PinsHaveSettled() {
		ball.Reset();
		lastStandingCount = -1;	// Indicates pins have settled, and ball not back in box
		ballEnteredBox = false;
		standingDisplay.color = Color.green;
	}

	int CountStanding() {
		int standing = 0;
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			if (pin.IsStanding()) {
				standing++;
			}
		}
		return standing;
	}

	void OnTriggerEnter(Collider collider) {
		GameObject thingHit = collider.gameObject;

		// Ball enters play box
		if (thingHit.GetComponent<Ball>()) {
			ballEnteredBox = true;
			standingDisplay.color = Color.red;
		}
	}

	void OnTriggerExit(Collider collider) {
		GameObject thingLeft = collider.gameObject;
		if (thingLeft.GetComponent<Pin>()) {
			Destroy(thingLeft);
		}
	}
}
