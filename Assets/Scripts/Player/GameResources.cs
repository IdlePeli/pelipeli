using System;

public static class GameResources
{
    public static event EventHandler OnWoodAmountChanged;
    public static event EventHandler OnStoneAmountChanged;

    private static int _woodAmount;
    private static int _stoneAmount;

    public static void AddWoodAmount(int amount)
    {
        _woodAmount += amount;
        OnWoodAmountChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void AddStoneAmount(int amount)
    {
        _stoneAmount += amount;
        OnStoneAmountChanged?.Invoke(null, EventArgs.Empty);
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
