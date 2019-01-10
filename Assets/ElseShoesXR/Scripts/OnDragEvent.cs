using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OnDragEvent : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

    public SelectionCircle mySelectionCircle;
    public ScrollRect sr;
    public void OnBeginDrag(PointerEventData eventData)
    {
        mySelectionCircle.isDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(DelayedDragStop());
    }

    IEnumerator DelayedDragStop()
    {
        yield return new WaitForSeconds(.5f);
        mySelectionCircle.isDragged = false;
    }
}
