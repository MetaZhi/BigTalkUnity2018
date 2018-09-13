using UnityEngine;
using UnityEngine.UI;

public class TestClick : MonoBehaviour {
    private Button _button;

    void Start () {
        _button = GetComponent<Button>();

        // 通过添加一个方法
        _button.onClick.AddListener(OnClick);

        // 通过匿名函数
        _button.onClick.AddListener(delegate ()
        {
            Debug.Log("delegate");
        });

        // 通过lambda表达式
        _button.onClick.AddListener(() =>
        {
            Debug.Log("delegate");
        });

        // 有时想传入一个参数，建议使用lambda的方式
        _button.onClick.AddListener(() => OnClick(_button.gameObject));
    }

    private void OnClick(GameObject gameObject)
    {
        Debug.Log("click!" + gameObject.name);
    }

    public void OnClick()
    {
        Debug.Log("click!");
    }
}
