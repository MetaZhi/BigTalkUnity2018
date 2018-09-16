using UnityEngine;
using UnityEngine.UI;

public class TestInputfieldValueChange : MonoBehaviour
{
    InputField _inputField;
    void Start()
    {
        _inputField = GetComponent<InputField>();

        _inputField.onValueChanged.AddListener(OnValueChage);

        _inputField.onValueChanged.AddListener(OnValueChage);
        _inputField.onValueChanged.AddListener(delegate (string text) { Debug.Log(text); });
        _inputField.onValueChanged.AddListener(text => { Debug.Log(text); });
    }

    private void OnValueChage(string text)
    {
        Debug.Log(text);
    }
}
