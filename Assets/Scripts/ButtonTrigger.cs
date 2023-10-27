using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTrigger : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    private Int32 _buttonNumber;
    private Vector2 _position;
    private Vector2 _dragDirection;

    private void Start()
    {
        _buttonNumber = Convert.ToInt32(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragDirection = eventData.position - _position;

        _dragDirection = _dragDirection.normalized;
        
        _dragDirection.x = Mathf.Abs(_dragDirection.x) >= 0.8f ? Mathf.Sign(_dragDirection.x) : 0;
        _dragDirection.y = Mathf.Abs(_dragDirection.y) >= 0.8f ? Mathf.Sign(_dragDirection.y) : 0;

        Debug.Log(_dragDirection);
        GameController.Instance.GetSquarePosition(_buttonNumber, _dragDirection.normalized);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _position = eventData.position;
    }
}
