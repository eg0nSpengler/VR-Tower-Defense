using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Waves waves;
    public int waveNum = 0;
    public float timer;
    public bool spawning = false;
    private GM gm;
    void Start()
    {
        if (gm == null)
            gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
    }
    private void Update()
    {
        if (!spawning)
            timer += Time.deltaTime; // Tick up the timer
        
        if (waveNum != -1 && timer > waves.WaveListing[waveNum].WaveDelay) // Is it over spawntimer?
        {
            if (waveNum != -1)
            {
                StartCoroutine(SpawnWave());
                gm.SetWaveText(waves.WaveListing[waveNum].name);
            }

            timer = 0; // and reset our timer
        }
    }
    public IEnumerator SpawnWave()
    {
        int spawner = 0;
        // cache it
        var currentWave = waves.WaveListing[waveNum].wave;
        var EnemySpawn = currentWave.enemy;
  
        Spawner[] spawners = currentWave.spawners;
        float SpawnDelay = waves.WaveListing[waveNum].spawnDelay;
        for (int i = 0; i < currentWave.amount;)
        {
            spawning = true;
            i++;
            spawners[spawner].SpawnObject(currentWave.enemy, waves.WaveListing[waveNum].goal);
            spawner++;
            if (spawners.Length <= spawner)
            {
                spawner = 0; // Reset the spawner to the first one
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
        spawning = false;
        waveNum++;
        if (waveNum == waves.WaveListing.Length)
            waveNum = -1;
    }

}
