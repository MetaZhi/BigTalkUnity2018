using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour {

	// Use this for initialization
	void Start () {


        var anim = new Animal();
        anim.Name = "佚名";

        anim.Shout();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cat cat = new Cat();
            cat.Name = "小花";

            cat.Shout();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var dog = new Dog();
            dog.Name = "旺财";

            dog.Shout();

        }


    }
}
