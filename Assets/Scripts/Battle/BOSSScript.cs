using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOSSScript : MonoBehaviour
{
    [Header("BOSSѪ��")]
    public int maxBossHealth = 3;
    public int currentBossHealth;
    [Header("BOSS��Ļ��ʼʱ��")]
    public float danmuStartTime = 1.5f;
    public float startTimer = 0f;
    public bool isStarting = false;
    [Header("��Ļ������")]
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
            Debug.LogError("û�ҵ�BCM");
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

        //TODO:Ҫɾ���Ĳ���
        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("����");
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
        //�л���������һ��
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
        //�л�����
        GameObject.Find("Chara").GetComponent<PlyaerCtl>().Win();
    }
}
