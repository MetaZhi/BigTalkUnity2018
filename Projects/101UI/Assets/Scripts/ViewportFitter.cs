using UnityEngine;

public class ViewportFitter : MonoBehaviour
{

    public float AspectRatio = 16 / 9f;
    private Camera _camera;

    float _lastAspect;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.aspect = AspectRatio;
    }

    void Update()
    {
        var aspect = Screen.width / (float)Screen.height;
        if (aspect == _lastAspect)
            return;

        _lastAspect = aspect;

        if (aspect > AspectRatio) // 当前比较宽，需要两边留黑
        {
            var actualWidth = Screen.height * AspectRatio;
            var viewportWidth = actualWidth / Screen.width;
            var viewportX = (1 - viewportWidth) / 2;

            _camera.rect = new Rect(viewportX, 0, viewportWidth, 1);
        }
        else// 当前比较窄，需要上下留黑
        {
            var actualHeight = Screen.width / AspectRatio;
            var viewportHeight = actualHeight / Screen.height;
            var viewportY = (1 - viewportHeight) / 2;

            _camera.rect = new Rect(0, viewportY, 1, viewportHeight);
        }
    }
}
