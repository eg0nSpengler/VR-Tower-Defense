using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public int energyReward;
        public WaveMakeup wave;
        public float WaveDelay = 10f;
        public float spawnDelay = 3f;
        public Transform goal;
        public Modifiers Modifiers { get; set; }
        // public WaveEnemies normalEnemies;
        // public ushort normalEnemyCount;
        // public int normalTimer;

        //  public WaveEnemies Bosses;
        // public ushort bossEnemyCount;
        //public int bossTimer;
    }
    [System.Serializable]
    public class WaveMakeup
    {
        public Spawner[] spawners;
        public GameObject enemy;
        public int amount;
    }
    public Wave[] WaveListing;

}
