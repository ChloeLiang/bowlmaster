using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinSetter : MonoBehaviour {
	public int lastStandingCount = -1;
	public Text standingDisplay;
	public float distanceToRaise = 40f;
	public GameObject pinSet;

	private Ball ball;
	private bool ballEnteredBox = false;
	private float lastChangeTime;
	private int lastSettledCount = 10;
	private ActionMaster actionMaster = new ActionMaster();
	private Animator animator;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		standingDisplay.text = CountStanding().ToString();
		//standingDisplay.text = CountStanding().ToString();
		if (ballEnteredBox) {
			UpdateStandingCountAndSettle();
		}
	}

	public void RaisePins() {
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			pin.RaiseIfStanding();
		}
	}

	public void LowerPins() {
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			pin.Lower();
		}
	}

	public void RenewPins() {
		Instantiate(pinSet, new Vector3(0, distanceToRaise, 1829), Quaternion.identity);
		foreach (Pin pin in GameObject.FindObjectsOfType<Pin>()) {
			pin.GetComponent<Rigidbody>().useGravity = false;
		}
	}

	// Update the lastStandingCount
	// Call PinsHaveSettled() when they have
	void UpdateStandingCountAndSettle() {
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
		int standing = CountStanding();
		int pinFall = lastSettledCount - standing;
		lastSettledCount = standing;

		ActionMaster.Action action = actionMaster.Bowl(pinFall);
		if (action == ActionMaster.Action.Tidy) {
			animator.SetTrigger("tidyTrigger");
		} else if (action == ActionMaster.Action.EndTurn) {
			animator.SetTrigger("resetTrigger");
			lastSettledCount = 10;
		} else if (action == ActionMaster.Action.Reset) {
			animator.SetTrigger("resetTrigger");
			lastSettledCount = 10;
		} else if (action == ActionMaster.Action.EndGame) {
			throw new UnityException("Don't know how to handle end game yet");
		}

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
}
