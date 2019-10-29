using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldInfoHandler : MonoBehaviour
{
    public Canvas GameFieldInfoCanvas;
    public Text FieldInfo;
    public Text BuildingType;
    public Text BuildingLvl;
    public Text Upgrade;
    public Text Owner;
    public Text OwnerInfluence;
    public Text Invader;
    public Text InvaderInfluence;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameFieldInfoCanvas.enabled)
        {
            SetGameFieldInfo();
        }
    }
    public void ChangeCanvasClick()
    {
        if (GameFieldTiles.instance.tiles[GridClicksHandler.instance.SelectedField].OwnerId == PlayerBehavior.instance.PlayerId)
            GridClicksHandler.instance.OpenBuildMenu();
    }
    public void CloseButtonClick()
    {
        GridClicksHandler.instance.CloseAllMenus();
    }
    public void UpgradeClick()
    {
        PlayersInteractions.instance.TryUpgradeStructure(GridClicksHandler.instance.SelectedField, PlayerBehavior.instance.PlayerId);
        SetGameFieldInfo();
    }
    private void SetGameFieldInfo()
    {
        GameTile gameTile = GameFieldTiles.instance.tiles[GridClicksHandler.instance.SelectedField];
        string text = gameTile.GameFieldTileType.ToString();
        if (gameTile.GameFieldTileType == GameTile.TileType.Resource)
            text = gameTile.ResType.ToString();
        FieldInfo.text = string.Format("Field Type: {0}", text);

        text = gameTile.GameFieldTileType == GameTile.TileType.Structure ? gameTile.StructType.ToString() : "None"; 
        BuildingType.text = string.Format("Building: {0}", text);

        text = gameTile.BuildingLvl.ToString();
        BuildingLvl.text = string.Format("Building Lvl: {0}", text);

        text = ((gameTile.BuildingLvl * gameTile.BuildingLvl + 1) * 1000).ToString();
        Upgrade.text = string.Format("Upgrade  (cosst: {0})", text);

        text = gameTile.OwnerId.ToString();
        Owner.text = string.Format("Owner: {0}", text);

        text = gameTile.OwnerInfluence.ToString();
        OwnerInfluence.text = string.Format("Owner Influence: {0}", text);

        text = gameTile.InvaderId.ToString();
        Invader.text = string.Format("Invader: {0}", text);

        text = gameTile.InvaderInfluence.ToString();
        InvaderInfluence.text = string.Format("Invader Influence: {0}", text);
    }
}
