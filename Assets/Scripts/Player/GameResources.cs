using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResources
{
    public static event EventHandler OnWoodAmountChanged;
    public static event EventHandler OnStoneAmountChanged;

    private static int _woodAmount;
    private static int _stoneAmount;

    public static void AddWoodAmount(int amount)
    {
        _woodAmount += amount;
        if (OnWoodAmountChanged != null) OnWoodAmountChanged(null, EventArgs.Empty);
    }

    public static void AddStoneAmount(int amount)
    {
        _stoneAmount += amount;
        if (OnStoneAmountChanged != null) OnStoneAmountChanged(null, EventArgs.Empty);
    }

    public static int GetWoodAmount()
    {
        return _woodAmount;
    }
    
    public static int GetStoneAmount()
    {
        return _stoneAmount;
    }
}
