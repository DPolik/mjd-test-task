using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler
{
    private Vector3 lastDragPosition;
    private Vector3 beginDragPosition;

    [SerializeField] private float dragDistanceThreshold = 30;

    public static event Action<int> onDrag;

    public void OnDrag(PointerEventData eventData)
    {
        var delta = eventData.delta;
        if(Mathf.Abs(delta.x) > dragDistanceThreshold)
        {
            if(delta.x < 0)
            {
                onDrag.Invoke(1);
            }
            else
            {
                onDrag.Invoke(-1);
            }
        }
    }

}
