using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    public AbilityMenuPanelController abilityMenuPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();
    public StatPanelController statPanelController;
    public IEnumerator round;

    //PROTOTYPE ONLY (actual models thru resources.load())
    public GameObject heroPrefab;
    public Tile currentTile { get { return board.GetTile(pos); } }

    void Start()
    {
        ChangeState<InitBattleState>();
    }
}