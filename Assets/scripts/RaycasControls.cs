using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasControls : MonoBehaviour
{
    public Camera camera;

    private Vector3 touchPosWorld;
    private TouchPhase touchPhase = TouchPhase.Began;
    // Start is called before the first frame updatess
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == touchPhase)
        {
            touchPosWorld = camera.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, camera.transform.forward);

            if (hitInformation.collider != null)
            {
                GameObject touchedObject = hitInformation.transform.gameObject;

                //TODO: check if gameobjeck has Tilemap type 
                UnityEngine.Tilemaps.Tilemap tilemap = touchedObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                Vector3Int cellpos = tilemap.WorldToCell(touchPosWorld2D);
                //Destroy(GameFieldTiles.instance.tiles[cellpos].tileGameObject);
                //Debug.Log("Touched " + touchedObject.transform.name + ", additionalField: " + GameFieldTiles.instance.tiles[cellpos].AdditionalField);

            }
            else
            {
                Debug.Log("not Touched");
            }
        }
    }
}
