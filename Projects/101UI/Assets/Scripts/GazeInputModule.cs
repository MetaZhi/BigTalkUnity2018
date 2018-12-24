using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 使用方法
// 1. 将本脚本拖到EventSystem所在的物体上
// 2. 禁用其他Input Modules (例如：StandaloneInputModule)，否则会引起冲突
// 3. 确保Canvas是世界空间的3D UI并且包含GraphicRaycaster组件（默认）
public class GazeInputModule : PointerInputModule
{
    public enum Mode { Click = 0, Gaze };
    public Mode mode;

    [Header("Click Settings")]
    public string ClickInputName = "Submit";
    [Header("Gaze Settings")]
    public float GazeTimeInSeconds = 2f;

    public RaycastResult CurrentRaycast;

    private PointerEventData pointerEventData;
    private GameObject currentLookAtHandler;
    private float currentLookAtHandlerClickTime;

    public override void Process()
    {
        HandleLook();
        HandleSelection();
    }

    void HandleLook()
    {
        if (pointerEventData == null)
        {
            pointerEventData = new PointerEventData(eventSystem);
        }
        // 假设屏幕正中间有一个光标
        pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
        pointerEventData.delta = Vector2.zero;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, raycastResults);
        CurrentRaycast = pointerEventData.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
        ProcessMove(pointerEventData);
    }

    void HandleSelection()
    {
        if (pointerEventData.pointerEnter != null)
        {
            // 如果凝视目标切换了，重置计时器
            GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(pointerEventData.pointerEnter);
            if (currentLookAtHandler != handler)
            {
                currentLookAtHandler = handler;
                currentLookAtHandlerClickTime = Time.realtimeSinceStartup + GazeTimeInSeconds;
            }

            // 如果计时器时间够了，模拟执行点击事件
            if (currentLookAtHandler != null &&
                (mode == Mode.Gaze && Time.realtimeSinceStartup > currentLookAtHandlerClickTime) ||
                (mode == Mode.Click && Input.GetButtonDown(ClickInputName)))
            {
                ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerClickHandler);
                currentLookAtHandlerClickTime = float.MaxValue;
            }
        }
        else
        {
            currentLookAtHandler = null;
        }
    }
}