using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[attach to battle Controller]
public class ComputerPlayer : MonoBehaviour
{
    BattleController bc;
    Unit actor { get { return bc.turn.actor; } }

    //convenience wrapper for getting selected unit faction
    Faction faction { get { return actor.GetComponent<Faction>(); } }
    Unit nearestFoe;

    void Awake()
    {
        bc = GetComponent<BattleController>();
    }

    // Create and fill out a plan of attack
    public PlanOfAttack Evaluate()
    {
        
        PlanOfAttack poa = new PlanOfAttack();

        // Step 1: Decide what ability to use
        AttackPattern pattern = actor.GetComponentInChildren<AttackPattern>();
        if (pattern)
            pattern.Pick(poa);
        else
            DefaultAttackPattern(poa);

        // Step 2: Determine where to move and aim to best use the ability
        if (IsPositionIndependent(poa))
            PlanPositionIndependent(poa);
        else if (IsDirectionIndependent(poa))
            PlanDirectionIndependent(poa);
        else
            PlanDirectionDependent(poa);

        if (poa.ability == null)
            MoveTowardOpponent(poa);

        // Return the completed plan
        return poa;
    }

    void DefaultAttackPattern(PlanOfAttack poa)
    {
        // Just get the first "Attack" ability
        poa.ability = actor.GetComponentInChildren<Ability>();
        poa.target = Targets.Foe;
    }

    //check if position matters when using an ability. currently, will move at random if position is independent
    bool IsPositionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return range.positionOriented == false;
    }

    void PlanPositionIndependent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
        poa.moveLocation = poa.fireLocation = tile.pos;
    }

    //when position matters, facing angle does not. Checks every possible move position and firing range to decide best use of ability
    bool IsDirectionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return !range.directionOriented;
    }

    void PlanDirectionIndependent(PlanOfAttack poa)
    {
        Tile startTile = actor.tile;
        Dictionary<Tile, AttackOption> map = new Dictionary<Tile, AttackOption>();
        AbilityRange ar = poa.ability.GetComponent<AbilityRange>();
        List<Tile> moveOptions = GetMoveOptions();

        for (int i = 0; i < moveOptions.Count; ++i)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);
            List<Tile> fireOptions = ar.GetTilesInRange(bc.board);

            for (int j = 0; j < fireOptions.Count; ++j)
            {
                Tile fireTile = fireOptions[j];
                AttackOption ao = null;
                if (map.ContainsKey(fireTile))
                {
                    ao = map[fireTile];
                }
                else
                {
                    ao = new AttackOption();
                    map[fireTile] = ao;
                    ao.target = fireTile;
                    ao.direction = actor.dir;
                    RateFireLocation(poa, ao);
                }

                ao.AddMoveTarget(moveTile);
            }
        }

        actor.Place(startTile);
        List<AttackOption> list = new List<AttackOption>(map.Values);
        PickBestOption(poa, list);
    }

    //position and facing angle matter. checks every facing for best option
    void PlanDirectionDependent(PlanOfAttack poa)
    {
        Tile startTile = actor.tile;
        Directions startDirection = actor.dir;
        List<AttackOption> list = new List<AttackOption>();
        List<Tile> moveOptions = GetMoveOptions();

        for (int i = 0; i < moveOptions.Count; ++i)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);

            for (int j = 0; j < 4; ++j)
            {
                actor.dir = (Directions)j;
                AttackOption ao = new AttackOption();
                ao.target = moveTile;
                ao.direction = actor.dir;
                RateFireLocation(poa, ao);
                ao.AddMoveTarget(moveTile);
                list.Add(ao);
            }
        }

        actor.Place(startTile);
        actor.dir = startDirection;
        PickBestOption(poa, list);
    }
    
    //return list of reachable tiles
    List<Tile> GetMoveOptions()
    {
        return actor.GetComponent<Movement>().GetTilesInRange(bc.board);
    }

    //prepare to score attack options by adding marks to each tile with available targets 
    void RateFireLocation(PlanOfAttack poa, AttackOption option)
    {
        AbilityArea area = poa.ability.GetComponent<AbilityArea>();
        List<Tile> tiles = area.GetTilesInArea(bc.board, option.target.pos);
        option.areaTargets = tiles;
        option.isCasterMatch = IsAbilityTargetMatch(poa, actor.tile);

        for (int i = 0; i < tiles.Count; ++i)
        {
            Tile tile = tiles[i];
            if (actor.tile == tiles[i] || !poa.ability.IsTarget(tile))
                continue;

            bool isMatch = IsAbilityTargetMatch(poa, tile);
            option.AddMark(tile, isMatch);
        }
    }

    //determine which marks are a match. Currently, it simply looks at unit faction
    //hitting an enemy = match mark
    //hitting an ally = !match mark
    bool IsAbilityTargetMatch(PlanOfAttack poa, Tile tile)
    {
        bool isMatch = false;
        if (poa.target == Targets.Tile)
            isMatch = true;
        else if (poa.target != Targets.None)
        {
            Faction other = tile.content.GetComponentInChildren<Faction>();
            if (other != null && faction.IsMatch(other, poa.target))
                isMatch = true;
        }

        return isMatch;
    }

    //this method calculates the scores for each option based on marks received.
    //if no matches are found, no ability is performed
    void PickBestOption(PlanOfAttack poa, List<AttackOption> list)
    {
        int bestScore = 1;
        List<AttackOption> bestOptions = new List<AttackOption>();
        for (int i = 0; i < list.Count; ++i)
        {
            AttackOption option = list[i];
            int score = option.GetScore(actor, poa.ability);
            if (score > bestScore)
            {
                bestScore = score;
                bestOptions.Clear();
                bestOptions.Add(option);
            }
            else if (score == bestScore)
            {
                bestOptions.Add(option);
            }
        }

        if (bestOptions.Count == 0)
        {
            poa.ability = null; // Clear ability as a sign not to perform it
            return;
        }

        List<AttackOption> finalPicks = new List<AttackOption>();
        bestScore = 0;
        for (int i = 0; i < bestOptions.Count; ++i)
        {
            AttackOption option = bestOptions[i];
            int score = option.bestAngleBasedScore;
            if (score > bestScore)
            {
                bestScore = score;
                finalPicks.Clear();
                finalPicks.Add(option);
            }
            else if (score == bestScore)
            {
                finalPicks.Add(option);
            }
        }

        AttackOption choice = finalPicks[UnityEngine.Random.Range(0, finalPicks.Count)];
        poa.fireLocation = choice.target.pos;
        poa.attackDirection = choice.direction;
        poa.moveLocation = choice.bestMoveTile.pos;
    }
   

    //in the case that no use for the ability was found, AI will search for nearest enemy unit and approach.
    //This method uses a board search to find the closest enemy without KO status
    void FindNearestFoe()
    {
        nearestFoe = null;
        bc.board.Search(actor.tile, delegate (Tile arg1, Tile arg2) {
            if (nearestFoe == null && arg2.content != null)
            {
                Faction other = arg2.content.GetComponentInChildren<Faction>();
                if (other != null && faction.IsMatch(other, Targets.Foe))
                {
                    Unit unit = other.GetComponent<Unit>();
                    Stats stats = unit.GetComponent<Stats>();
                    if (stats[StatTypes.HP] > 0)
                    {
                        nearestFoe = unit;
                        return true;
                    }
                }
            }
            return nearestFoe == null;
        });
    }

    //Uses the find nearest foe method and traces back on the path for an appropriate move location within range
    void MoveTowardOpponent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        FindNearestFoe();
        if (nearestFoe != null)
        {
            Tile toCheck = nearestFoe.tile;
            while (toCheck != null)
            {
                if (moveOptions.Contains(toCheck))
                {
                    poa.moveLocation = toCheck.pos;
                    return;
                }
                toCheck = toCheck.prev;
            }
        }

        poa.moveLocation = actor.tile.pos;
    }

    //Finds nearest foe to determine which way to face once finished with turn
    public Directions DetermineEndFacingDirection()
    {
        Directions dir = (Directions)UnityEngine.Random.Range(0, 4);
        FindNearestFoe();
        if (nearestFoe != null)
        {
            Directions start = actor.dir;
            for (int i = 0; i < 4; ++i)
            {
                actor.dir = (Directions)i;
                if (nearestFoe.GetFacing(actor) == Facings.Front)
                {
                    dir = actor.dir;
                    break;
                }
            }
            actor.dir = start;
        }
        return dir;
    }
}