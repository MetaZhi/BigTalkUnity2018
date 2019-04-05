using UnityEngine;

public class Test : MonoBehaviour
{
    private Renderer renderer;
    void Start()
    {
        Invoke("ShowBoss", 2);
        renderer = GetComponent<Renderer>();
    }

    void ShowBoss()
    {
        Debug.Log("2秒后显示boss");
    }

    void Update()
    {
        Debug.Log(Input.GetButton("Fire"));
    }
}
