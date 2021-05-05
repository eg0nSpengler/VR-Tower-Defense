using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public GameObject ObjectToSpawn;
    private Spawner spawner;
    public bool Despawn = false;
    public bool IgnoreGM = false;
    public int DespawnTimer = 0;
   //public float SpawnTimer = 5.5f;
    //private float timer = 0.0f;
    private GM gm;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Spawner>();
        if (gm == null)
            gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
    }
    /// <summary>
    /// Spawns the given enemy, and adds it to the current enemy count.
    /// </summary>
    public void SpawnObject(GameObject Enemy, Transform goal)
    {
        if (Enemy) // if we have an object to spawn
        {
            if (gm != null) // and we have a gm object
            {
                bool canAdd = gm.AddEnemy(); // and we're allowed to actually add it
                if (IgnoreGM)
                    canAdd = true;
                if (canAdd)
                {
                    // cool, we got access to add it. lets instantiate
                    var migrator = Instantiate(
                    Enemy,
                    spawner.transform.position,
                    spawner.transform.rotation);
                    migrator.GetComponent<MoveTo>().goal = goal;
                    if (Despawn) // if despawn is ticked, despawn it in the specified time. this shouldnt be used in 99.9999999% of cases
                    {
                        Destroy(migrator, DespawnTimer);
                    }
                }
            }
            else
            {
                Debug.LogError("Spawner.cs: No GM found! Did you attach it to the spawner prefab?");
            }
        }
        else
        {
            Debug.LogError("Spawner.cs: No given object to spawn!");
        }

    }
}
