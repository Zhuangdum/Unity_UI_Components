using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class DragEvent : UnityEvent<PointerEventData>
{
}

public class CameraController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float angle = 10;
    public DragEvent drageEvent = new DragEvent();
    public void OnBeginDrag(PointerEventData data)
    {
        
    }
    
    public void OnDrag(PointerEventData data)
    {
        if (drageEvent != null)
            drageEvent.Invoke(data);
    }
    
    public void OnEndDrag(PointerEventData data)
    {
        
    }

    public void HandleDrag(PointerEventData data)
    {
        Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, angle*data.delta.x);
    }
}
