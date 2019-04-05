using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
本金（作为成员变量）存入银行，年利率作为成员变量，每年到期后将本息转存1年。

以下要求分别使用一个方法实现：
1、接收本金、年利率、存入的年限作为方法参数，计算并打印出(Debug.Log)每年过后的本息总和
2、年利率、本息倍数作为方法参数，打印出经过多少年，本息和可以达到本息倍数。比如本息倍数传入5，那就是计算经过多少年本息可以变为5倍。（使用循环计算）
*/

public class Caculate : MonoBehaviour
{

    public float balance;
    public float rate;
    public float multiple;

	// Use this for initialization
	void Start () {
        // CaculateInterest(balance, rate, 10);
		CaculateYears(rate, multiple);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 计算并打印利息
    void CaculateInterest(float balance, float rate, int year)
    {
        float total = balance;
        for (int i = 1; i <= year; i++)
        {
            total = total + total * rate;
            Debug.Log("第"+i+"年的本息总和是："+total);
        }
    }

    // 计算本金翻多少倍的年份
    void CaculateYears(float rate, float multiple)
    {
        if (rate <= 0)
        { 
            Debug.Log("年利率必须大于0");
            return;
        }

        float balance = 1;
        float target = balance * multiple;

        float total = balance;
        int year = 0;
        while (true)
        {
            year += 1;
            total = total + total * rate;
            if (total > target)
                break;
        }

        Debug.Log("在年利率为"+rate+"的情况下，经过"+year+"年， 本息总和会为初始本金的"+total+"倍");
    }
}
