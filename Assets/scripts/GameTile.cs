using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTile
{
    public enum TileType { None, Terrain, Resource, Capital, Structure}
    //private defaultTileType = TileType.None;
    public int additionalField { get; set; }
    public TileType gameFieldTileType { get; set; }
    public GameObject tileGameObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
