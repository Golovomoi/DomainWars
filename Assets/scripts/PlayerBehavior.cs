using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior instance;
    public int PlayerId { get; set; }
    public Vector3Int SelectedField { get; set; }
    public int CurrentDiamondsAmmount { get; set; }
    public int CurrentWoodAmmount { get; set; }
    public int CurrentForceAmmount { get; set; }
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
        Debug.Log("Player awaken ");
    }
    // Start is called before the first frame update
    void Start()
    {
        SelectedField = new Vector3Int(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerState();
    }
    private void FixedUpdate()
    {
        
    }
    private void UpdatePlayerState()
    {
        CurrentDiamondsAmmount = PlayersInteractions.instance.GetPlayerDiamonds(PlayerId);
        CurrentForceAmmount = PlayersInteractions.instance.GetPlayerForce(PlayerId);
        CurrentWoodAmmount = PlayersInteractions.instance.GetPlayerWood(PlayerId);
    }
}
