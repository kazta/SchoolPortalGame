using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class DropZone : MonoBehaviour, IDropHandler
{
    public ZoneType zoneType;
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
        }
    }
}
