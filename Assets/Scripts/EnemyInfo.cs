using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int reward;
    public bool isBoss;
    public int maxHealth;
    public int CurrentHealth { get; set; }
    public bool dead = false;
    private void Start()
    {
        CurrentHealth = maxHealth;
    }

}
