using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadResource : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Background");
        Debug.Log(sprite);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
