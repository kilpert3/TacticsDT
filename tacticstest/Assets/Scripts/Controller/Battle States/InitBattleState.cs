using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//INITIALIZE BATTLE
public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        //wait one frame for transition
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits(); //  Dummy method for testing
        AddVictoryCondition(); //  victory condition testing
        yield return null;
        owner.ChangeState<CutSceneState>(); // Enter Test cutscene at start
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
    }

    //CODE FOR TESTING ONLY, creates units manually
    void SpawnTestUnits()
    {
        string[] recipes = new string[]
        {
            "Alaois",
            "Hania",
            "Kamau",
            "Enemy Rogue",
            "Enemy Warrior",
            "Enemy Wizard"
        };

        List<Tile> locations = new List<Tile>(board.tiles.Values);
        for (int i = 0; i < recipes.Length; ++i)
        {
            int level = 1;
            GameObject instance = UnitFactory.Create(recipes[i], level);

            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];
            locations.RemoveAt(random);

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            unit.dir = (Directions)UnityEngine.Random.Range(0, 4);
            unit.Match();

            units.Add(unit);
        }

        SelectTile(units[0].tile.pos);
    }

    //MORE TEST CODE
    void AddVictoryCondition()
    {
        DefeatBossVictoryCondition vc = owner.gameObject.AddComponent<DefeatBossVictoryCondition>();
        Unit enemy = units[units.Count - 1];
        vc.target = enemy;
        Health health = enemy.GetComponent<Health>();
        health.MinHP = 1;
    }
}