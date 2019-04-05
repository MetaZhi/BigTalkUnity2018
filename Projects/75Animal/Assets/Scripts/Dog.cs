using UnityEngine;

public class Dog : Animal
{
    public override void Shout()
    {
        Debug.Log("我是小狗" + Name + " 汪汪汪");
    }
}