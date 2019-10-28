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

    public void ChangeCanvasClick()
    {
            GridClicksHandler.instance.OpenFieldInfo();
    }

    public void BuildInvadeClick()
    {
        
        PlayersInteractions.instance.TryBuildInvadeStruct(GridClicksHandler.instance.SelectedField,  PlayerBehavior.instance.PlayerId);
        CloseBuildMenu();
    }
    public void BuildDefenceClick()
    {

        PlayersInteractions.instance.TryBuildDefStruct(GridClicksHandler.instance.SelectedField, PlayerBehavior.instance.PlayerId);
        CloseBuildMenu();
    }
    public void BuildOcupyClick()
    {

        PlayersInteractions.instance.TryBuildOcupyStruct(GridClicksHandler.instance.SelectedField, PlayerBehavior.instance.PlayerId);
        CloseBuildMenu();
    }
    public void CloseBuildMenu()
    {
        GridClicksHandler.instance.CloseAllMenus();
    }
}
