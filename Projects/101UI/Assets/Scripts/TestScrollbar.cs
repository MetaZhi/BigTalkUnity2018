using UnityEngine;
using UnityEngine.UI;

public class TestScrollBar : MonoBehaviour
{
    private Scrollbar _scroll;

    void Start()
    {
        _scroll = GetComponent<Scrollbar>();


        // 下面是处理事件的三种方式
        _scroll.onValueChanged.AddListener(OnScrollChanged);

        _scroll.onValueChanged.AddListener(delegate (float pos) { Debug.Log(pos); });

        _scroll.onValueChanged.AddListener(pos => { Debug.Log(pos); });
    }

    private void OnScrollChanged(float pos)
    {
        Debug.Log(pos);
    }
}
