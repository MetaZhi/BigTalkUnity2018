using UnityEngine;
using UnityEngine.UI;

public class TestSlider : MonoBehaviour
{
    private Slider _scroll;

    void Start()
    {
        _scroll = GetComponent<Slider>();


        // 下面是处理事件的三种方式
        _scroll.onValueChanged.AddListener(OnSliderChanged);

        _scroll.onValueChanged.AddListener(delegate (float value) { Debug.Log(value); });

        _scroll.onValueChanged.AddListener(value => { Debug.Log(value); });
    }

    private void OnSliderChanged(float value)
    {
        Debug.Log(value);
    }
}
