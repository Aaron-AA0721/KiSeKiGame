using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAICharacter : MonoBehaviour
{
    protected GridMap mapController;
    protected Vector2Int LookDirection;
    
    [SerializeField]protected float maxHealth;
    public float CurrentHealth{ get; protected set; }
    
    
    
    public bool IsInvincible{ get; protected set; }
    protected int InvincibleTimer;
    
    [SerializeField]protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    
    public Vector2Int CurrPosition { get; protected set; }
    public Vector2Int originPosition;
    protected virtual void Start()
    {
        CurrPosition = originPosition;
        mapController = FindObjectOfType<GridMap>();
        LookDirection = new(1, 0);
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CurrentHealth <= 0)
        {
            Death();
            return;
        }
        if(LookDirection.x * transform.localScale.x<0)
            transform.localScale = new Vector2(LookDirection.x>0?0.2f:-0.2f,0.2f);
        //animator.SetFloat("ControlSpeed", IsControllable ? Mathf.Abs(moveX):0);
    }
    
    public virtual void Action()
    {
        
    }
    protected virtual void Attack(int damage)
    {
        //Debug.Log("attack");
        mapController.player.GetHurt(damage);
    }
    public void ResetPosition()
    {
        CurrPosition = originPosition;
    }

    public virtual void GetHurt(int damage)
    {
        
    }
    protected virtual void HealthChange(int delta)
    {
        CurrentHealth += delta;
        if (CurrentHealth < 0) {
            CurrentHealth = 0;
            //Death();
        }
        if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;
    }
    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
