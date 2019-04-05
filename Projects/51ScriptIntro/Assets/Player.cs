using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : MonoBehaviour
{
    public string Name;
    public float Hp;
    public float Mp;

    public void Run()
    {
        Debug.Log("跑步");
    }

    public void Attack()
    {
        Debug.Log("攻击");
    }

    void Start()
    {
        string student1 = "Jack";
        string student2 = "Lucy";
        string student3 = "Lilei";
        string student4 = "Tom";
        string student5 = "Ana";

        

        string[] students = { "jack", "lucy", "tom"};
        foreach (string student in students)
        {
        }
    }
}