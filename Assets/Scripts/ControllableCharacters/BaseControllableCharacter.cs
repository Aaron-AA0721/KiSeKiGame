using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class BaseControllableCharacter : MonoBehaviour
{
    protected float moveX;
    protected float moveY;
    [SerializeField]protected float moveSpeed;
    protected static Vector2 LookDirection;
    
    [SerializeField]protected static float maxHealth;
    public static float CurrentHealth{ get; protected set; }
    
    
    
    public static bool IsInvincible{ get; protected set; }

    protected static float InvincibleTimer;

    public static bool IsControllable { get; protected set; } = true;
    protected static float ControllableTimer;
    
    [SerializeField]protected Animator animator;

    protected SpriteRenderer spriteRenderer;
    
    public Vector2Int CurrPosition { get; protected set; } = new (0,0);
    // Start is called before the first frame update
    protected virtual void Start()
    {
        IsControllable = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Control();
        //animator.SetFloat("ControlSpeed", IsControllable ? Mathf.Abs(moveX):0);
    }
    protected virtual void Control()
    {
        if (!IsControllable) return;
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        //RigidBody.velocity = new Vector2(moveSpeed * moveX, RigidBody.velocity.y);
        if (!Mathf.Approximately(moveX,0) )
        {
            LookDirection.Set(moveX, moveY);
        }
        if(!Mathf.Approximately(moveX,moveY) )
        {
            IsControllable = false;
            StartCoroutine(Move());
        }
        transform.localScale = new Vector2(LookDirection.x>=0?1:-1,1);
    }

    IEnumerator Move()
    {
        Vector2Int dir = new Vector2Int(Mathf.Approximately(moveX, 0) ? 0 : moveX > 0 ? 1 : -1,
                 Mathf.Approximately(moveY, 0) ? 0 : moveY > 0 ? 1 : -1);
        Debug.Log(CurrPosition);
        Vector2Int target = CurrPosition + dir;
        if (GridMap.TilePassable(target))
        {
            Vector3 des = transform.position + new Vector3(dir.x, dir.y, 0);
            // for (int i = 0; i < 100; i+=3)
            // {
            //     transform.position = math.lerp(transform.position,transform.position+new Vector3(dir.x*0.5f,dir.y*0.5f,0),i/100f);
            //     yield return null;
            // }
            transform.position = des;
            CurrPosition = target;
        }

        yield return new WaitForSeconds(0.5f);
        IsControllable = true;
        
        yield return null;
    }
}
