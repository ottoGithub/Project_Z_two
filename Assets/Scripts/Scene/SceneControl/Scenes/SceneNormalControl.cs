﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNormalControl : BaseSceneControl
{
    private int cryCount;
    private int monsterCount;

    public override void onAwake()
    {
        base.onAwake();
        MessageCenter.Instance.addListener(MsgCmd.Die_Main_Player, onMainPlayerDie);
        MessageCenter.Instance.addListener(MsgCmd.Die_Crystal_Entity, onCrystalDie);
        MessageCenter.Instance.addListener(MsgCmd.Die_Monster_Entity, onMonsterDie);
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.removeListener(MsgCmd.Die_Main_Player, onMainPlayerDie);
        MessageCenter.Instance.removeListener(MsgCmd.Die_Crystal_Entity, onCrystalDie);
        MessageCenter.Instance.removeListener(MsgCmd.Die_Monster_Entity, onMonsterDie);
    }

    public override void onStart()
    {
        if (this.info != null)
        {
            DDOLObj.Instance.StartCoroutine(createEntityCrystal(info.CrystalTime));
            //DDOLObj.Instance.StartCoroutine(createEntityMonster(info.AISpawnTimer));
            TimeMgr.Instance.setTimerHandler(new TimerHandler(info.AIWave * info.AISpawnTimer + 1, true, info.AISpawnTimer, createMonster, false));
        }
    }

    //创建怪
    protected void createMonster()
    {
        for (int i = 0; i < info.LstAI.Count; i++)
        {
            EntityMgr.Instance.createEntity(info.LstAI[i], uid);
            uid++;
        }
    }

    public override void onSetData()
    {
        this.cryCount = this.info.LstCrystal.Count;
        this.monsterCount = this.info.LstAI.Count * this.info.AIWave;
    }

    //玩家死亡
    private void onMainPlayerDie(Message msg)
    {
        //死亡游戏结束 败
        //弹出死亡界面 倒计时切换到 列表场景
        StartCoroutine(gameOverByFail(8f, "你已经死亡...准备重新开始游戏"));
    }
    //水晶都死亡
    private void onCrystalDie(Message msg)
    {
        //水晶全部死亡 游戏结束  败
        this.cryCount--;
        if (this.cryCount <= 0)
            StartCoroutine(gameOverByFail(8f, "水晶全部死亡...准备重新开始游戏"));
    }
    //怪物都死亡
    private void onMonsterDie(Message msg)
    {
        //怪物全部死亡 游戏结束 胜
        this.monsterCount--;
        if (this.monsterCount <= 0)
            StartCoroutine(gameOverBySuccess(8f, "游戏胜利...准备开始下一关卡"));
    }

    private IEnumerator gameOverByFail(float time, string str)
    {
        TimeMgr.Instance.removeALLTimerHanlder();
        EntityMgr.Instance.removeAllEntity();
        yield return new WaitForSeconds(time);
        SceneMgr.Instance.onLoadScene("GameStart", null, (progress) =>
        {
            DDOLCanvas.Instance.setFill(progress);
        }, true);
    }
    private IEnumerator gameOverBySuccess(float time, string str)
    {
        TimeMgr.Instance.removeALLTimerHanlder();
        EntityMgr.Instance.removeAllEntity();
        yield return new WaitForSeconds(time);
        int index = info.LevelIndex + 1;
        SceneMgr.Instance.onLoadScene("CrossFire" + index, null, (progress) =>
            {
                DDOLCanvas.Instance.setFill(progress);
            }, true);
    }

}

