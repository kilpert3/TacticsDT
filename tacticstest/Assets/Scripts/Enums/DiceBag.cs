using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//framework for die roll simulation
public class DiceBag : MonoBehaviour
{

    private Random _rng;

    public DiceBag() {
        _rng = new Random();
    }

    //The default dice-rolling method. All methods link to this one.    
    private int InternalRoll( uint dice ) {
        int roll = 1 + _rng.Next((int)dice);
        Debug.Log("Dicebag rolled " + roll + "!");
        return roll;
    }

    // Rolls the specified dice.
    public int Roll( Dice d ) {
        return InternalRoll( ( uint )d );
    }

    // Rolls the chosen dice then adds a modifier to the rolled number.
    public int RollWithModifier( Dice dice , uint modifier ) {
        return InternalRoll( ( uint )dice ) + ( int )modifier;
    }

    // Rolls a series of dice and returns a collection containing them.
    public List<int> RollQuantity( Dice d , uint times ) {
        List<int> rolls = new List<int>();
        for( int i = 0 ; i < times ; i++ ) {
            rolls.Add( InternalRoll( ( uint )d ) );
        }
        return rolls;
    }
}

