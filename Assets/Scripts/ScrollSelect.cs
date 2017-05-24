using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public List<Transform> _selectableList = new List<Transform>();
   public List<Vector3> _OrignPos = new List<Vector3>();
    int selectedIndex = 1;
    bool b_isDrag = false;

    public void Left()
    {
        if (selectedIndex >= _selectableList.Count - 1)
            return;
        ++selectedIndex;
    }

    public void Right()
    {
        if (selectedIndex - 1 < 0)
            return;
        --selectedIndex;
    }

    private void FixedUpdate()
    {
        if (b_isDrag == true)
        {
            return;
        }
        for (int i = 0; i < _selectableList.Count; i++)
        {
            _selectableList[i].localPosition = Vector3.Lerp(_selectableList[i].localPosition, new Vector3((selectedIndex - i) * -500, 0, 0), 0.1f);

            if (Mathf.Abs(_selectableList[i].localPosition.x) / 500 >= 1)
            {
                _selectableList[i].localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            else
            {
                float s = 1 - (Mathf.Abs(_selectableList[i].localPosition.x) % 500f) / 500f;
                s = s < 0.8f ? 0.8f : s;

                _selectableList[i].localScale = Vector3.one * s;
            }
        }
    }

    Vector3 lastPos;
    float offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPos = Input.mousePosition;
        b_isDrag = true;
        for (int i = 0; i < _selectableList.Count; i++)
        {
            _OrignPos[i] = _selectableList[i].localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        offset = Input.mousePosition.x - lastPos.x;
        for (int i = 0; i < _selectableList.Count; i++)
        {
            _selectableList[i].localPosition = _OrignPos[i] + new Vector3(offset, 0, 0);
            if (Mathf.Abs(_selectableList[i].localPosition.x) / 500 >= 1)
            {
                _selectableList[i].localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            else
            {
                float s = 1 - (Mathf.Abs(_selectableList[i].localPosition.x) % 500f) / 500f;
                s = s < 0.8f ? 0.8f : s;

                _selectableList[i].localScale = Vector3.one * s;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        b_isDrag = false;
        float closeValue = 1000;
        int closeindex = 0;
        for (int i = 0; i < _selectableList.Count; i++)
        {
            if (Mathf.Abs(_selectableList[i].localPosition.x) < closeValue)
            {
                closeValue = Mathf.Abs(_selectableList[i].localPosition.x);
                closeindex = i;
            }
        }
        selectedIndex = closeindex;
    }
    
}
