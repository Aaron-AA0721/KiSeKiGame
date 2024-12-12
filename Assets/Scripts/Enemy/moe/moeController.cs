using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class moeController : BaseAICharacter
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public override void Action()
    {
        base.Action();
        if (mapController.player.IsInvisible)
        {
            List<Vector2Int> target = new List<Vector2Int>();
            target.Add(new (1,0));
            target.Add(new (-1,0));
            target.Add(new (0,1));
            target.Add(new (0,-1));
            int i = 4;
            int ran = Random.Range(0, i);
            while ((!mapController.WithinMapBoundary(CurrPosition.x + target[ran].x,CurrPosition.y + target[ran].y) 
                    || mapController.gridMap[CurrPosition.x + target[ran].x, CurrPosition.y + target[ran].y].IsBlocked )
                   && i>0)
            {
                target.Remove(target[ran]);
                i--;
                ran = Random.Range(0, i);
            }

            if (i != 0)
            {
                LookDirection = target[ran];
                CurrPosition += target[ran];
                transform.position += new Vector3(target[ran].x,target[ran].y,0);
            }
        }
        else
        {
            if (mapController.player.CurrPosition == CurrPosition ||
                mapController.player.CurrPosition == CurrPosition + LookDirection)
            {
                Attack(1);
            }
            else
            {
                List<Vector2Int> target = new List<Vector2Int>();
                float minD = 1e9f;
                for (int i = -1; i <= 1; i += 2)
                {
                    // if (CurrPosition.x + i >= 0 && CurrPosition.x + i < mapController.mapSize.x)
                    //     Debug.Log(minD + " | " + (CurrPosition.x + i) +", " + CurrPosition.y + "|" +
                    //               mapController.gridMap[CurrPosition.x + i, CurrPosition.y].Cost);
                    // if(CurrPosition.y + i >= 0 && CurrPosition.y + i < mapController.mapSize.y )
                    //     Debug.Log(minD +" | " + CurrPosition.x  +", " + (i+CurrPosition.y) + "|" +
                    //               mapController.gridMap[CurrPosition.x, CurrPosition.y + i].Cost);
                    if (CurrPosition.x + i >= 0 && CurrPosition.x + i < mapController.mapSize.x && mapController.gridMap[CurrPosition.x + i, CurrPosition.y].D4Cost < minD)
                    {
                        minD = mapController.gridMap[CurrPosition.x + i, CurrPosition.y].D4Cost;
                        target.Clear();
                    }
                    if (CurrPosition.x + i >= 0 && CurrPosition.x + i < mapController.mapSize.x && mapController.gridMap[CurrPosition.x + i, CurrPosition.y].D4Cost <= minD)
                    {
                        target.Add(new(i,0));
                    }
                    if (CurrPosition.y + i >= 0 && CurrPosition.y + i < mapController.mapSize.y && mapController.gridMap[CurrPosition.x, CurrPosition.y + i].D4Cost < minD)
                    {
                        minD = mapController.gridMap[CurrPosition.x, CurrPosition.y + i].D4Cost;
                        target.Clear();
                    }
                    if (CurrPosition.y + i >= 0 && CurrPosition.y + i < mapController.mapSize.y && mapController.gridMap[CurrPosition.x, CurrPosition.y + i].D4Cost <= minD)
                    {
                        target.Add(new(0,i));
                    }
                }
    
                if (target.Count !=0)
                {
                    //Debug.Log(minD +" | " +target[0] +"|" +target.Count);
                    if (target.Count > 1)
                    {
                        bool sameDir = false;
                        foreach (var dir in target)
                        {
                            if (dir == LookDirection)
                            {
                                sameDir = true;
                                LookDirection = target[0];
                                CurrPosition += target[0];
                                transform.position += new Vector3(target[0].x,target[0].y,0);
                            }
                        }
    
                        if (!sameDir)
                        {
                            if (target[0].x * LookDirection.x + target[0].y * LookDirection.y == 0)
                            {
                                LookDirection = target[0];
                                CurrPosition += target[0];
                                transform.position += new Vector3(target[0].x,target[0].y,0);
                            }
                            else
                            {
                                LookDirection = target[0];
                            }
                            
                        }
                    }
                    else
                    {
                        if (target[0] == LookDirection)
                        {
                            LookDirection = target[0];
                            CurrPosition += target[0];
                            transform.position += new Vector3(target[0].x,target[0].y,0);
                        }
                        else
                        {
                            if (target[0].x * LookDirection.x + target[0].y * LookDirection.y == 0)
                            {
                                LookDirection = target[0];
                                CurrPosition += target[0];
                                transform.position += new Vector3(target[0].x,target[0].y,0);
                            }
                            else
                            {
                                LookDirection = target[0];
                            }
                        }
                    }
                }
            }
        }
    }
    protected override void Attack(int damage)
    {
        base.Attack(damage);
    }
}
