using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Character97Controller : BaseControllableCharacter
{
    private bool IsInvisible;
    private int MaxInvisibleTime = 4;
    private int InvisibleTimer;
    private int InvisibleTimerRecoveryRate = 2;
    private int InvisibleTimerRecoveryTimer = 1;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    void SetAlpha()
    {
        foreach (var obj in transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>())
        {
            if (obj.name.Contains("lex"))
            {
                obj.color = new Color(obj.color.r,obj.color.g,obj.color.b,IsInvisible?0.9f:1f);
            }
            else obj.color = new Color(obj.color.r,obj.color.g,obj.color.b,IsInvisible?0.6f:1f);
        }
    }
}
