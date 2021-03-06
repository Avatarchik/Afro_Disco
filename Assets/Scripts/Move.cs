﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A move that can be executed
/// Barebones class that holds move info
/// </summary>
public class Move : ICloneable
{
    public Dictionary<Vector2, string[]> Patterns = new Dictionary<Vector2, string[]>();
	public Dictionary<Vector2, Vector2> FiringPos = new Dictionary<Vector2, Vector2>();
    public Color Color { get; private set;}
    public string MoveName { get; private set; }
    public int Range { get; private set; }
    public int PushPower { get; private set; }



    //Instance variables
    public Vector2 origin { get; private set; }
    public Vector2 foundMoveCard { get; private set; }
    public int Priority { get; private set; }

    //Normal constructor
    public Move(string name, Color col, int range, int pushPower)
    {        
        Color = col;
        MoveName = name;
        PushPower = pushPower;
        Range = range;
    }


    //Priority constructor
    public Move(string name, Color col, int range, int pushPower, int priority)
    {
        Color = col;
        MoveName = name;
        PushPower = pushPower;
        Range = range;
        Priority = priority;
    }

    /// <summary>
    /// Give this move an origin and specific pattern
    /// </summary>
    /// <param name="v"></param>
    /// <param name="index"></param>
    public void SetFound(Vector2 Origin, Vector2 cardinality)
    {
        origin = Origin;
        foundMoveCard = cardinality;
    }


    /// <summary>
    /// Add a pattern that defines the move
    /// </summary>
    /// <param name="pat">string array of pattern, see board for matching</param>
    /// <param name="cardinality">the direction the move fires, used for push logic</param>
    public void AddPattern(string[] pat, Vector2 cardinality)
    {
        if (Patterns.ContainsKey(cardinality))
        {
            Debug.Log(MoveName + "has overlapping keys!");
            return;
        }
        Patterns.Add(cardinality, pat);
    }
		
	public void AddPattern(string[] pat, Vector2 cardinality, Vector2 firingPos)
	{
		if (Patterns.ContainsKey(cardinality))
		{
			Debug.Log(MoveName + "has overlapping keys!");
			return;
		}
		Patterns.Add(cardinality, pat);
		FiringPos.Add(cardinality, firingPos);
	}

    /// <summary>
    /// Get the found move pattern from this move instance, returns null if not found
    /// </summary>
    /// <returns></returns>
    public string[] GetFoundMove()
    {
        string[] a = null;
        Patterns.TryGetValue(foundMoveCard,out a);
        return a;
    }

	public Vector2 GetFoundMoveFiringPos()
	{
		Vector2 a;
		FiringPos.TryGetValue(foundMoveCard,out a);
		return a;
	}

    //Implement Iclonable so we can make instances from the move checker rencerence
    public object Clone()
    {
        return this.MemberwiseClone();
    }


    //Virtual methods to be implemented by children

    //Executes move logic and updates dancers on the board
    public virtual void Execute()
    {
        throw new Exception("Can't execute reference move, that's wack");
    }

    //Returns dancers in range
    public virtual List<Dancer> CheckRange()
    {
        throw new Exception("Can't execute reference move, that's wack");
    }

    //Checks if dancer is in range
    public virtual bool InRange()
    {
        throw new Exception("Can't execute reference move, that's wack");
    }
}
