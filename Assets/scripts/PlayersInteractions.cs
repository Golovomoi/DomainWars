using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInteractions : MonoBehaviour
{
    public static PlayersInteractions instance;
    // May Be move to GameFieldTiles;
    private Dictionary<int, Color> PlayersColors = new Dictionary<int, Color>();
    private HashSet<Vector3Int> InvadeBuildings = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> DefenceBuildings = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> OcupyBuildings = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> Capitals = new HashSet<Vector3Int>();
    private Queue<Vector3Int> BfQueue = new Queue<Vector3Int>();
    private HashSet<Vector3Int> VisitedFields = new HashSet<Vector3Int>();
    delegate void ChangeTileState(GameTile gameTile, int influenceValue, int playerId);
    int debugint;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        debugint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        IncreseGlobalInfluence();
        IncreseLocalInfluence();
        ExpandBorders();
    }
    public bool TryBuildInvadeStruct(Vector3Int buildPos, int playerId)
    {
        // TODO: Add conditions
        if (GameFieldTiles.instance.tiles[buildPos].OwnerId == playerId && GameFieldTiles.instance.tiles[buildPos].GameFieldTileType == GameTile.TileType.Terrain)
        {
            GameFieldTiles.instance.AddInvadeStructure(buildPos);
            InvadeBuildings.Add(buildPos);
            return true;
        }
            return false;
        //return (!GameFieldTiles.instance.tiles.ContainsKey(buildPos) || GameFieldTiles.instance.tiles[buildPos].GameFieldTileType == GameTile.TileType.None) ? true : false;
    }
    public bool TryBuildDefStruct(Vector3Int buildPos, int playerId)
    {
        if (GameFieldTiles.instance.tiles[buildPos].OwnerId == playerId && GameFieldTiles.instance.tiles[buildPos].GameFieldTileType == GameTile.TileType.Terrain)
        {
            GameFieldTiles.instance.AddDefenceStructure(buildPos);
            DefenceBuildings.Add(buildPos);
            return true;
        }
            return false;
    }
    public bool TryBuildOcupyStruct(Vector3Int buildPos, int playerId)
    {
        if (GameFieldTiles.instance.tiles[buildPos].OwnerId == playerId && GameFieldTiles.instance.tiles[buildPos].GameFieldTileType == GameTile.TileType.Terrain)
        {
            GameFieldTiles.instance.AddOcupyStructure(buildPos);
            OcupyBuildings.Add(buildPos);
            return true;
        }
            return false;
    }
    public void AddCapital(Vector3Int capitalPos, GameTile gameTile, int playerId)
    {
        Capitals.Add(capitalPos);
        GameFieldTiles.instance.AddCapitalStructure(capitalPos, playerId);
        GameFieldTiles.instance.SetTileColor(capitalPos,  PlayersColors[playerId]);
    }
    public void AddPlayerColor(int playerId, Color playerColor)
    {
        PlayersColors.Add(playerId, playerColor);
    }
    private void ExpandBorders()
    {
        ChangeTileState tileFunc = ExpandBordersInTile;
        foreach (var buildingPos in Capitals)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.Capital)
            {
                ChangelInfluenceByBuilding(buildingPos, 4, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
        foreach (var buildingPos in OcupyBuildings)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.OcupyBld)
            {
                ChangelInfluenceByBuilding(buildingPos, 2, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
    }
    private void IncreseLocalInfluence()
    {
        ChangeTileState tileFunc = ChangeLocalInfluenceInTile;
        foreach (var buildingPos in Capitals)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.Capital)
            {
                ChangelInfluenceByBuilding(buildingPos, 4, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
        foreach (var buildingPos in DefenceBuildings)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.DefenceBld)
            {
                ChangelInfluenceByBuilding(buildingPos, 2, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
    }
    private void IncreseGlobalInfluence()
    {
        ChangeTileState tileFunc = ChangeGlobalInfluenceInTile;
        foreach (var buildingPos in Capitals)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.Capital)
            {
                ChangelInfluenceByBuilding(buildingPos, 4, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
        foreach (var buildingPos in InvadeBuildings)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.InvadeBld)
            {
                ChangelInfluenceByBuilding(buildingPos, 2, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
    }
    private void ChangelInfluenceByBuilding(Vector3Int tilePos, int influenceValue, int playerId, ChangeTileState tileFunc)
    {
        BfQueue.Clear();
        BfQueue.Enqueue(tilePos);
        VisitedFields.Clear();
        VisitedFields.Add(tilePos);
        while(BfQueue.Count != 0)
        {
            Vector3Int currentField = BfQueue.Dequeue();
            int distanceFromBuilding = GameFieldTiles.instance.TileDistance(tilePos, currentField);
            //Debug.Log("distance from " + tilePos + "to " + currentField + "is: "+  distanceFromBuilding);
            //Debug.Log("in cycle: " + currentField);
            GameTile gameTile = GameFieldTiles.instance.tiles[currentField];
            tileFunc(gameTile, influenceValue - distanceFromBuilding + 1, playerId);
            if (distanceFromBuilding < influenceValue)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector3Int nextField = GameFieldTiles.instance.OddrOffsetNeighbor(currentField, i);
                    if (!VisitedFields.Contains(nextField))
                    {
                        //Debug.Log("added: " + nextField);
                        VisitedFields.Add(nextField);
                        BfQueue.Enqueue(nextField);
                    }
                }
            }
        }
    }
    private void ChangeGlobalInfluenceInTile(GameTile gameTile, int influenceValue, int playerId)
    {
        if (gameTile.OwnerId != playerId)
        {
            int remaindedInfluence = influenceValue;
            if (gameTile.InvaderId != playerId)
            {
                if (gameTile.OwnerInfluence > 0)
                {
                    if (gameTile.OwnerInfluence <= remaindedInfluence)
                    {
                        remaindedInfluence -= gameTile.OwnerInfluence;
                        gameTile.OwnerInfluence = 0;
                    }
                    else
                    {
                        gameTile.OwnerInfluence -= remaindedInfluence;
                        remaindedInfluence = 0;
                    }
                }
                if (gameTile.InvaderInfluence < remaindedInfluence)
                {
                    gameTile.InvaderInfluence = remaindedInfluence - gameTile.InvaderInfluence;
                    gameTile.InvaderId = playerId;
                }
                else
                {
                    gameTile.InvaderInfluence -= remaindedInfluence;
                }
            }
            else
            {
                gameTile.InvaderInfluence += remaindedInfluence;
            }
        }
    }
    private void ChangeLocalInfluenceInTile(GameTile gameTile, int influenceValue, int playerId)
    {
        if (gameTile.OwnerId == playerId)
        {
            int remaindedInfluence = influenceValue;
            if (gameTile.InvaderId != playerId)
            {
                if (gameTile.InvaderInfluence <= remaindedInfluence)
                {
                    remaindedInfluence -= gameTile.InvaderInfluence;
                    gameTile.OwnerInfluence += remaindedInfluence;
                    gameTile.InvaderInfluence = 0;
                    gameTile.InvaderId = playerId;
                }
                else
                {
                    gameTile.InvaderInfluence -= remaindedInfluence;
                }
            }
            else
            {
                gameTile.OwnerInfluence += remaindedInfluence;
            }
        }
    }
    private void ExpandBordersInTile(GameTile gameTile, int influenceValue, int playerId)
    {
        //TODO: Forbidden Cells, recources, destroy structures
        if (gameTile.OwnerId != playerId)
        {
            if (gameTile.InvaderId == playerId && gameTile.InvaderInfluence > 0)
            {
                gameTile.OwnerId = playerId;
                gameTile.OwnerInfluence = gameTile.InvaderInfluence;
                gameTile.InvaderInfluence = 0;
                GameFieldTiles.instance.SetTileColor(gameTile.LocalPlace, PlayersColors[playerId]);
            }
        }
    }
}
