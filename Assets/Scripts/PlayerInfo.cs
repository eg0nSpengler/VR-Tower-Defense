using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int maxEnergy = 100;
    public float energyMultiplier = 1;
    public float killstreakPower = 0.5f;
    public float damageMultiplier = 1f;
    public float trapMultiplier = 1f;
    public string prestigeName = "Antimatter Coins";
    public int prestigeCoins = 0;
    public int winstreak = 0;
}
