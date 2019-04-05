using UnityEngine;

public class ExecuteOrder : MonoBehaviour {

    void Awake()
    {
        Debug.Log("Awake");
    }

    void Start () {
	}

        /* 
    中间是注释内容
    可以有多行 
    */
        void Update () {
	    Debug.Log("Update");
    }
    void LateUpdate()
    {
        Debug.Log("LateUpdate");
    }
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
    }
}
