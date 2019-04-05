using UnityEngine;

public class Animal
{
    public string Name;

    public virtual void Shout()
    {
        Debug.Log("我不会叫");
    }
}