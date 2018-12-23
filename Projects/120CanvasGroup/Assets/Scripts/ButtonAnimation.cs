using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    bool isRotating;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isRotating = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isRotating = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isRotating)
        {
            transform.Rotate(new Vector3(0, 0, 1) * -60 * Time.deltaTime);
        }
	}
}
