using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Server side code for controlling motion of automated non player
// characters
public class AutoNPCMotion : NetworkBehaviour {


	// Temp info for random walk
	private float howFarToMove = 0.0f;
	private Vector3 targetPos;
	private float startTime;
	private float speed = 4.0f;
	private Vector3 startPos;

	void Start () {
		ResetMotion ();
	}
		
	private void ResetMotion () {
		
		howFarToMove = Random.Range (3.0f, 7.0f);
	
		Vector3 dir = transform.forward * howFarToMove;
		targetPos = transform.position + dir;
		startTime = Time.time;
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			// Not sure this will ever get called, as object should be marked
			// server only. But, safety first.
			Debug.Log("Warning: Unexpected attempt at local player authority on NPC");
			return;
		}

		// Want to move in a random walk
		float distanceSoFar = (Time.time - startTime) * speed;
		float fractionTravelled = distanceSoFar / howFarToMove;

		if (fractionTravelled <= 1.0f) {
			transform.position = Vector3.Lerp (startPos, targetPos, fractionTravelled);
		} else {
			// Change direction
			transform.Rotate (0.0f, Random.Range (-90.0f, 90.0f), 0.0f);
			ResetMotion ();
		}
	}
}
