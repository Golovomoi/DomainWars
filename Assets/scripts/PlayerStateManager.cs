using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
    public Text DiamondAmount;
    public Text WoodAmount;
    public Text ForceAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DiamondAmount.text = PlayerBehavior.instance.CurrentDiamondsAmmount.ToString();
        WoodAmount.text = PlayerBehavior.instance.CurrentWoodAmmount.ToString();
        ForceAmount.text = PlayerBehavior.instance.CurrentForceAmmount.ToString();
    }
}
