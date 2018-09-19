using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            bool detected = Physics.Raycast(ray, out hit);
            
            if (detected)
            {
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Target")
                {
                    hit.collider.gameObject.SetActive(false);
                    StartCoroutine(TargetBack(hit.collider.gameObject));
                }
            }
        }
		
	}

    IEnumerator TargetBack(GameObject go)
    {
        yield return new WaitForSeconds(2);
        go.SetActive(true);
    }
}
