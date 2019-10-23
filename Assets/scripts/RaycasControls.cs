using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasControls : MonoBehaviour
{
    //public Camera camera;

    //Camera m_MainCamera;

    private Vector3 touchPosWorld;
    private TouchPhase touchPhase = TouchPhase.Ended;
    // Start is called before the first frame updatess
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == touchPhase)
        //{
        //    touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        //    Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        //    RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        //    if (hitInformation.collider != null)
        //    {
        //        GameObject touchedObject = hitInformation.transform.gameObject;

        //        //TODO: check if gameobjeck has Tilemap type 
        //        UnityEngine.Tilemaps.Tilemap tilemap = touchedObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
        //        Vector3Int cellpos = tilemap.WorldToCell(touchPosWorld2D);
        //        //Destroy(GameFieldTiles.instance.tiles[cellpos].tileGameObject);
        //        //Debug.Log("Touched " + touchedObject.transform.name + ", additionalField: " + GameFieldTiles.instance.tiles[cellpos].additionalField);
        //        //Debug.Log("touched cell: " + cellpos);

        //    }
        //    else
        //    {
        //        //Debug.Log("not Touched");
        //    }
        //}
    }
}
