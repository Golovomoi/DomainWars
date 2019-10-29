using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public int gameFieldSize = 50;
    public Tilemap gameField;
    public Tile TerrainTile;
    public Tile BorderTile;
    public GameObject diamondPrefab;
    public GameObject forcePrefab;
    public GameObject woodPrefab;
    public GameObject playerPrefab;
    public GameObject GreedyBotPrefab;
    public GameObject capitalPrefab;
    public int playersCount;
    //private GameTile tgTile;
    // Start is called before the first frame update
    void Start()
    {
        SpawnResources();
        int playerId = 1;
        //GameObject plGameobj = Instantiate(playerPrefab, gameField.CellToWorld(FindSpawnPosition(new Vector3Int { x = -gameFieldSize/ 4, y = gameFieldSize / 4, z = 0 })), Quaternion.identity);
        //PlayerBehavior playerBehaviorScript = plGameobj.GetComponent<PlayerBehavior>();
        //playerBehaviorScript.playerId = playerId;
        Vector3Int spawnPos = FindSpawnPosition(new Vector3Int { x = -gameFieldSize / 4, y = gameFieldSize / 4, z = 0 });
        SpawnCapital(spawnPos, playerId);
        PlayerBehavior.instance.PlayerId = playerId;
        playerId++;
        spawnPos = FindSpawnPosition(new Vector3Int { x = gameFieldSize / 4, y = -gameFieldSize / 4, z = 0 });
        SpawnCapital(spawnPos, playerId);
        GameObject botObject = Instantiate(GreedyBotPrefab, spawnPos, Quaternion.identity);
        GreedyBotAI greedyBotAI = botObject.GetComponent<GreedyBotAI>();
        greedyBotAI.BotId = playerId;

    }

    void SpawnResources()
    {
        //TODO: "mae as const"
        gameField.size = new Vector3Int(gameFieldSize, gameFieldSize, 0);
        gameField.origin = new Vector3Int(-gameFieldSize / 2, -gameFieldSize / 2, 0);
        gameField.ResizeBounds();
        gameField.SetTile(new Vector3Int(0, 0, 0), TerrainTile);
        //BoundsInt gameArea = new BoundsInt(gameField.origin, gameField.size);
        foreach (Vector3Int pos in gameField.cellBounds.allPositionsWithin)
        {
            var gameTile = new GameTile
            {
                LocalPlace = pos,
                AdditionalField = 0,
                OwnerId = 0,
                OwnerInfluence = 0,
                GameFieldTileType = GameTile.TileType.Terrain,
                InvaderId = 0,
                InvaderInfluence = 0,
                StructType = GameTile.StructureType.None,
                BuildingLvl = 0
            };
            if (pos.x == gameField.origin.x || pos.x == gameField.origin.x + gameField.size.x - 1 ||
                pos.y == gameField.origin.y || pos.y == gameField.origin.y + gameField.size.y - 1)
            {
                gameTile.GameFieldTileType = GameTile.TileType.Border;
                gameField.SetTile(new Vector3Int(pos.x, pos.y, pos.z), BorderTile);
                GameFieldTiles.instance.tiles.Add(pos, gameTile);
                continue;
            }
            GameFieldTiles.instance.tiles.Add(pos, gameTile);
            
            gameField.SetTile(new Vector3Int(pos.x, pos.y, pos.z), TerrainTile);

            //var tileType = GameTile.TileType.None;
            int genTileType = Random.Range(0, 100) / 5;
            //if (genTileType < 3) tileType = GameTile.TileType.Resource;
            //genTileType = 0;
            //TODO:  move AddRessourceto GameFieldTiles
            switch (genTileType)
            {
                case 0:
                    AddResource(diamondPrefab, GameTile.ResourceType.Diamonds, pos);
                    break;
                case 1:
                    AddResource(forcePrefab, GameTile.ResourceType.Force, pos);
                    break;
                case 2:
                    AddResource(woodPrefab, GameTile.ResourceType.Wood, pos);
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnCapital(Vector3Int spawnPos, int playerId)
    {
        GameTile gameTile = GameFieldTiles.instance.tiles[spawnPos];
        PlayersInteractions.instance.AddPlayerColor(playerId, Random.ColorHSV());
        PlayersInteractions.instance.AddPlayer(spawnPos, playerId);
    }
    private Vector3Int FindSpawnPosition(Vector3Int spawnPos)
    {
        Debug.Log("trying: " + spawnPos);
        if (GameFieldTiles.instance.tiles[spawnPos].GameFieldTileType != GameTile.TileType.Terrain) Debug.Log("tile busy"); else Debug.Log("tile free");
        //TODO: rework, increese range
        Vector3Int initialSpawnPos = spawnPos;
        int spawnTries = 0;
        while( spawnTries < 6/*gameFieldSize / playersCount*/) {
            GameTile gameTile = GameFieldTiles.instance.tiles[spawnPos];
            if (gameTile.GameFieldTileType == GameTile.TileType.Terrain)
            {
                Debug.Log(spawnPos + " free");
                return spawnPos;
            }
            spawnPos = GameFieldTiles.instance.OddrOffsetNeighbor(initialSpawnPos, spawnTries++);
            Debug.Log("trying next");
        }
        return initialSpawnPos;
    }
    private void AddResource(GameObject resourceGameObject, GameTile.ResourceType resourceType, Vector3Int pos)
    {
        GameTile gameTile = GameFieldTiles.instance.tiles[pos];
        gameTile.GameFieldTileType = GameTile.TileType.Resource;
        gameTile.ResType = resourceType;
        gameTile.tileGameObject = Instantiate(resourceGameObject, gameField.CellToLocal(pos), Quaternion.identity);
        gameTile.tileGameObject.transform.parent = gameField.transform;
    }
}
