using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAICharacter : MonoBehaviour
{
    
    protected static Vector2 LookDirection;
    
    [SerializeField]protected static float maxHealth;
    public static float CurrentHealth{ get; protected set; }
    
    
    
    public static bool IsInvincible{ get; protected set; }
    protected static int InvincibleTimer;
    
    
    protected Rigidbody2D RigidBody;
    [SerializeField]protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    
    public Vector2Int CurrPosition { get; protected set; } = new (0,0);
    protected Vector2Int direction = new(0, 0);
    protected virtual void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        //animator.SetFloat("ControlSpeed", IsControllable ? Mathf.Abs(moveX):0);
    }
    protected virtual void Move()
    {
        
    }
}
