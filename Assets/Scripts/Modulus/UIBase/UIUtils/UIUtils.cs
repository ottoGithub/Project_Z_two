﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum CommonBgType
{
    AnimBG_Rain,
    AnimBG_Star,
    AnimBG_Fog,
}

public class UIUtils
{
    public static GameObject cloneObj(GameObject go)
    {
        return MonoBehaviour.Instantiate(go) as GameObject;
    }

    public static GameObject cloneObj(GameObject go, Transform parent)
    {
        GameObject item = MonoBehaviour.Instantiate(go) as GameObject;
        item.transform.SetParent(parent);
        item.transform.localScale = new Vector3(1, 1, 1);
        item.SetActive(true);
        return item;
    }
    //获取屏幕高度
    public static float getScreenHeight()
    {
        return Screen.height;
    }

    public static void addCommonBg(BaseUI baseUI, CommonBgType type = CommonBgType.AnimBG_Star)
    {
        GameObject bg = ResMgr.Instance.load("UI/" + type.ToString()) as GameObject;
        if (bg != null)
        {
            bg.transform.SetParent(baseUI.CacheTrans);
            bg.transform.localPosition = Vector3.zero;
            bg.transform.localScale = Vector3.one;
            bg.transform.SetSiblingIndex(0);

            UIEventTrigger listener = bg.AddComponent<UIEventTrigger>();
            listener.setClickHandler(baseUI.closeSelfUI);
        }
    }

}

