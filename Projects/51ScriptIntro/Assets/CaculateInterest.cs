using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaculateInterest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        float principal = 10000; // 本金
        float interestRate = 0.03f; // 利率，别忘了最后的f
        int currentYear = 1;
        while(true)
        {
            float interest = principal * interestRate; // 第一年利息
            float total = principal + interest; // 第一年本息总和

            Debug.Log("第" + currentYear + "年，利息为" + interest + ", 本息总和为：" + total);
            if (total >= 20000)
                break;

            principal = total; // 下一年本息总和变为本金
            currentYear++;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}