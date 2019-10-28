using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInteractions : MonoBehaviour
{
    public static PlayersInteractions instance;
    // Should be mutable struct or class?
    struct PlayerResources
    {
        public int Diamonds;
        public int Wood;
        public int Force;
    }
    private Dictionary<int, PlayerResources> PlayersResources = new Dictionary<int, PlayerResources>();
    // May Be move colors to GameFieldTiles;
    private Dictionary<int, Color> PlayersColors = new Dictionary<int, Color>();
    private HashSet<Vector3Int> OwnedResources = new HashSet<Vector3Int>();
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
        CalculateResources();
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
    public bool TryUpgradeStructure(Vector3Int structPos, int playerId)
    {
        GameTile gameTile = GameFieldTiles.instance.tiles[structPos];
        return false;
    }
    public void AddPlayer(Vector3Int capitalPos, int playerId)
    {
        AddCapital(capitalPos, playerId);
        PlayerResources playerResources = new PlayerResources
        {
            Diamonds = 0,
            Force = 0,
            Wood = 0
        };
        PlayersResources.Add(playerId, playerResources);
    }
    private void AddCapital(Vector3Int capitalPos, int playerId)
    {
        Capitals.Add(capitalPos);
        GameFieldTiles.instance.AddCapitalStructure(capitalPos, playerId);
        GameFieldTiles.instance.SetTileColor(capitalPos,  PlayersColors[playerId]);
    }
    public void AddPlayerColor(int playerId, Color playerColor)
    {
        PlayersColors.Add(playerId, playerColor);
    }
    public int GetPlayerDiamonds(int playerId)
    {
        return PlayersResources[playerId].Diamonds;
    }
    public int GetPlayerWood(int playerId)
    {
        return PlayersResources[playerId].Wood;
    }
    public int GetPlayerForce(int playerId)
    {
        return PlayersResources[playerId].Force;
    }
    private void AddOwnedResource(Vector3Int resPos)
    {
        if (!OwnedResources.Contains(resPos))
        {
            OwnedResources.Add(resPos);
        }
    }
    private void CalculateResources()
    {
        foreach (var resPos in OwnedResources)
        {
            GameTile gameTile = GameFieldTiles.instance.tiles[resPos];
            PlayerResources playerResources = PlayersResources[gameTile.OwnerId];
            switch (gameTile.ResType)
            {
                case GameTile.ResourceType.Diamonds:
                    playerResources.Diamonds += 1;
                    break;
                case GameTile.ResourceType.Wood:
                    playerResources.Wood += 1;
                    break;
                case GameTile.ResourceType.Force:
                    playerResources.Force += 1;
                    break;
            }
            PlayersResources[gameTile.OwnerId] = playerResources;
        }
    }
    private void ExpandBorders()
    {
        ChangeTileState tileFunc = ExpandBordersInTile;
        foreach (var buildingPos in Capitals)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.Capital)
            {
                ChangelInfluenceByBuilding(buildingPos, GameFieldTiles.instance.tiles[buildingPos].BuildingLvl + 3, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
        foreach (var buildingPos in OcupyBuildings)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.OcupyBld)
            {
                ChangelInfluenceByBuilding(buildingPos, GameFieldTiles.instance.tiles[buildingPos].BuildingLvl + 1, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
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
                ChangelInfluenceByBuilding(buildingPos, GameFieldTiles.instance.tiles[buildingPos].BuildingLvl + 3, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
        foreach (var buildingPos in DefenceBuildings)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.DefenceBld)
            {
                ChangelInfluenceByBuilding(buildingPos, GameFieldTiles.instance.tiles[buildingPos].BuildingLvl + 1, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
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
                ChangelInfluenceByBuilding(buildingPos, GameFieldTiles.instance.tiles[buildingPos].BuildingLvl + 3, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
            }
        }
        foreach (var buildingPos in InvadeBuildings)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Structure && GameFieldTiles.instance.tiles[buildingPos].StructType == GameTile.StructureType.InvadeBld)
            {
                ChangelInfluenceByBuilding(buildingPos, GameFieldTiles.instance.tiles[buildingPos].BuildingLvl + 1, GameFieldTiles.instance.tiles[buildingPos].OwnerId, tileFunc);
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
                if (gameTile.GameFieldTileType == GameTile.TileType.Resource)
                {
                    AddOwnedResource(gameTile.LocalPlace);
                }
            }
        }
    }
}
