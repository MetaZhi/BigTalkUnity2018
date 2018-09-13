using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDropdown : MonoBehaviour
{
    private Dropdown _dropdown;

    void Start()
    {
        _dropdown = GetComponent<Dropdown>();

        // 生成一个选项列表
        var options = new List<Dropdown.OptionData>();

        // 生成10个选项
        for (int i = 0; i < 10; i++)
        {
            options.Add(new Dropdown.OptionData(i.ToString()));
        }

        // 设置dropdown的选项
        _dropdown.options = options;


        // 下面是处理事件的三种方式
        _dropdown.onValueChanged.AddListener(OnDropdownChanged);

        _dropdown.onValueChanged.AddListener(delegate (int index) { Debug.Log(_dropdown.options[index].text); });

        _dropdown.onValueChanged.AddListener(index => { Debug.Log(_dropdown.options[index].text); });
    }

    private void OnDropdownChanged(int index)
    {
        Debug.Log(_dropdown.options[index].text);
    }
}
