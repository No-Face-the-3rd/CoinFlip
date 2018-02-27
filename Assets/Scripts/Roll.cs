using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RollState
{
    Tails, Edge, Heads
}

public class Roll : MonoBehaviour {

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 Rot = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        transform.rotation = Quaternion.Euler(Rot);
        float flipStrength = Random.Range(0.5f, 25.0f);
        float yVal = Random.Range(0.1f, 1.0f);
        float ang = Random.Range(0.0f, 2.0f * Mathf.PI);
        float xVal = Mathf.Sqrt(1.0f - Mathf.Pow(yVal, 2.0f)) * Mathf.Cos(ang);
        float zVal = Mathf.Sqrt(1.0f - Mathf.Pow(yVal, 2.0f)) * Mathf.Sin(ang);
        Vector3 flipDir = new Vector3(xVal, yVal, zVal);
        xVal = Random.Range(-1.0f, 1.0f);
        yVal = -1.0f;
        zVal = Random.Range(-1.0f, 1.0f);
        rb.AddForceAtPosition(flipDir * flipStrength,Vector3.zero,ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public RollState GetRollstate()
    {
        float dotState = Vector3.Dot(transform.up, Vector3.up);
        int dotStateTrunc = Mathf.RoundToInt(dotState);
        return (RollState)(dotStateTrunc + 1);
    }
}
