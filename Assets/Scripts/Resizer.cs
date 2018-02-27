using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resizer : MonoBehaviour {

    public float radius = 1.0f;
    public float depthRatio = 1.0f;

    public float conversionRatio = 1.0f;
    public float massInGrams = 1000.0f;

    private void OnValidate()
    {
        float circleScale = GetRadius() * 2.0f;
        float heightScale = GetDepth() / 2.0f;
        transform.localScale = new Vector3(circleScale,heightScale,circleScale);
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb)
        {
            rb.mass = massInGrams * 0.001f;
        }
        name = "Coin: " + radius + " units- " + conversionRatio + " meters per unit- " + depthRatio + " units per radius. Mass in g: " + massInGrams;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public float GetRadius()
    {
        return radius * conversionRatio;
    }

    public float GetDepth()
    {
        return GetRadius() * depthRatio;
    }
    public float GetMass()
    {
        return massInGrams;
    }
}
