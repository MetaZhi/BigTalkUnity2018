using UnityEngine;

public class Cat : Animal
{
    public override void Shout()
    {
        Debug.Log("我是小猫" + Name + " 喵喵喵");
    }
}