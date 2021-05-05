using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using TMPro;

public class VendingController : MonoBehaviour
{
    public int SelectedObject = 0;
    public VendingObject Vendable;
    public EC EnergyController;
    public Transform spawnLocation;
    public GameObject text;
    private TextMeshProUGUI purchaseText;
    public void Start()
    {
        var stock = Vendable.vendableObjects[SelectedObject].stock.ToString() + " Left";
        purchaseText = text.GetComponent<TextMeshProUGUI>();
        purchaseText.text = Vendable.vendableObjects[SelectedObject].name + " - " + Vendable.vendableObjects[SelectedObject].energyCost.ToString() + "e " + stock;
    }
    /// <summary>
    /// Don't run this yourself! 
    /// Purchases the current selected object.
    /// </summary>
    public void PurchaseObject()
    {
        Debug.Log("Trying to purchase " + Vendable.vendableObjects[SelectedObject]);
        // if there's stock, and we can afford it, buy it
        if (Vendable.vendableObjects[SelectedObject].stock > 0 && EnergyController.SpendEnergy(Vendable.vendableObjects[SelectedObject].energyCost))
        {
            Vendable.vendableObjects[SelectedObject].stock--;
            Debug.Log("Spent " + Vendable.vendableObjects[SelectedObject].energyCost + "e");
            // change the button to green to show the purchase went through
            ColorSelf(Color.green);
            //if (fromHand) // this is only for my sake. it makes the vending machine work in 2d debug.
            //    fromHand.TriggerHapticPulse(1000);
            var Vended = Instantiate(
                Vendable.vendableObjects[SelectedObject].vendedObject,
                spawnLocation.transform.position,
                spawnLocation.transform.rotation);
            ChangeSelectedItem(0); // LAZY HACK MAKE THIS BETTER THIS IS BAD VERY BAD
        }
        else
        {
            ColorSelf(Color.red); // make it red
        }
    }
    public void ChangeSelectedItem(int amountToMove)
    {
        if (SelectedObject + amountToMove < 0)
            SelectedObject = Vendable.vendableObjects.Length-1;
        else if (SelectedObject + amountToMove > Vendable.vendableObjects.Length-1)
            SelectedObject = 0;
        else
            SelectedObject += amountToMove;
        var stock = Vendable.vendableObjects[SelectedObject].stock.ToString() + " Left";
        if (Vendable.vendableObjects[SelectedObject].stock == 0)
        {
            stock = "Sold out!";
        }
        purchaseText.text = Vendable.vendableObjects[SelectedObject].name + " - " + Vendable.vendableObjects[SelectedObject].energyCost.ToString() + "e " + stock;
    }
    private void ColorSelf(Color newColor)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
        {
            renderers[rendererIndex].material.color = newColor;
        }
    }
}
