using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GameFieldTiles : MonoBehaviour
{
    public static GameFieldTiles instance;
    public Tilemap tilemap;
    public GameObject diamondPrefab;
    public GameObject forcePrefab;
    public GameObject woodPrefab;
    public GameObject capitalPrefab;
    public GameObject InvadeStructPrefab;
    public GameObject DefenceStructPrefab;
    public GameObject OcupyStructPrefab;

    public Dictionary<Vector3Int, GameTile> tiles;
    public static readonly int[,,] OddrDirections = 
    { 
        { { 1, 0 }, { 0, 1 }, { -1, 1 },
          { -1, 0 }, { -1, -1 }, { 0, -1 } },
        { { 1, 0 }, {1, 1 }, { 0, 1 },
          { -1,  0}, { 0, -1}, {1, -1} }
    };
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        GameFieldInit();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3Int OddrOffsetNeighbor(Vector3Int hex, int direction)
    {
        int parity = hex.y & 1;
        return new Vector3Int(hex.x + OddrDirections[parity, direction, 0], hex.y + OddrDirections[parity, direction, 1], hex.z);
    }
    private Vector3Int OddrToCube(Vector3Int hex)
    {
        int x = hex.x - (hex.y - (hex.y & 1)) / 2;
        int z = hex.y;
        int y = -x - z;
        return new Vector3Int(x,y,z);
    }
    public int TileDistance(Vector3Int a, Vector3Int b)
    {
        Vector3Int from = OddrToCube(a);
        Vector3Int to = OddrToCube(b);
        return (Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y -to.y) + Mathf.Abs(from.z - to.z)) / 2;
    }
    private void GameFieldInit()
    {
        tiles = new Dictionary<Vector3Int, GameTile>();
    }
    public void AddCapitalStructure(Vector3Int buildPos, int playerId)
    {
        GameTile gameTile = tiles[buildPos];
        gameTile.OwnerId = playerId;
        gameTile.OwnerInfluence = 1000;
        gameTile.GameFieldTileType = GameTile.TileType.Structure;
        gameTile.StructType = GameTile.StructureType.Capital;
        gameTile.BuildingLvl = 4;
        if (gameTile.tileGameObject != null)
            Destroy(gameTile.tileGameObject);
        gameTile.tileGameObject = Instantiate(capitalPrefab, tilemap.CellToLocal(buildPos), Quaternion.identity);

    }
    public void AddInvadeStructure(Vector3Int buildPos)
    {
        GameTile gameTile = tiles[buildPos];
        gameTile.GameFieldTileType = GameTile.TileType.Structure;
        gameTile.StructType = GameTile.StructureType.InvadeBld;
        gameTile.BuildingLvl = 2;
        if (gameTile.tileGameObject != null)
            Destroy(gameTile.tileGameObject);
        gameTile.tileGameObject = Instantiate(InvadeStructPrefab, tilemap.CellToLocal(buildPos), Quaternion.identity);
    }
    public void AddDefenceStructure(Vector3Int buildPos)
    {
        GameTile gameTile = tiles[buildPos];
        gameTile.GameFieldTileType = GameTile.TileType.Structure;
        gameTile.StructType = GameTile.StructureType.DefenceBld;
        gameTile.BuildingLvl = 2;
        if (gameTile.tileGameObject != null)
            Destroy(gameTile.tileGameObject);
        gameTile.tileGameObject = Instantiate(DefenceStructPrefab, tilemap.CellToLocal(buildPos), Quaternion.identity);
    }
    public void AddOcupyStructure(Vector3Int buildPos)
    {
        GameTile gameTile = tiles[buildPos];
        gameTile.GameFieldTileType = GameTile.TileType.Structure;
        gameTile.StructType = GameTile.StructureType.OcupyBld;
        gameTile.BuildingLvl = 2;
        if (gameTile.tileGameObject != null)
            Destroy(gameTile.tileGameObject);
        gameTile.tileGameObject = Instantiate(OcupyStructPrefab, tilemap.CellToLocal(buildPos), Quaternion.identity);
    }
    public void SetTileColor(Vector3Int tilePos, Color tileColor)
    {
        tilemap.SetTileFlags(tilePos, TileFlags.None);
        tilemap.SetColor(tilePos, tileColor);
    }
    public void SetNewGameTile()
    {

    }
}
