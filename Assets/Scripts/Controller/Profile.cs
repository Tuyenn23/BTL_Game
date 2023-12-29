using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile
{
    public void AddGold(int goldBonus, string _analytic)
    {
        int _count = GetGold() + goldBonus;
        PlayerDataManager.Instance.SetGold(_count);
    }

    public int GetGold()
    {
        return PlayerDataManager.Instance.GetGold();
    }

    public void AddKey(int amount, string _analytic)
    {

        PlayerDataManager.Instance.SetKey(GetKey() + amount);
    }

    public int GetKey()
    {
        return PlayerDataManager.Instance.GetKey();
    }
}
