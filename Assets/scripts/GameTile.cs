using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTile
{
    public enum TileType { Terrain, Resource, Structure}
    public int additionalField { get; set; }
    public string gamefieldType { get; set; }

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
