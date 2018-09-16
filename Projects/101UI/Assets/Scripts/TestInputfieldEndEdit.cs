using UnityEngine;
using UnityEngine.UI;

public class TestInputfieldEndEdit : MonoBehaviour
{
    InputField _inputField;
    void Start()
    {
        _inputField = GetComponent<InputField>();

        _inputField.onEndEdit.AddListener(OnEndEdit);
        _inputField.onEndEdit.AddListener(delegate (string text) { Debug.Log(text); });
        _inputField.onEndEdit.AddListener(text => { Debug.Log(text); });
    }

    private void OnEndEdit(string text)
    {
        Debug.Log(text);
    }
}
