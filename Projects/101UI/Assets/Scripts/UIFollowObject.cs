using UnityEngine;

public class UIFollowObject : MonoBehaviour
{
    public Transform Target;
    public Transform UI;

    void Update()
    {
        // viewport位置是左下角为(0,0)，范围是0-1
        var pos = Camera.main.WorldToViewportPoint(Target.position);
        // 转换到屏幕中心为(0,0)
        pos = pos - new Vector3(0.5f, 0.5f);
        // 获取canvas组件
        var canvas = UI.GetComponentInParent<Canvas>();
        // 获取canvas的尺寸
        var size = canvas.GetComponent<RectTransform>().sizeDelta;
        // 父物体是canvas时的local坐标
        var posInCanvas = new Vector3(pos.x * size.x, pos.y * size.y);
        // local坐标转为世界坐标
        var worldPos = canvas.transform.TransformPoint(posInCanvas);
        // 转为UI的父物体坐标系中的local坐标
        var localPos = UI.parent.InverseTransformPoint(worldPos);

        UI.localPosition = localPos;
    }
}
