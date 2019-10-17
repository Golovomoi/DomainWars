using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameFieldTiles : MonoBehaviour
{
    public static GameFieldTiles instance;
    public Tilemap tilemap;

    public Dictionary<Vector3, GameTile> tiles;

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

    private void GameFieldInit()
    {
        tiles = new Dictionary<Vector3, GameTile>();
    }
    public void SetNewGameTile()
    {

    }
}
