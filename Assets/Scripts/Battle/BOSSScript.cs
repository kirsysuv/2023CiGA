using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOSSScript : MonoBehaviour
{
    [Header("BOSS血量")]
    public int maxBossHealth = 3;
    public int currentBossHealth;
    [Header("BOSS弹幕起始时间")]
    public float danmuStartTime = 1.5f;
    public float startTimer = 0f;
    public bool isStarting = false;
    [Header("弹幕管理器")]
    public BulletControlManager BCM;
    private void Awake()
    {
        currentBossHealth = maxBossHealth;
    }
    void Start()
    {
        startTimer = 0f;
        BCM = GetComponentInChildren<BulletControlManager>();
        if (BCM == null)
        {
            Debug.LogError("没找到BCM");
        }
    }


    void Update()
    {
        if(!isStarting)
        {
            startTimer += Time.deltaTime;
            if (startTimer > danmuStartTime)
            {
                BossBattleStart();
                isStarting = true;
            }
        }

        //TODO:要删除的测试
        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("触发");
            DecreasedBossHealth();
        }
    }
    public void BossBattleStart()
    {
        BCM.ChangeBulletPhase();
    }
    public void DecreasedBossHealth()
    {
        currentBossHealth = currentBossHealth - 1;
        if (currentBossHealth > 0)
        {
            ChangePhase();
        }
        //切换场景到下一个
        else if (currentBossHealth == 0)
        {
            BossDead();
        }
    }

    public void ChangePhase()
    {
        int currentHP = maxBossHealth - currentBossHealth + 1;
        BCM.ChangeBulletPhase(currentHP);
    }
    public void BossDead()
    {
        BCM.CloseAllPhase();
        //切换场景
        GameObject.Find("Chara").GetComponent<PlyaerCtl>().Win();
    }
}
