﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMgr : Singleton<SpriteMgr>
{
    private Dictionary<string, Sprite> dictSp = null;

    public override void init()
    {
        dictSp = new Dictionary<string, Sprite>();
    }

    public Sprite getSprite(string name)
    {
        Sprite sp = null;
        if (dictSp.ContainsKey(name))
        {
            sp = dictSp[name];
        }
        else
        {
            sp = ResMgr.Instance.loadResByType<Sprite>("Textures/" + name);
            if (sp == null)
            {
                sp = ResMgr.Instance.loadResByType<Sprite>("Textures/uiDefault");
            }
            if (!dictSp.ContainsKey(sp.name))
                dictSp.Add(sp.name, sp);
        }
        return sp;
    }


}

