using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 mapSize;
    [Range(0, 1)]
    public float outLinePercent;

    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        string holderName = "Gererated Map";
        if(transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for(int x=0; x<mapSize.x; x++)
        {
            for(int y=0; y<mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + .5f + x, 0, -mapSize.y / 2 + .5f + y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                //Debug.Log($"{-mapSize.x / 2 + 5f + x}, 0, {-mapSize.y / 2 + .5f + y}");
                newTile.localScale = Vector3.one * (1 - outLinePercent);
                //Debug.Log($"{newTile.localScale}");
                newTile.parent = mapHolder;
            }
        }
    }
}
