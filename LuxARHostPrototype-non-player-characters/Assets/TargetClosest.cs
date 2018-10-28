using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClosest : MonoBehaviour {
    public GameObject[] autoNpcs;
    public List<Transform> autoNpcTrans;
    public ParticleSeek ps;
    // Use this for initialization

    private void Awake()
    {

        autoNpcs = GameObject.FindGameObjectsWithTag("AutoNpc");

        foreach (var an in autoNpcs)
        {
            if (an.gameObject != gameObject)
            {
                autoNpcTrans.Add(an.transform);
            }
        }

    }

    void Start () {
       
      
    }
	
	// Update is called once per frame
	void Update () {

        
        ps.target = GetClosestEnemy(autoNpcTrans);

    }

    Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
