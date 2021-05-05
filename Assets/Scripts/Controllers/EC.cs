using UnityEngine;

public class EC : MonoBehaviour
{
    public float KillStreakTimer = 10f;
    public float timer = 0.0f;
    public int energy = 15; 
    public int energyMax = 100;
    public int KillStreak;
    public float KillstreakPower = 0.8f;
    public bool ActiveKillstreak = false;
    private int highestKillstreak = 0;
    private int energyEarned = 0;
    private AudioSource source;
    private GM gm;
    [System.Serializable]
    public class KillStreakSFX
    {
        public AudioClip announcerKillClip;
        public short threshold;
    }
    public KillStreakSFX[] killStreakSounds;

    public AudioClip firstBloodClip;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (gm == null)
            gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
    }

    // Update is called once per frame
    void Update()
    {
        if (energy <= 0)
        {
            //TODO: gameover function
        }
        timer -= Time.deltaTime; // tick up the killstreak timer
        if (timer < 0)
        {
            ActiveKillstreak = false;
        }
    }
    public int GetEnergy()
    {
        return energy;
    }
    public int GetEnergyMax()
    {
        return energyMax;
    }
    public int GetKillStreak()
    {
        return KillStreak;
    }
    /// <summary>
    /// Adds the given amount of energy to the players balance (after killstreak bonus)
    /// </summary>
    /// <param name="amount">The amount to give the player</param>
    /// <param name="kill">Was it a kill? If so we need to tick up the killstreak.</param>
    public void AddEnergy(int amount = 1, bool kill = false)
    {
        if (kill) // if we got a kill and it wasnt just a despawn or something
            UpdateKillStreak(); // update our killstreak
        // give the player energy based on my dumb fuckin function
        int x = Mathf.RoundToInt((Mathf.Log10(Mathf.Pow(KillStreak, 9) * KillstreakPower)) + 1);
        energy += x;
        energyEarned += x;
        if (energy > energyMax) // did that put them over the max energy?
            energy = energyMax; // if so, kick em right back to where they were. get outta here nerd
        if (energy < -1) // wtf?
            energy = 0; // gameover the fucker
    }

    /// <summary>
    /// Spends the energy, ie purchasing an object. 
    /// </summary>
    /// <param name="cost">Self explanatory.</param>
    /// <returns>True if the cost was successfully deducted, false if it wasn't.</returns>
    public bool SpendEnergy(int cost = 0)
    {
        if (!(energy - cost <= 1)) // always leave them with 1 energy. if they cant do that, dont buy it.
        {

            energy -= cost; // remove the energy
            Debug.Log("Spent " + cost + "e, leaving player with " + energy);
            return true; // tell the other function we took the money out of the players balance
        }
        Debug.Log("Could not spend " + cost + "e.");
        return false;
    }
    public void TakeDamage(int healthRemaining, bool isBoss)
    {
        energy = energy - (healthRemaining * 2);
        if (energy <= 0)
        {
            gm.GameOver(energyEarned, highestKillstreak);
        }
    }
    /// <summary>
    /// Update our current killstreak, and play any applicable SFX.
    /// </summary>
    void UpdateKillStreak()
    {
        if (timer < 0)
        {
            // too slow, git gud
            timer = KillStreakTimer;
            KillStreak = 1;
            ActiveKillstreak = false;
        }
        else
        {
            ActiveKillstreak = true;
            KillStreak++;
            if (KillStreak > highestKillstreak)
            {
                highestKillstreak = KillStreak;
            }
            foreach(KillStreakSFX SFX in killStreakSounds) // Check each of our sfx thresholds to see if we meet any of them
            {
                if (SFX.threshold == KillStreak)
                {
                    source.PlayOneShot(SFX.announcerKillClip, 1);
                }
            }
            // reset the timer to allow for some dope ass killstreaks
            timer = KillStreakTimer;
        }
    }
    public void PlayFirstBlood()
    {
        source.PlayOneShot(firstBloodClip, 1);
    }
}