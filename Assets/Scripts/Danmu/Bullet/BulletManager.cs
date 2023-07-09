﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Sprite[] SpriteType;

    public void ShootConfig(BulletData bulletData, BaseGameObjectPool pool)
    {
        //这个Num是用来控制角度的
        int num = bulletData.Count / 2;
        for (int i = 0; i < bulletData.Count; i++)
        {
            GameObject go = pool.Get(bulletData.Position, bulletData.LifeTime); //从对象池中获取
            Bullet bullet = go.GetComponent<Bullet>();

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

            if (Random.Range(0, 100) <= bulletData.SpecialChance)
            {
                go.GetComponent<Danmu>().gainEnergy = true;
                sr.sprite = SpriteType[(int)bulletData.bType+2];

            }
            else
            {
                go.GetComponent<Danmu>().gainEnergy = false;
                sr.sprite = SpriteType[(int)bulletData.bType];
            }


            bullet.BulletSpeed = bulletData.Speed;
            bullet.DelayTime = bulletData.DelayTime;

            if (bulletData.Count % 2 == 1)
            {
                if (bulletData.isParallel == false)
                {
                    go.transform.rotation = bulletData.direction * Quaternion.Euler(0, 0, bulletData.Angle * num);
                    go.transform.position = go.transform.position;
                }
                else
                {
                    go.transform.rotation = bulletData.direction;
                    go.transform.position = go.transform.position + go.transform.right * num * bulletData.parallelDistance;
                }


            }
            else
            {
                if (bulletData.isParallel == false)
                {
                    go.transform.rotation = bulletData.direction * Quaternion.Euler(0, 0, bulletData.Angle / 2 + bulletData.Angle * (num - 1));
                    go.transform.position = go.transform.position;
                }
                else
                {
                    go.transform.rotation = bulletData.direction;
                    go.transform.position = go.transform.position + go.transform.right * ((num - 1) * bulletData.parallelDistance + bulletData.parallelDistance / 2);
                }
            }

            num--;
            go.transform.position = go.transform.position + go.transform.up * bulletData.CenterDis;

            

        }
    }
}
