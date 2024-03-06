using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基础属性")] 
    public float MaxHealth;

    public float CurrentHealth;
    

    [Header("计时器")] 
    [Tooltip("无敌时间")]
    public float InvincibleTime;

    private float InvincibleCounter;

    public bool IfInvincible;

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDeath;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (IfInvincible)
        {
            InvincibleCounter -= Time.deltaTime;
            if (InvincibleCounter<=0)
            {
                IfInvincible = false;
            }
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (IfInvincible)
        {
            return;
        }

        if (CurrentHealth>attacker.Damage)
        {
            CurrentHealth -= attacker.Damage;
            TriggerInvincible();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            CurrentHealth = 0;
            //触发死亡机制
            OnDeath?.Invoke();
        }
        
    }

    private void TriggerInvincible()
    {
        if (!IfInvincible)
        {
            IfInvincible = true;
            InvincibleCounter = InvincibleTime;
        }
    }
}
