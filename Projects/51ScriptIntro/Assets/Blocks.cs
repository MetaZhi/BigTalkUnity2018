using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    private float width;
    public Dictionary<string, Type> consumableToScript = new Dictionary<string, Type> { { "LevelPoint", typeof(CaculateInterest) } };

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
