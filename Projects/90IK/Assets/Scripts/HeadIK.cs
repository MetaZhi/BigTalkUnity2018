using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadIK : MonoBehaviour {

    GameObject _debugSphere;
    Animator _animator;

	// Use this for initialization
	void Start () {
        _debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnAnimatorIK(int layerIndex)
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = 100;

        var pos = Camera.main.ScreenToWorldPoint(mousePosition);
        _debugSphere.transform.position = pos;

        _animator.SetLookAtPosition(pos);
        _animator.SetLookAtWeight(1);
    }
}
