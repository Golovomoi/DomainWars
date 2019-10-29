using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBotAI : MonoBehaviour
{
    public int BotId { get; set; }
    private Dictionary<Vector3Int, GameTile> GameField;
    private List<GameTile> CurrentBorders = new List<GameTile>();
    private List<GameTile> CurrentTerritory = new List<GameTile>();
    private List<GameTile> EnemyStructures = new List<GameTile>();
    private List<GameTile> BotStructures = new List<GameTile>();
    private float OcupyGoalProfit;
    private Vector3Int OcupyGoalPos;
    private float InvadeGoalProfit;
    private Vector3Int InvadeGoalPos;
    private int CurrentDiamonds;
    private int CurrentWood;
    private int CurrentForce;
    private Queue<Vector3Int> BfQueue = new Queue<Vector3Int>();
    private HashSet<Vector3Int> VisitedFields = new HashSet<Vector3Int>();
    // Start is called before the first frame update
    void Start()
    {
        GameField = GameFieldTiles.instance.tiles;
    }

    private void FixedUpdate()
    {
        UpdateResources();
        GetGameInfo();
        FindOcupyGoal();
        AchieveOcupyGoal();
        FindInvadeGoal();
        AchieveInvadeGoal();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void GetGameInfo()
    {
        CurrentTerritory.Clear();
        CurrentBorders.Clear();
        BotStructures.Clear();
        EnemyStructures.Clear();
        foreach (var gameTile in GameField.Values)
        {
            if (gameTile.OwnerId != 0 && gameTile.StructType != GameTile.StructureType.None)
                if (gameTile.OwnerId == BotId)
                    BotStructures.Add(gameTile);
                else
                    EnemyStructures.Add(gameTile);
            if (gameTile.OwnerId == BotId)
                CurrentTerritory.Add(gameTile);
            if (IsBorder(gameTile))
                CurrentBorders.Add(gameTile);
        }
    }
    private void FindOcupyGoal()
    {
        OcupyGoalProfit = 0;
        foreach (var gameTile in BotStructures)
        {
            int newCellsCount = 0;
            if (gameTile.StructType == GameTile.StructureType.OcupyBld)
            {
                BfQueue.Clear();
                BfQueue.Enqueue(gameTile.LocalPlace);
                VisitedFields.Clear();
                VisitedFields.Add(gameTile.LocalPlace);
                while (BfQueue.Count != 0)
                {
                    Vector3Int currentField = BfQueue.Dequeue();
                    if (GameField[currentField].GameFieldTileType == GameTile.TileType.Border)
                        continue;
                    int distanceFromBuilding = GameFieldTiles.instance.TileDistance(gameTile.LocalPlace, currentField);
                    if (distanceFromBuilding == gameTile.BuildingLvl + 1 && GameField[currentField].InvaderId == BotId)
                        newCellsCount++;
                    if (distanceFromBuilding < gameTile.BuildingLvl)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector3Int nextField = GameFieldTiles.instance.OddrOffsetNeighbor(currentField, i);
                            if (!VisitedFields.Contains(nextField))
                            {
                                VisitedFields.Add(nextField);
                                BfQueue.Enqueue(nextField);
                            }
                        }
                    }
                }
            }
            float possibleProfit = 0f;
            if (newCellsCount > 0) 
            { 
                possibleProfit = (float) newCellsCount / (gameTile.BuildingLvl * gameTile.BuildingLvl + 1) * 1000;
            }
            if (possibleProfit > OcupyGoalProfit && (gameTile.BuildingLvl * gameTile.BuildingLvl + 1) * 1000 < CurrentDiamonds)
            {
                OcupyGoalProfit = possibleProfit;
                OcupyGoalPos = gameTile.LocalPlace;
            }
        }
        foreach (var gameTile in CurrentBorders)
        {
            int newCellsCount = 0;
            if (gameTile.GameFieldTileType == GameTile.TileType.Terrain)
            {
                BfQueue.Clear();
                BfQueue.Enqueue(gameTile.LocalPlace);
                VisitedFields.Clear();
                VisitedFields.Add(gameTile.LocalPlace);
                while (BfQueue.Count != 0)
                {
                    Vector3Int currentField = BfQueue.Dequeue();
                    if (GameField[currentField].GameFieldTileType == GameTile.TileType.Border)
                        continue;
                    int distanceFromBuilding = GameFieldTiles.instance.TileDistance(gameTile.LocalPlace, currentField);
                    if (distanceFromBuilding == 2 && GameField[currentField].InvaderId == BotId)
                        newCellsCount++;
                    if (distanceFromBuilding < 2)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector3Int nextField = GameFieldTiles.instance.OddrOffsetNeighbor(currentField, i);
                            if (!VisitedFields.Contains(nextField))
                            {
                                VisitedFields.Add(nextField);
                                BfQueue.Enqueue(nextField);
                            }
                        }
                    }
                }
            }
            float possibleProfit = 0f;
            if (newCellsCount > 0)
                possibleProfit = (float) newCellsCount / 1000;
            if (possibleProfit > OcupyGoalProfit && 1000 < CurrentDiamonds)
            {
                OcupyGoalProfit = possibleProfit;
                OcupyGoalPos = gameTile.LocalPlace;
            }
        }
    }
    private void FindInvadeGoal()
    {
        InvadeGoalProfit = 0;
        foreach (var gameTile in BotStructures)
        {
            int newCellsCount = 0;
            if (gameTile.StructType == GameTile.StructureType.InvadeBld)
            {
                BfQueue.Clear();
                BfQueue.Enqueue(gameTile.LocalPlace);
                VisitedFields.Clear();
                VisitedFields.Add(gameTile.LocalPlace);
                while (BfQueue.Count != 0)
                {
                    Vector3Int currentField = BfQueue.Dequeue();
                    if (GameField[currentField].GameFieldTileType == GameTile.TileType.Border)
                        continue;
                    int distanceFromBuilding = GameFieldTiles.instance.TileDistance(gameTile.LocalPlace, currentField);
                    
                    if (distanceFromBuilding == gameTile.BuildingLvl + 1 && GameField[currentField].InvaderId != BotId)
                        newCellsCount++;
                    if (distanceFromBuilding < gameTile.BuildingLvl)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector3Int nextField = GameFieldTiles.instance.OddrOffsetNeighbor(currentField, i);
                            if (!VisitedFields.Contains(nextField))
                            {
                                VisitedFields.Add(nextField);
                                BfQueue.Enqueue(nextField);
                            }
                        }
                    }
                }
            }
            float possibleProfit = 0f;
            if (newCellsCount > 0)
            {
                possibleProfit = (float) newCellsCount / (gameTile.BuildingLvl * gameTile.BuildingLvl + 1) * 1000;
            }
            if (possibleProfit > InvadeGoalProfit && (gameTile.BuildingLvl * gameTile.BuildingLvl + 1) * 1000 < CurrentForce)
            {
                InvadeGoalProfit = possibleProfit;
                InvadeGoalPos = gameTile.LocalPlace;
            }
        }
        foreach (var gameTile in CurrentBorders)
        {
            int newCellsCount = 0;
            if (gameTile.GameFieldTileType == GameTile.TileType.Terrain)
            {
                BfQueue.Clear();
                BfQueue.Enqueue(gameTile.LocalPlace);
                VisitedFields.Clear();
                VisitedFields.Add(gameTile.LocalPlace);
                while (BfQueue.Count != 0)
                {
                    Vector3Int currentField = BfQueue.Dequeue();
                    if (GameField[currentField].GameFieldTileType == GameTile.TileType.Border)
                        continue;
                    int distanceFromBuilding = GameFieldTiles.instance.TileDistance(gameTile.LocalPlace, currentField);
                    
                    if (distanceFromBuilding == 2 && GameField[currentField].InvaderId != BotId)
                        newCellsCount++;
                    if (distanceFromBuilding < 2)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector3Int nextField = GameFieldTiles.instance.OddrOffsetNeighbor(currentField, i);
                            if (!VisitedFields.Contains(nextField))
                            {
                                VisitedFields.Add(nextField);
                                BfQueue.Enqueue(nextField);
                            }
                        }
                    }
                }
            }
            float possibleProfit = 0f;
            if (newCellsCount > 0)
                possibleProfit = (float) newCellsCount / 1000;
            if (possibleProfit > InvadeGoalProfit && 1000 < CurrentForce)
            {
                InvadeGoalProfit = possibleProfit;
                InvadeGoalPos = gameTile.LocalPlace;
            }
        }
    }
    private void AchieveInvadeGoal()
    {
        if (InvadeGoalProfit > 0)
        {
            PlayersInteractions.instance.TryBuildInvadeStruct(InvadeGoalPos, BotId);
        }
        InvadeGoalProfit = 0;
    }
    private void AchieveOcupyGoal()
    {
        if (OcupyGoalProfit > 0)
        {
            PlayersInteractions.instance.TryBuildOcupyStruct(OcupyGoalPos, BotId);
        }
        OcupyGoalProfit = 0;
    }
    private bool IsBorder(GameTile gameTile)
    {
        if (gameTile.OwnerId == BotId)
            for (int i = 0; i < 6; i++)
                if (GameField[GameFieldTiles.instance.OddrOffsetNeighbor(gameTile.LocalPlace, i)].OwnerId != BotId) return true;
        return false;
    }
    private void UpdateResources()
    {
        CurrentDiamonds = PlayersInteractions.instance.GetPlayerDiamonds(BotId);
        CurrentForce = PlayersInteractions.instance.GetPlayerForce(BotId);
        CurrentWood = PlayersInteractions.instance.GetPlayerWood(BotId);
    }
}
