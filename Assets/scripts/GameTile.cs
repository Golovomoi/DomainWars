using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GameTile : IPointerClickHandler
{
    public Vector3Int LocalPlace { get; set;  }
    public enum TileType { None, Terrain, Wood, Diamonds, Force, Structure }
    public enum StructureType { None, Capital, InvadeBld, DefenceBld, OcupyBld }
    //private defaultTileType = TileType.None;
    public int AdditionalField { get; set; }
    public TileType GameFieldTileType { get; set; }
    public StructureType StructType { get; set; }
    public int OwnerId { get; set; }
    public int OwnerInfluence { get; set; }
    public int InvaderId { get; set; }
    public int InvaderInfluence { get; set; }
    public int BuildingLvl { get; set; }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK!");
    }
}
