using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CameraMovement : MonoBehaviour
{
    public float speed = 0.1F;
    private bool shouldntMoove;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: think about drag and drop;
        //if (Input.touchCount == 1 &&IsPointerOverUIObject())
        //{
        //    Debug.Log("Yes, over GameObj");
        //    shouldntMoove = true;
        //}
        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) shouldntMoove = false;
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && !shouldntMoove)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
        }

    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
