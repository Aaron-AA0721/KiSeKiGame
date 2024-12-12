using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public struct Grid
    {
        public bool IsBlocked;
        public bool ContainingEntity;
        public float D8Cost;
        public float D4Cost;
        public Grid(bool block,bool eneity)
        {
            IsBlocked = block;
            ContainingEntity = eneity;
            D8Cost = 1e9f;
            D4Cost = 1e9f;
        }
    }
    private Tilemap tilemap;
    
    public Vector2Int mapSize { get; protected set; }

    public Grid[,] gridMap { get; private set; }

    public BaseControllableCharacter player;

    public List<BaseAICharacter> enemies;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<BaseControllableCharacter>();
        enemies = FindObjectsOfType<BaseAICharacter>().ToList();
        tilemap = GetComponent<Tilemap>();
        Debug.Log("origin:"+tilemap.origin);
        Debug.Log(tilemap.cellBounds);
        tilemap.CompressBounds();
        mapSize = new(tilemap.cellBounds.size.x,tilemap.cellBounds.size.y);
        Debug.Log(mapSize);
        gridMap = new Grid[mapSize.x,mapSize.y];
        
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {

                gridMap[i, j] = new Grid(
                    tilemap.GetColliderType(new (tilemap.origin.x+i,tilemap.origin.y+j,0)) != Tile.ColliderType.None,
                    false);
                //tilemap.GetColliderType(new (tilemap.origin.x+i,tilemap.origin.y+j,0)) == Tile.ColliderType.None
                    
                //Debug.Log(tilemap.GetColliderType(new (tilemap.origin.x+i*5+2,tilemap.origin.y+j*5+2,0)));
            }
        }

        ResetMap();
        UpdateCostMap();
    }
    
    public bool TilePassable(Vector2Int target)
    {
        //Debug.Log(tilemap.GetColliderType(new (tilemap.origin.x+target.x,tilemap.origin.y+target.y,0)));
        if (target.x < 0 || target.y < 0 || target.x >= mapSize.x || target.y >= mapSize.y) return false;
        return !gridMap[target.x,target.y].IsBlocked;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetMap()
    {
        player.gameObject.transform.position = new(tilemap.origin.x + player.originPosition.x+0.5f,0.5f+tilemap.origin.y+player.originPosition.y);
        player.ResetPosition();
        foreach (var enemy in enemies)
        {
            enemy.gameObject.transform.position = new(tilemap.origin.x + enemy.originPosition.x+0.5f,0.5f+tilemap.origin.y+enemy.originPosition.y);
            enemy.ResetPosition();
        }
    }
    void ClearCost()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {

                gridMap[i, j].D8Cost = 1e9f;
                gridMap[i, j].D4Cost = 1e9f;
                //tilemap.GetColliderType(new (tilemap.origin.x+i,tilemap.origin.y+j,0)) == Tile.ColliderType.None

                //Debug.Log(tilemap.GetColliderType(new (tilemap.origin.x+i*5+2,tilemap.origin.y+j*5+2,0)));
            }
        }
    }
    public void UpdateCostMap()
    {
        Vector2Int playerPos = player.CurrPosition;
        Queue<Vector2Int> GridQueue = new Queue<Vector2Int>();
        GridQueue.Enqueue(playerPos);
        Vector2Int curr;
        ClearCost();
        gridMap[playerPos.x, playerPos.y].D8Cost = 0;
        gridMap[playerPos.x, playerPos.y].D4Cost = 0;
        while (GridQueue.Count > 0)
        {
            curr = GridQueue.Dequeue();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    
                    if (WithinMapBoundary(curr.x + i,curr.y + j))
                    {
                        if (!gridMap[curr.x + i, curr.y + j].IsBlocked &&
                            gridMap[curr.x + i, curr.y + j].D8Cost >
                            gridMap[curr.x, curr.y].D8Cost + Mathf.Sqrt(i * i + j * j))
                        {
                            gridMap[curr.x + i, curr.y + j].D8Cost =
                                gridMap[curr.x, curr.y].D8Cost + Mathf.Sqrt(i * i + j * j);
                            GridQueue.Enqueue(new(curr.x + i, curr.y + j));
                        }
                    }
                }
            }
        }
        GridQueue.Enqueue(playerPos);
        while (GridQueue.Count > 0)
        {
            curr = GridQueue.Dequeue();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        if ( WithinMapBoundary(curr.x + i,curr.y + j))
                        {
                            if (!gridMap[curr.x + i, curr.y + j].IsBlocked &&
                                gridMap[curr.x + i, curr.y + j].D4Cost >
                                gridMap[curr.x, curr.y].D4Cost + Mathf.Sqrt(i * i + j * j))
                            {
                                gridMap[curr.x + i, curr.y + j].D4Cost =
                                    gridMap[curr.x, curr.y].D4Cost + Mathf.Sqrt(i * i + j * j);
                                GridQueue.Enqueue(new(curr.x + i, curr.y + j));
                            }
                        }
                    }
                    
                }
            }
        }
        // string debugs = "";
        // for (int i = 0; i < mapSize.x; i++)
        // {
        //     for (int j = 0; j < mapSize.y; j++)
        //     {
        //         debugs += gridMap[i, j].Cost + ", ";
        //         //tilemap.GetColliderType(new (tilemap.origin.x+i,tilemap.origin.y+j,0)) == Tile.ColliderType.None
        //
        //         //Debug.Log(tilemap.GetColliderType(new (tilemap.origin.x+i*5+2,tilemap.origin.y+j*5+2,0)));
        //     }
        //
        //     debugs += "\n";
        // }
        // Debug.Log(debugs);
    }

    public bool WithinMapBoundary(int i, int j)
    {
        return i >= 0 && i < mapSize.x && j >= 0 && j < mapSize.y;
    }

    public void AttackEnemy(Vector2Int position,int damage)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.CurrPosition == position) enemy.GetHurt(damage);
        }
    }
}
