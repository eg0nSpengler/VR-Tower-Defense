using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ZoneController : MonoBehaviour
{
    public EC EnergyController;
    public TMPro.TextMeshProUGUI text;
    public TeleportArea teleportArea;
    public bool Purchased = false;
    public void PurchaseObject(int zoneCost)
    {
        Debug.Log("Trying to purchase new zone");
        if (EnergyController.SpendEnergy(zoneCost))
        {
            Purchased = true;
            text.text = "Purchased!";
            Debug.Log("Spent " + zoneCost + "e");
            //if (fromHand) // this is only for my sake. it makes the vending machine work in 2d debug.
            //    fromHand.TriggerHapticPulse(1000);
            teleportArea.locked = false;
        }
    }
}
