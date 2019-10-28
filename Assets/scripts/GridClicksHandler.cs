using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridClicksHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Tilemap tilemap;
    public Canvas buildMenuCanvas;
    public Canvas fieldInfoCanvas;
    private float pointerDownTime;
    public static GridClicksHandler instance;
    public Color SelectionColor;
    private Color SelectedFieldOriginColor = Color.white;
    public Vector3Int SelectedField { get; set; }
    private Vector3Int prevSelectedField;
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
        SelectedField = new Vector3Int(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("CLICK! from GameFieldTiles at cell: " + currentCellPos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownTime = Time.realtimeSinceStartup;
        //Debug.Log("pointer Down time: " + pointerDownTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 clickPos = new Vector3
        {
            x = eventData.position.x,
            y = eventData.position.y,
            z = 10
        };
        Vector3 UpPos = new Vector3
        {
            x = eventData.pressPosition.x,
            y = eventData.pressPosition.y,
            z = 10
        };
        _ = new Vector3Int();
        Vector3Int pressCell = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(UpPos));
        _ = new Vector3Int();
        Vector3Int currentCellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(clickPos));

        if ((currentCellPos == pressCell) && IsAnyCanvasEnabled())
            CloseAllMenus();
        if (currentCellPos  == pressCell)
        {
            SetSelecetedField(currentCellPos);
            if (GameFieldTiles.instance.tiles[SelectedField].OwnerId == PlayerBehavior.instance.PlayerId && GameFieldTiles.instance.tiles[SelectedField].GameFieldTileType == GameTile.TileType.Terrain)
                OpenMenu(buildMenuCanvas);
            else
                OpenMenu(fieldInfoCanvas);

        }
    }
    private void OpenMenu(Canvas canvas)
    {
        _ = new Vector3();
        Vector3 buildCanvasPos = tilemap.CellToWorld(SelectedField);
        var rect = canvas.transform as RectTransform;
        //buildCanvasPos.x += rect.sizeDelta.x / 10000 * rect.localScale.x;
        buildCanvasPos.y += rect.sizeDelta.y / 2 * rect.localScale.y;
        canvas.transform.SetPositionAndRotation(buildCanvasPos, Quaternion.identity);
        canvas.enabled = true;
        //Debug.Log(rect.localScale.x);
        //Debug.Log("Position: " + eventData.position);
        //Debug.Log("Press Position: " + eventData.pressPosition);
    }
    public void OpenBuildMenu()
    {
        if (GameFieldTiles.instance.tiles[SelectedField].OwnerId == PlayerBehavior.instance.PlayerId && GameFieldTiles.instance.tiles[SelectedField].GameFieldTileType == GameTile.TileType.Terrain)
        {
            CloseAllMenus();
            SetSelecetedField(SelectedField);
            OpenMenu(buildMenuCanvas);
        }
    }
    public void OpenFieldInfo()
    {
        CloseAllMenus();
        SetSelecetedField(SelectedField);
        OpenMenu(fieldInfoCanvas);
    }
    private bool IsAnyCanvasEnabled()
    {
        return buildMenuCanvas.enabled || fieldInfoCanvas.enabled;
    }
    public void CloseAllMenus()
    {
        buildMenuCanvas.enabled = false;
        fieldInfoCanvas.enabled = false;
        tilemap.SetTileFlags(SelectedField, TileFlags.None);
        tilemap.SetColor(SelectedField, SelectedFieldOriginColor);
    }
    private void SetSelecetedField(Vector3Int fieldPos)
    {
        tilemap.SetTileFlags(SelectedField, TileFlags.None);
        tilemap.SetColor(SelectedField, SelectedFieldOriginColor);
        SelectedField = fieldPos;
        PlayerBehavior.instance.SelectedField = SelectedField;
        tilemap.SetTileFlags(SelectedField, TileFlags.None);
        SelectedFieldOriginColor = tilemap.GetColor(SelectedField);
        tilemap.SetColor(SelectedField, SelectionColor);
    }
}
