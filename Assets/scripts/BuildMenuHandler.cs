using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuHandler : MonoBehaviour
{
    public Canvas BuildMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildAtackClick()
    {
        
        PlayersInteractions.instance.TryBuildAtackStruct(GridClicksHandler.instance.SelectedField);
        CloseBuildMenu();
    }

    public void CloseBuildMenu()
    {
        GridClicksHandler.instance.CloseBuildMenu();
    }
}
