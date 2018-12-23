using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShot : MonoBehaviour {

    public float ShotInterval = 1;
    AudioSource _source;
    float _lastShotTime = 0;

	// Use this for initialization
	void Start () {
        _source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1"))
        {
            if (Time.time - _lastShotTime > ShotInterval)
            {
                _source.Play();
                _lastShotTime = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (Time.time - _lastShotTime > ShotInterval)
            {
                _source.PlayOneShot(_source.clip);
                _lastShotTime = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (Time.time - _lastShotTime > ShotInterval)
            {
                AudioSource.PlayClipAtPoint(_source.clip, transform.position);
                _lastShotTime = Time.time;
            }
        }
    }
}
