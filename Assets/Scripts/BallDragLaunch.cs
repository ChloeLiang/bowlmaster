﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Ball))]
public class BallDragLaunch : MonoBehaviour {
	private Ball ball;
	private Vector3 dragStart, dragEnd;
	private float startTime, endTime;

	// Use this for initialization
	void Start () {
		ball = GetComponent<Ball>();
	}

	public void MoveStart(float amount) {
		if (!ball.inPlay) {
			ball.transform.Translate(new Vector3(amount, 0, 0));
		}

	}

	// Capture time & position of drag start
	public void DragStart() {
		dragStart = Input.mousePosition;
		startTime = Time.time;
	}

	// Launch the ball
	public void DragEnd() {
		dragEnd = Input.mousePosition;
		endTime = Time.time;

		float dragDuration = endTime - startTime;
		float launchSpeedX = (dragEnd.x - dragStart.x) / dragDuration;
		float launchSpeedZ = (dragEnd.y - dragStart.y) / dragDuration;

		Vector3 launchVelocity = new Vector3(launchSpeedX, 0, launchSpeedZ);
		ball.Launch(launchVelocity);
	}
}
