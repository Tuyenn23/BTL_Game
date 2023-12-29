//using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public bool isDestroyWhenClosed = false;
    public bool isDisableWhenClosed = true;
    public bool isAutoSetSortingLayer = true;

    public Action ActionOpen;
    public Action ActionClose;

    protected bool isShow = false;

    private RectTransform _rect;
    private Stack<Action> _actionOpen;
    private Stack<Action> _actionClose;
    private RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }

            return _rect;
        }
    }
    protected virtual void Awake()
    {

    }
    public bool IsShow
    {
        get
        {
            return isShow;
        }
    }

    public virtual void Show(bool _isShown, bool isHideMain = true)
    {
        if (isShow == _isShown)
        {

            if (isShow)
            {
                if (isAutoSetSortingLayer)
                {
                    Rect.SetAsLastSibling();
                }
            }
            return;
        }

        isShow = _isShown;
        if (isShow)
        {
            if (isAutoSetSortingLayer)
            {
                Rect.SetAsLastSibling();
            }

            gameObject.SetActive(true);

            ActionOpen?.Invoke();

        }
        else
        {
            ActionClose?.Invoke();

            if (isDisableWhenClosed)
            {
                gameObject.SetActive(false);
            }
            else if (isDestroyWhenClosed)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }

    }
    public virtual void OnBackPressed()
    {
        Show(false);

    }
}
