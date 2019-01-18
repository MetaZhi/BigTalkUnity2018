using UnityEngine;
using UnityEngine.Experimental.VFX;

public class VFXEventSender : MonoBehaviour
{
    void Start()
    {
        var com = GetComponent<VisualEffect>();
        com.SendEvent("OnPlay");
    }
}
