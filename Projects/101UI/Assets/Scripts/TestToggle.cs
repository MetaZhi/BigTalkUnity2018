using UnityEngine;
using UnityEngine.UI;

public class TestToggle : MonoBehaviour
{
    private Toggle _toggle;

    void Start()
    {
        _toggle = GetComponent<Toggle>();

        // 通过添加一个方法
        _toggle.onValueChanged.AddListener(OnToggleChanged);

        // 通过匿名函数
        _toggle.onValueChanged.AddListener(delegate (bool b) { Debug.Log(b); });

        // 通过lambda表达式
        _toggle.onValueChanged.AddListener(b => { Debug.Log(b); });
    }

    private void OnToggleChanged(bool arg0)
    {
        Debug.Log(arg0);
    }
}
