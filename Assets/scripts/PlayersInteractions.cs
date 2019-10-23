using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInteractions : MonoBehaviour
{
    public static PlayersInteractions instance;
    private List<Color> PlayersColors;
    private HashSet<Vector3Int> AtackBuildings = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> DefenceBuildings = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> InvadeBuildings = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> Capitals = new HashSet<Vector3Int>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        IncreseGlobalInfluence();
    }
    public bool TryBuildAtackStruct(Vector3Int buildPos)
    {
        return true;
        //return (!GameFieldTiles.instance.tiles.ContainsKey(buildPos) || GameFieldTiles.instance.tiles[buildPos].GameFieldTileType == GameTile.TileType.None) ? true : false;
    }
    public bool TryBuildDefStruct(Vector3Int buildPos)
    {
        return true;
    }
    public void AddCapital(Vector3Int capitalPos)
    {
        Capitals.Add(capitalPos);
    }
    private void IncreseGlobalInfluence()
    {
        foreach (var buildingPos in Capitals)
        {
            if (GameFieldTiles.instance.tiles[buildingPos].GameFieldTileType == GameTile.TileType.Capital)
            {
                IncreseGlobalInfluenceFromTile(buildingPos, 13);
            }
        }
    }
    private void IncreseGlobalInfluenceFromTile(Vector3Int tilePos, int influenceValue)
    {

    }
}
