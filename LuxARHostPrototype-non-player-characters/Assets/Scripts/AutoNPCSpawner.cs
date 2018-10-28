using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoNPCSpawner : NetworkBehaviour {
	public GameObject autoNPCPrefab;
    public List<Transform> transforms;
	public int numberOfNPCs;

    public int seekerStrength;
    public float trailsLifeTime;

	public override void OnStartServer()
	{
		for (int i = 0; i < numberOfNPCs; i++) {
			var spawnPosition = new Vector3 (
				                    Random.Range (-40.0f, 40.0f),
				                    0.0f,
				                    Random.Range (-40.0f, 40.0f));
			var spawnRotation = Quaternion.Euler (
				                    0.0f,
				                    Random.Range (0, 180),
				                    0.0f);
			var npc = (GameObject.Instantiate (autoNPCPrefab, spawnPosition, spawnRotation));
			NetworkServer.Spawn (npc);

            //setup seeker strength
            //npc.GetComponentInChildren<ParticleSeek>().force = seekerStrength;
            transforms.Add(npc.transform);
		}
	}

    private void LateUpdate()
    {
        foreach (var an in transforms) {
            an.GetComponentInChildren<ParticleSeek>().force = seekerStrength;

            ////an.GetComponentInChildren<ParticleSeek>().target = GetClosestEnemy(transforms);
            //var ps = an.GetComponentInChildren<ParticleSystem>();
            ////ps.trails.lifetime = trailsLifeTime;
            //ParticleSystem.MainModule psmain = ps.main;
            //psmain.startLifetime = trailsLifeTime;
        }

    }


   
}


