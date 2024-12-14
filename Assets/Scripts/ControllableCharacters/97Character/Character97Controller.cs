using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Character97Controller : BaseControllableCharacter
{
    private bool AttackFlag;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        maxHealth = 2;
        CurrentHealth = maxHealth;
        IsInvisible = false;
        MaxInvisibleTime = 5;
        InvisibleTimerRecoveryTimeRequired = 2;
        InvisibleTimerRecoveryTimer = -1;
        InvisibleTimer = MaxInvisibleTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // if (Input.GetKey(KeyCode.LeftShift) && InvisibleTimer >0 && InvisibleTimerRecoveryTimer<=0)
        // {
        //     //InvisibleTimer -= Time.deltaTime;
        //     IsInvisible = true;
        // }
        // else
        // {
        //     if (IsInvisible)
        //     {
        //         IsInvisible = false;
        //         InvisibleTimerRecoveryTimer = 0.4f;
        //         Attack();
        //     }
        // }
        // if (!IsInvisible)
        // {
        //     InvisibleTimerRecoveryTimer -= Time.deltaTime;
        //     if (InvisibleTimerRecoveryTimer <= 0.2)
        //     {
        //          InvisibleTimer += Time.deltaTime * InvisibleTimerRecoveryRate; 
        //          InvisibleTimer = Mathf.Min(InvisibleTimer, MaxInvisibleTime);
        //     }
        //    
        // }
        //Debug.Log(InvisibleTimer);
        SetAlpha();
    }

    protected override void Control()
    {
        base.Control();
        if (IsControllable && Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsInStep = true;
            if (IsInvisible)
            {
                ExitInvisibleState();
            }
            else
            {
                EnterInvisibleState();
            }

        }

        if (IsInStep)
        {
            IsInStep = false;
            
            if (IsInvisible)
            {
                InvisibleTimer -= 1;
                if (InvisibleTimer <= 0)
                {
                    ExitInvisibleState();
                }
            }
            else
            {
                if (InvisibleTimerRecoveryTimer > 0) InvisibleTimerRecoveryTimer--;
                if (InvisibleTimerRecoveryTimer <= 0)
                {
                    InvisibleTimer++;
                    if (InvisibleTimer > MaxInvisibleTime) InvisibleTimer = MaxInvisibleTime;
                    //Debug.Log(InvincibleTimer);
                }
            }

            mapController.UpdateCostMap();
            foreach (var enemy in FindObjectsOfType<BaseAICharacter>())
            {
                enemy.Action();
            }

            if (AttackFlag)
            {
                Attack();
                AttackFlag = false;
            }
            Debug.Log(InvisibleTimer);
        }

    }


    void SetAlpha()
    {
        foreach (var obj in transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
        {
            if (obj.name.Contains("lex"))
            {
                obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, IsInvisible ? 0.9f : 1f);
            }
            else obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, IsInvisible ? 0.6f : 1f);
        }
    }

    protected override void Attack()
    {
        base.Attack();
        mapController.AttackEnemy(CurrPosition, 1);
        mapController.AttackEnemy(CurrPosition + LookDirection, 1);
    }

    void EnterInvisibleState()
    {
        if (InvisibleTimerRecoveryTimer > 0 || InvisibleTimer <= 0)
        {
            IsInStep = false;
            return;
        }
        InvisibleTimer++;
        IsInvisible = true;
    }

    void ExitInvisibleState()
    {
        IsInvisible = false;
        InvisibleTimerRecoveryTimer = InvisibleTimerRecoveryTimeRequired;
        AttackFlag = true;
    }

    public override void GetHurt(int damage)
    {
        base.GetHurt(damage);
        HealthChange(-damage);
    }
}