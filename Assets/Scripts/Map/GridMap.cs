using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    private static Tilemap tilemap;

    private Vector2Int mapSize;

    public static TileBase[,] map { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Debug.Log("origin:"+tilemap.origin);
        Debug.Log(tilemap.cellBounds);
        mapSize = new(tilemap.cellBounds.size.x,tilemap.cellBounds.size.y);
        Debug.Log(mapSize);
        map = new TileBase[mapSize.x,mapSize.y];
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                
                map[i, j] = tilemap.GetTile(new (tilemap.origin.x+i,tilemap.origin.y+j,0));
                //Debug.Log(tilemap.GetColliderType(new (tilemap.origin.x+i*5+2,tilemap.origin.y+j*5+2,0)));
            }
        }
    }

    public static bool TilePassable(Vector2Int target)
    {
        return tilemap.GetColliderType(new (target.x,target.y,0)) == Tile.ColliderType.None;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
