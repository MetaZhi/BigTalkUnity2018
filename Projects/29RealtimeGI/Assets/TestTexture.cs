using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTexture : MonoBehaviour
{
    public Texture tex;

	// Use this for initialization
    void Start()
    {
        var child = transform.GetChild(0);
        child.GetComponent<Renderer>().material.mainTexture = tex;

        
    }

    void Update()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            var mmm = child.gameObject.GetComponent<Renderer>();

            if (i == 0)
            {
                mmm.material.mainTexture = tex;

            }

            i++;
        }
    }
}
