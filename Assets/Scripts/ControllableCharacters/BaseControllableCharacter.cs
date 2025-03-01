using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class BaseControllableCharacter : MonoBehaviour
{
    protected GridMap mapController;
    protected float moveX;
    protected float moveY;
    [SerializeField]protected float moveSpeed;
    protected static Vector2Int LookDirection;
    
    [SerializeField]protected static float maxHealth;
    public static float CurrentHealth{ get; protected set; }
    
    
    
    public static bool IsInvincible{ get; protected set; }

    protected static float InvincibleTimer;
    
    [SerializeField]protected float attackCD;
    protected float attackCDtimer;
    
    protected float Skill1CD;
    protected float Skill1CDtimer;

    public bool IsInvisible { get; protected set; }
    protected int MaxInvisibleTime;
    protected int InvisibleTimer;
    protected int InvisibleTimerRecoveryTimeRequired;
    protected int InvisibleTimerRecoveryTimer;
    
    public static bool IsControllable { get; protected set; } = true;
    protected static float ControllableTimer;
    
    [SerializeField]protected Animator animator;
    protected Rigidbody2D RigidBody;
    protected SpriteRenderer spriteRenderer;
    public Vector2Int originPosition;
    public Vector2Int CurrPosition { get; protected set; } = new (0,0);

    //protected bool IsInStep = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        mapController = FindObjectOfType<GridMap>();
        IsControllable = true;
        //CurrPosition = originPosition;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Control();
        if (attackCDtimer >= -0.3) attackCDtimer-=Time.deltaTime;
        if (ControllableTimer >= 0)
        {
            ControllableTimer-=Time.deltaTime;
            if (ControllableTimer <= 0) IsControllable = true;
        }

        if (Skill1CDtimer >= 0) Skill1CDtimer -= Time.deltaTime;
        
        animator.SetFloat("ControlSpeed", IsControllable ? Mathf.Abs(moveX):0);
        animator.SetFloat("AttackCDTimer",attackCDtimer);
    }
    protected virtual void Control()
    {
        if (!IsControllable) return;
        
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        RigidBody.velocity = new Vector2(moveSpeed * moveX, RigidBody.velocity.y);
         if (!Mathf.Approximately(moveX,0) )
         {
             LookDirection.Set(moveX>0?1:-1, moveY>0?1:-1);
         }
        // if(!Mathf.Approximately(moveX,0) || !Mathf.Approximately(moveY,0))
        // {
        //     LookDirection.Set(Mathf.Approximately(moveX,0)?0:moveX>0?1:-1, Mathf.Approximately(moveY,0)?0:moveY>0?1:-1);
        //     IsControllable = false;
        //     IsInStep = true;
        //     VoidMove();
        // }

        transform.localScale = new Vector2(LookDirection.x == 0 ? transform.localScale.x : LookDirection.x > 0 ? 1:-1,1);
    }

    // protected virtual void VoidMove()
    // {
    //     Vector2Int dir = new Vector2Int(Mathf.Approximately(moveX, 0) ? 0 : moveX > 0 ? 1 : -1,
    //         Mathf.Approximately(moveY, 0) ? 0 : moveY > 0 ? 1 : -1);
    //     
    //     Vector2Int target = CurrPosition + dir;
    //     if (mapController.TilePassable(target))
    //     {
    //         Vector3 des = transform.position + new Vector3(dir.x, dir.y, 0);
    //         // for (int i = 0; i < 100; i+=3)
    //         // {
    //         //     transform.position = math.lerp(transform.position,transform.position+new Vector3(dir.x*0.5f,dir.y*0.5f,0),i/100f);
    //         //     yield return null;
    //         // }
    //         StartCoroutine(AniMove(des));
    //         
    //         CurrPosition = target;
    //         // mapController.UpdateCostMap();
    //         // foreach (var enemy in FindObjectsOfType<BaseAICharacter>())
    //         // {
    //         //     enemy.Action();
    //         // }
    //         //Debug.Log(CurrPosition);
    //     }
    //     else
    //     {
    //         StartCoroutine(AfterMove());
    //     }
    //     
    // }
    // protected virtual IEnumerator AniMove(Vector3 des)
    // {
    //     
    //     transform.position = des;
    //     yield return AfterMove();
    //     // yield return new WaitForSeconds(0.2f);
    //     // IsControllable = true;
    //     //
    //     // yield return null;
    // }
    // protected virtual IEnumerator AfterMove()
    // {
    //     
    //     yield return new WaitForSeconds(0.2f);
    //     IsControllable = true;
    //     
    //     yield return null;
    // }
    public void ResetPosition()
    {
        CurrPosition = originPosition;
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("Attack");
        attackCDtimer = attackCD;
    }
    protected virtual void Skill1()
    {
        animator.SetTrigger("Skill1");
        Skill1CDtimer = Skill1CD;
        
    }
    public virtual void GetHurt(int damage)
    {
        
    }
    protected virtual void HealthChange(int delta)
    {
        CurrentHealth += delta;
        if (CurrentHealth < 0) CurrentHealth = 0;
        if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;
        Debug.Log("HP:"+CurrentHealth);
    }
}
