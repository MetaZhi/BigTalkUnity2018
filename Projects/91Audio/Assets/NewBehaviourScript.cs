using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    private AudioSource _audioSource;

    // Use this for initialization
	void Start ()
	{
	    _audioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonUp(0))
	    {
	        _audioSource.PlayOneShot(_audioSource.clip);
	    }

	    if (Input.GetMouseButtonUp(1))
	    {
            _audioSource.Play();
	    }

	    if (Input.GetMouseButtonUp(2))
	    {
	        AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
	    }
    }
}
