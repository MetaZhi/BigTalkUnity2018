using UnityEngine;
using UnityEngine.UI;

public class TestScrollRect : MonoBehaviour
{
    private ScrollRect _scroll;

    void Start()
    {
        _scroll = GetComponent<ScrollRect>();


        // 下面是处理事件的三种方式
        _scroll.onValueChanged.AddListener(OnDropdownChanged);

        _scroll.onValueChanged.AddListener(delegate (Vector2 pos) { Debug.Log(pos); });

        _scroll.onValueChanged.AddListener(pos => { Debug.Log(pos); });
    }

    private void OnDropdownChanged(Vector2 pos)
    {
        Debug.Log(pos);
    }
}
