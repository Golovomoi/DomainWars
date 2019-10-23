using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridClicksHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Tilemap tilemap;
    public Canvas buildMenuCanvas;
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
        _ = new Vector3Int();
        Vector3Int currentCellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(clickPos));
        
        //SelectedField = currentCellPos;
        
        //Debug.Log("Event data up time: " + Time.realtimeSinceStartup);
        if ((Time.realtimeSinceStartup - pointerDownTime < 0.5f) && !buildMenuCanvas.enabled)
        {
            SetSelecetedField(currentCellPos);
            _ = new Vector3();
            Vector3 buildCanvasPos = tilemap.CellToWorld(currentCellPos);
            var rect = buildMenuCanvas.transform as RectTransform;
            //buildCanvasPos.x += rect.sizeDelta.x / 10000 * rect.localScale.x;
            buildCanvasPos.y += rect.sizeDelta.y / 2 * rect.localScale.y;
            buildMenuCanvas.transform.SetPositionAndRotation(buildCanvasPos, Quaternion.identity);
            buildMenuCanvas.enabled = true;
            //Debug.Log(rect.localScale.x);
            //Debug.Log("Position: " + eventData.position);
            //Debug.Log("Press Position: " + eventData.pressPosition);
        }
    }
    public void CloseBuildMenu()
    {
        buildMenuCanvas.enabled = false;
        tilemap.SetTileFlags(SelectedField, TileFlags.None);
        tilemap.SetColor(SelectedField, SelectedFieldOriginColor);
    }
    private void SetSelecetedField(Vector3Int fieldPos)
    {
        tilemap.SetTileFlags(SelectedField, TileFlags.None);
        tilemap.SetColor(SelectedField, SelectedFieldOriginColor);
        SelectedField = fieldPos;
        tilemap.SetTileFlags(SelectedField, TileFlags.None);
        SelectedFieldOriginColor = tilemap.GetColor(SelectedField);
        tilemap.SetColor(SelectedField, SelectionColor);
    }
}
