using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public int gameFieldSize = 50;
    public Tilemap gameField;
    public Tile TerrainTile;
    public GameObject diamondPrefab;
    public GameObject forcePrefab;
    public GameObject woodPrefab;
    public GameObject playerPrefab;
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
        FindSpawnPosition(new Vector3Int { x = -gameFieldSize / 4, y = gameFieldSize / 4, z = 0 }, playerId);
        PlayerBehavior.instance.PlayerId = playerId;
    }

    void SpawnResources()
    {
        //TODO: "mae as const"
        gameField.size = new Vector3Int(gameFieldSize, gameFieldSize, 0);
        gameField.origin = new Vector3Int(-gameFieldSize / 2, -gameFieldSize / 2, 0);
        gameField.ResizeBounds();
        gameField.SetTile(new Vector3Int(0, 0, 0), TerrainTile);
        //gameField.SetTile(new Vector3Int(0, 1, 0), testTile);
        //gameField.SetTile(new Vector3Int(1, 0, 0), testTile);
        //gameField.SetTile(new Vector3Int(-1, 0, 0), testTile);
        //gameField.SetTile(new Vector3Int(-1, 1, 0), testTile);
        //gameField.SetTile(new Vector3Int(-1, -1, 0), testTile);
        //gameField.SetTile(new Vector3Int(0, -1, 0), testTile);
        //BoundsInt gameArea = new BoundsInt(gameField.origin, gameField.size);ss
        foreach (Vector3Int pos in gameField.cellBounds.allPositionsWithin)
        {
            gameField.SetTile(new Vector3Int(pos.x, pos.y, pos.z), TerrainTile);


            //var tileType = GameTile.TileType.None;
            int genTileType = Random.Range(0, 100) / 5;
            //if (genTileType < 3) tileType = GameTile.TileType.Resource;
            //genTileType = 0;
            switch (genTileType)
            {
                case 0:
                    AddResource(diamondPrefab, pos);
                    break;
                case 1:
                    AddResource(forcePrefab, pos);
                    break;
                case 2:
                    AddResource(woodPrefab, pos);
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector3Int FindSpawnPosition(Vector3Int spawnPos, int playerId)
    {
        Debug.Log("trying: " + spawnPos);
        if (GameFieldTiles.instance.tiles.ContainsKey(spawnPos)) Debug.Log("tile busy"); else Debug.Log("tile free");
        //TODO: rework, increese range
        Vector3Int initialSpawnPos = spawnPos;
        int spawnTries = 0;
        while( spawnTries < 6/*gameFieldSize / playersCount*/) { 
            if (!GameFieldTiles.instance.tiles.ContainsKey(spawnPos))
            {
                Debug.Log(spawnPos + " free");
                var gameTile = new GameTile
                {
                    LocalPlace = spawnPos,
                    AdditionalField = 0,
                    OwnerId = 1,
                    OwnerInfluence = 1000,
                    GameFieldTileType = GameTile.TileType.Capital,
                    tileGameObject = Instantiate(capitalPrefab, gameField.CellToLocal(spawnPos), Quaternion.identity)
                };
                GameFieldTiles.instance.tiles.Add(spawnPos, gameTile);
                PlayersInteractions.instance.AddCapital(spawnPos);
                return spawnPos;
            }
            switch (spawnTries % 6)
            {
                case 0:
                    spawnPos.x += 1;
                    break;
                case 1:
                    spawnPos.x -= 2;
                    break;
                case 2:
                    spawnPos.y += 1;
                    break;
                case 3:
                    spawnPos.y -= 2;
                    break;
                case 4:
                    spawnPos.x += 1;
                    break;
                case 5:
                    spawnPos.y += 2;
                    break;

            }
            Debug.Log("trying next");
            spawnTries++;
        }
        if (GameFieldTiles.instance.tiles.ContainsKey(initialSpawnPos))
        {
            //rethink, rework. DOESNT WORK NOW!!
            Destroy(GameFieldTiles.instance.tiles[initialSpawnPos].tileGameObject);
            GameFieldTiles.instance.tiles[initialSpawnPos].tileGameObject = Instantiate(capitalPrefab, gameField.CellToLocal(initialSpawnPos), Quaternion.identity);
        }
        else
        {
            var gameTile = new GameTile
            {
                AdditionalField = 0,
                GameFieldTileType = GameTile.TileType.Capital,
                tileGameObject = Instantiate(capitalPrefab, gameField.CellToLocal(initialSpawnPos), Quaternion.identity)
            };
            GameFieldTiles.instance.tiles.Add(initialSpawnPos, gameTile);
        }
        return initialSpawnPos;
    }
    private void AddResource(GameObject resourceGameObject, Vector3Int pos)
    {
        var gameTile = new GameTile
        {
            AdditionalField = 0,
            GameFieldTileType = GameTile.TileType.Resource,
            tileGameObject = Instantiate(resourceGameObject, gameField.CellToLocal(pos), Quaternion.identity)
        };
        gameTile.tileGameObject.transform.parent = gameField.transform;
        GameFieldTiles.instance.tiles.Add(pos, gameTile);
    }
}
