using UnityEngine;

public class Dog
{
    public string Type; // 狗的品种
    public string Name; // 狗的名字

    public void Bark()
    {
        Debug.Log(Name + "：汪汪汪！！！");
    }
}
