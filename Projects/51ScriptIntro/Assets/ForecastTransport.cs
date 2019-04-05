using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForecastTransport : MonoBehaviour {
    // 华氏温度
    public float Fahrenheit;
    public bool willRain;

    // Use this for initialization
    void Start ()
    {
        float temp = (Fahrenheit - 32) * 5 / 9;
        Debug.Log("摄氏温度是：" + temp);

        if (willRain)
        {
            Debug.Log("交通工具选择：打车");
        }
        else
        {
            if (temp < 10 || temp > 30)
            {
                Debug.Log("交通工具选择：公交");
            }
            else if(temp < 20)
            {
                Debug.Log("交通工具选择：骑单车");
            }
            else
            {
                Debug.Log("交通工具选择：步行");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
