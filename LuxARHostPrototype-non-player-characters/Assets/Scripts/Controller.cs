using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player
{
	public int ID { get ; set; }
	public GameObject avatar { get; set; }

	// Temp - array of positions we can to drive this object
	public const int NPTS = 100;
	public Vector3[] tpath = new Vector3[NPTS];

	private int nextPos = 0;

	public Player () {
		// Generate a path
		float radius = (Random.value * 15f) + 5f;  // random radius
		float theta = 0f;
		float thetadelta = (Mathf.PI * 2f) / (float) NPTS;

		// Randomise direction
		if (Random.value > 0.5f) {
			thetadelta *= -1f;
		}
		for (int i = 0; i < NPTS; i++, theta += thetadelta) {
			tpath [i].x = radius * Mathf.Sin (theta);
			tpath [i].z = radius * Mathf.Cos (theta);
		}
	}

	// Sets origin for path vector (just for testing variety)
	public void SetOrigin (Vector3 origin)
	{
		for (int i = 0; i < NPTS; i++) {
			tpath [i] += origin;
		}
	}

	// Move avatar to next stored position, wrapping to first if needed.
	public void UpdatePosition () {
		avatar.transform.position = tpath[nextPos++];
		if (nextPos == NPTS) {
			nextPos = 0;
		}
	}
}


public class Controller : MonoBehaviour {

	public GameObject prefab;

	// Keep hold of list of players as we create them.
	private List<Player> players;
	private int nextID = 1;

	// Use this for initialization
	void Start () {
		if (prefab == null) {
			Debug.LogError ("Prefab not found.");
		}

		players = new List<Player>();
	}

	// Update is called once per frame
	void Update () {

		// Move all the players
		foreach (Player p in players) {
			p.UpdatePosition ();
		}

		// If space key hit, then spawn a new player at mouse position
		if (Input.GetKeyDown (KeyCode.Space)) {
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) {
				Player newPlayer = new Player ();
				newPlayer.ID = nextID++;
				newPlayer.avatar = Spawn (hit.point);
				newPlayer.SetOrigin (hit.point);

				if (newPlayer.avatar) {
					players.Add (newPlayer);
					Debug.Log ("Player " + newPlayer.ID + " created.");
					// Randomise particle start colour
					newPlayer.avatar.GetComponent<PlayerMove>().SetColor (Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f));
				}
			}
		}

		// Change target location if T key hit
		if (Input.GetKeyDown(KeyCode.T)) {
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {

				//players.ForEach
				foreach (Player x in players) {
					PlayerMove p = x.avatar.GetComponent<PlayerMove>();
					if (p){
//						p.goal.position = hit.point;
						p.ResetMotion ();
					}
				}
			}
		}
	}

	GameObject Spawn (Vector3 spawnPos)
	{
		if (prefab) {
			Debug.Log ("Instantiating player...");
			GameObject newPlayer = GameObject.Instantiate (prefab, spawnPos, Quaternion.identity);

//			newPlayer.GetComponent<PlayerMove>().goal = (Transform) GameObject.Find ("Target").transform;

//			if (newPlayer.GetComponent<PlayerMove>().goal == null) {
				Debug.LogError ("Target not found for new player");
//			}
			return newPlayer;
		} else {
			Debug.LogError ("Player Prefab not found");
			return null;
		}
	}
}
