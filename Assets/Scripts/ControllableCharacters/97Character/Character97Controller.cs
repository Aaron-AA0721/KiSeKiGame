using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Character97Controller : BaseControllableCharacter
{
    private bool AttackFlag;
    private bool AttackState = false;
    [SerializeField] private int Shadow_Num = 5;
    [SerializeField]private float Skill1Speed;
    [SerializeField]private float Skill1Duration;

    private List<GameObject> shadows = new List<GameObject>();
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
        
        
        attackCD = .7f;
        attackCDtimer = 0;
        
        
        Skill1CD = 1f;
        Skill1CDtimer = 0;
        Skill1Duration = 0.2f;
        Skill1Speed = 90f;

        // for (int i = 0; i < Shadow_Num; i++)
        // {
        //     GameObject shadow = Instantiate(gameObject);
        //     shadow.gameObject.SetActive(false);
        //     shadows.Add(shadow);
        // }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (attackCDtimer < 0)
        {
            attackCDtimer = 0;
            AttackState = false;
            animator.SetBool("AttackState", false);
        }
        
        if (Input.GetKeyDown(KeyCode.K) && IsControllable)
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && IsControllable)
        {
            Skill1();
        }
        
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

    // protected override void Control()
    // {
    //     base.Control();
    //     if (IsControllable && Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         IsInStep = true;
    //         if (IsInvisible)
    //         {
    //             ExitInvisibleState();
    //         }
    //         else
    //         {
    //             EnterInvisibleState();
    //         }
    //
    //     }
    //
    //     if (IsInStep)
    //     {
    //         IsInStep = false;
    //         
    //         if (IsInvisible)
    //         {
    //             InvisibleTimer -= 1;
    //             if (InvisibleTimer <= 0)
    //             {
    //                 ExitInvisibleState();
    //             }
    //         }
    //         else
    //         {
    //             if (InvisibleTimerRecoveryTimer > 0) InvisibleTimerRecoveryTimer--;
    //             if (InvisibleTimerRecoveryTimer <= 0)
    //             {
    //                 InvisibleTimer++;
    //                 if (InvisibleTimer > MaxInvisibleTime) InvisibleTimer = MaxInvisibleTime;
    //                 //Debug.Log(InvincibleTimer);
    //             }
    //         }
    //
    //         mapController.UpdateCostMap();
    //         foreach (var enemy in FindObjectsOfType<BaseAICharacter>())
    //         {
    //             enemy.Action();
    //         }
    //
    //         if (AttackFlag)
    //         {
    //             Attack();
    //             AttackFlag = false;
    //         }
    //         Debug.Log(InvisibleTimer);
    //     }
    //
    // }


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
        if (AttackState || (!AttackState && attackCDtimer<=0))
        {
            //Debug.Log(attackCDtimer);
            animator.SetBool("AttackState",AttackState);
            base.Attack();
            
            AttackState = !AttackState;
            Debug.Log(AttackState);
        }

        
        // mapController.AttackEnemy(CurrPosition, 1);
        // mapController.AttackEnemy(CurrPosition + LookDirection, 1);
    }
    protected override void Skill1()
    {
        if (Skill1CDtimer > 0) return;
        base.Skill1();
        IsControllable = false;
        ControllableTimer = Skill1Duration;
        //Debug.Log(LookDirection);
        RigidBody.velocity = new Vector2(0,0);
        RigidBody.velocity = new Vector2(-Skill1Speed*LookDirection.x,0);
        //RigidBody.AddForce(new Vector2(-Skill1Speed*LookDirection.x,0),ForceMode2D.Force);
        StartCoroutine(Skill1Process());
        
        Debug.Log(RigidBody.velocity);
    }

    IEnumerator Skill1Process()
    {
        RigidBody.velocity = new Vector2(-Skill1Speed*LookDirection.x,0);
        float t = 0;
        int ShadowCount = 0;
        while (t < Skill1Duration)
        {
            RigidBody.velocity = new Vector2(-Skill1Speed*LookDirection.x *(1-t / Skill1Duration),0);
            for (int i = 0;i<shadows.Count;i++)
            {
                foreach (var sp in shadows[i].GetComponentsInChildren<SpriteRenderer>())
                {
                    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a-0.4f*Time.deltaTime/Skill1Duration);
                }
            }
            if (t > ShadowCount * Skill1Duration / Shadow_Num)
            {
                GameObject shadow = Instantiate(gameObject);
                if(shadow.GetComponent<Character97Controller>())Destroy(shadow.GetComponent<Character97Controller>());
                if(shadow.GetComponent<Rigidbody2D>())Destroy(shadow.GetComponent<Rigidbody2D>());
                if(shadow.GetComponent<Collider2D>())Destroy(shadow.GetComponent<Collider2D>());
                if (shadow.GetComponentInChildren<Animator>()) shadow.GetComponentInChildren<Animator>().speed = 0;
                foreach (var sp in shadow.GetComponentsInChildren<SpriteRenderer>())
                {
                    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0.5f);
                    sp.sortingLayerName = "PlayerShadow";
                }

                ShadowCount++;
                shadows.Add(shadow);
                //     shadow.gameObject.SetActive(false);
                //     shadows.Add(shadow);
            }
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        while (t < Skill1Duration)
        {
            for (int i = 0;i<shadows.Count;i++)
            {
                foreach (var sp in shadows[i].GetComponentsInChildren<SpriteRenderer>())
                {
                    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a-0.5f*Time.deltaTime/Skill1Duration);
                }
            }
            t += Time.deltaTime;
            yield return null;
        }
        foreach (var shadow in shadows)
        {
            if(shadow)Destroy(shadow);
        }
        shadows.Clear();
    }
    // void EnterInvisibleState()
    // {
    //     if (InvisibleTimerRecoveryTimer > 0 || InvisibleTimer <= 0)
    //     {
    //         IsInStep = false;
    //         return;
    //     }
    //     InvisibleTimer++;
    //     IsInvisible = true;
    // }
    //
    // void ExitInvisibleState()
    // {
    //     IsInvisible = false;
    //     InvisibleTimerRecoveryTimer = InvisibleTimerRecoveryTimeRequired;
    //     AttackFlag = true;
    // }

    public override void GetHurt(int damage)
    {
        base.GetHurt(damage);
        HealthChange(-damage);
    }
}