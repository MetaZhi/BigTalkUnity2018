using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour {

    public float speed = 3;

    Vector3 _startPos;

	// Use this for initialization
	void Start () {
        _startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 velocity = new Vector3(h, 0, v).normalized * speed;

        transform.position += velocity * Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(RestorePosition());
        }		
	}

    IEnumerator RestorePosition()
    {
        var dir = (_startPos - transform.position).normalized;
        var velocity = dir * speed;

        while (true)
        {
            if (Vector3.Distance(transform.position, _startPos) < speed * Time.deltaTime)
            {
                yield break;
            }

            transform.position += velocity * Time.deltaTime;
            yield return null;
        }
    }
}
