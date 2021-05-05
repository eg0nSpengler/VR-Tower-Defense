using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public short CurrentEnemies = 0;
    public short MaxEnemies = 15;
    private Transform Energy;
    private Transform Killstreak;
    public int KillCount = 0;
    public GameObject enemy;
    public EC EnergyController;
    public Canvas energyCanvas;
    public bool PlayerGameOver = false;
    private TMPro.TextMeshProUGUI counter;
    private TMPro.TextMeshProUGUI killTimer;
    private PlayerInfo player;
    private TMPro.TextMeshProUGUI gameOver;
    private TMPro.TextMeshProUGUI wave;
    private float x;
    public void Start()
    {
        Energy = energyCanvas.transform.Find(n: "Text_Energy");
        counter = Energy.GetComponent<TMPro.TextMeshProUGUI>();
        Killstreak = energyCanvas.transform.Find(n: "Text_KillTimer");
        killTimer = Killstreak.GetComponent<TMPro.TextMeshProUGUI>();
        gameOver = energyCanvas.transform.Find(n: "Text_GameOver").GetComponent<TMPro.TextMeshProUGUI>();
        wave = energyCanvas.transform.Find(n: "Text_Wave").GetComponent<TMPro.TextMeshProUGUI>();
        if (player == null)
            player = GameObject.FindWithTag("Player").GetComponent<PlayerInfo>();
    }
    public void Update()
    {
        if (EnergyController.ActiveKillstreak)
        {
            x = EnergyController.timer;
            killTimer.text = "Killstreak Timer: " + x.ToString();
        }
        UpdateUI();
    }
    public IEnumerator Fade()
    {
        //for (float f = 7f; f >= 0; f -= 0.1f)
        //{
        //    Color c = wave.GetComponent<Renderer>().material.color;
        //    c.a = f;
        //    wave.GetComponent<Renderer>().material.color = c;
        //    yield return null;
        //}
        float t = 0;
        Color startColor = new Color32(255, 255, 255, 255);
        Color endColor = new Color32(255, 255, 255, 0);

        wave.color = startColor;

        while (t < 1)
        {
            wave.color = Color.Lerp(startColor, endColor, t);
            t += Time.deltaTime / 5f;
            yield return null;
        }
    }
    public void SetWaveText(string x)
    {
        wave.text = x;
        StartCoroutine(Fade());

    }
    /// <summary>
    /// Returns true if it was able to add an enemy to the count. 
    /// </summary>
    /// <returns></returns>
    public bool AddEnemy()
    {
        // check if the player is around, if not, dont spawn the monster

        if (!PlayerGameOver && CurrentEnemies < MaxEnemies)
        {
            CurrentEnemies++;
            return true;
        } 
        return false;
    }
    /// <summary>
    /// Removes the root gameobject of the collided enemy,  and deals with contacting the EnergyController.
    /// It also checks if it was first blood, and calls the function for that. 
    /// </summary>
    /// <param name="collider"></param>
    public void RemoveEnemy(Collider collider, Rigidbody rigidbody = null, int damage = 0)
    {
        var rootGO = collider.gameObject.transform.root.gameObject;
        if (rootGO) // check if they still exist
        {
            if (rigidbody != null) // hit with weapon https://youtu.be/zQq12lDl0Kk?t=86
            {
                rootGO.GetComponent<EnemyInfo>().CurrentHealth -= Mathf.RoundToInt((rigidbody.velocity.magnitude * rigidbody.mass) * player.damageMultiplier);
                Debug.Log("Hit " + rootGO.name + " for " + rigidbody.velocity.magnitude * rigidbody.mass + " damage, leaving them with " + rootGO.GetComponent<EnemyInfo>().CurrentHealth);
            }
            rootGO.GetComponent<EnemyInfo>().CurrentHealth -= damage * Mathf.RoundToInt(player.trapMultiplier);
            
            if (rootGO.GetComponent<EnemyInfo>().CurrentHealth <= 0 && rootGO.GetComponent<EnemyInfo>().dead == false)
            {
                rootGO.GetComponent<EnemyInfo>().dead = true;
                if (KillCount == 0)
                {
                    EnergyController.PlayFirstBlood();
                }
                EnergyController.AddEnergy(1, true);
                UpdateUI();
                KillCount++;
                Destroy(collider.gameObject.transform.root.gameObject);
                Debug.Log("Destroyed " + collider.gameObject.transform.root.gameObject);
                CurrentEnemies--;
            }
        }

    }
    public void GameOver(int earnedEnergy, int highestKillstreak)
    {
        PlayerGameOver = true;
        float timer = 15f;
        int coins = (earnedEnergy / 100) + (highestKillstreak / 10);
        player.prestigeCoins += coins;
        gameOver.text = "Game Over" + "\n" + player.prestigeName + " earned: " + coins;
        counter.text = "";

        timer -= Time.deltaTime; // tick up the killstreak timer
        if (timer < 0)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }


    }
    public void UpdateUI()
    {
        if (!PlayerGameOver)
        {
            counter.text = "Energy:" + EnergyController.GetEnergy().ToString() + "/" + EnergyController.GetEnergyMax() + "\n";
            if (EnergyController.ActiveKillstreak)
            {
                counter.text += " Killstreak: " + EnergyController.GetKillStreak();
            }
        }
    }
}
