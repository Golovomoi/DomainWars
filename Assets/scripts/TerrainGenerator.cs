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
    //private GameTile tgTile;
    // Start is called before the first frame update
    void Start()
    {
        //TODO: "mae as const"
        gameField.size = new Vector3Int(gameFieldSize, gameFieldSize, 0);
        gameField.origin = new Vector3Int(-gameFieldSize/2, -gameFieldSize/2, 0);
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

            var gameTile = new GameTile();
            //{
            //    AdditionalField = 10,
            //    tileGameObject = Instantiate(diamondPrefab, gameField.CellToLocal(pos), Quaternion.identity)
            //};

            int genTileType = Random.Range(0, 100)/5;
            if (genTileType < 4) gameTile.gameFieldTileType = GameTile.TileType.Resource;
            switch (genTileType)
            {
                case 0:
                    gameTile.tileGameObject = Instantiate(diamondPrefab, gameField.CellToLocal(pos), Quaternion.identity);
                    break;
                case 1:
                    gameTile.tileGameObject = Instantiate(forcePrefab, gameField.CellToLocal(pos), Quaternion.identity);
                    break;
                case 2:
                    gameTile.tileGameObject = Instantiate(woodPrefab, gameField.CellToLocal(pos), Quaternion.identity);
                    break;
            }

            GameFieldTiles.instance.tiles.Add(pos, gameTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
