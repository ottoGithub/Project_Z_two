﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDetailControl : BaseControl
{
    public override void initEnum()
    {
        this.uiEnum = UIEnum.skillDetailUI;
    }

    public override void initListener()
    {
        MessageCenter.Instance.addListener(MsgCmd.Open_SkillDetail_UI, openUI);
    }

    private void openUI(Message msg)
    {
        bool isOpen = UIMgr.Instance.isOpen(this.uiEnum);
        if (isOpen)
            closeUI();
        else
            openUI();
    }

    private void openUI()
    {
        EntityDynamicActor dy = EntityMgr.Instance.getMainPlayer() as EntityDynamicActor;
        if (dy == null || dy.getSkillWidget() == null) return;
        List<int> skills = dy.getSkillWidget().getSkillInfo();

        SkillDetailData dt = new SkillDetailData();
        for (int i = 0; i < skills.Count; i++)
        {
            SkillConfigConfig conf = SkillConfigConfig.Get(skills[i]);
            if (conf != null)
            {
                SkillDetailItemData info = new SkillDetailItemData();
                info.id = conf.tempId;
                info.skillIcon = conf.skillIcon;
                info.skillName = conf.skillName;
                info.skillDesc = conf.skillDesc;
                info.skillModeType = getModeType(conf.skillModeType);
                info.atkType = getAtkType(conf.atkType);
                info.atkRange = conf.atkRange;
                info.horAngle = conf.horAngle;
                info.verAngle = conf.verAngle;
                info.skillDamage = conf.skillDamage;
                dt.lst.Add(info);
            }
        }
        this.updateUI(dt);
    }
    private void closeUI()
    {
        UIMgr.Instance.closeUI(this.uiEnum);
    }

    private string getModeType(int type)
    {
        string str = "";
        SkillModeType t = (SkillModeType)type;
        switch (t)
        {
            case SkillModeType.baseSkill:
                str = "基础技能";
                break;
            case SkillModeType.normalSkill:
                str = "普通技能";
                break;
            case SkillModeType.triggerSkill:
                str = "触发技能";
                break;
            case SkillModeType.dodgeSkill:
                str = "闪避技能";
                break;
        }
        return str;
    }
    private string getAtkType(int type)
    {
        string str = "";
        AttackType t = (AttackType)type;
        switch (t)
        {
            case AttackType.normal:
                str = "普通攻击";
                break;
            case AttackType.hitBack:
                str = "击退攻击";
                break;
            case AttackType.hitAir:
                str = "浮空攻击";
                break;
        }
        return str;
    }
}
