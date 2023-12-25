using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject[] slotobj = new GameObject[6];
    public GameObject slotPrefab;
    public GameObject inventoryPanel;
 
    
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateDisplay();

    }
    public void CreateDisplay() {
        
        for (int i = 0; i < playerInventory.itemSlot.Length; i++)
        {

            GameObject obj = Instantiate(slotPrefab, inventoryPanel.transform, false);
            Color color = obj.GetComponent<Image>().color;
            if (playerInventory.itemSlot[i].slotId != 0)
            {
                color.a = 1;
                obj.GetComponent<Image>().color = color;
                obj.GetComponent<Image>().sprite = playerInventory.itemSlot[i].item.Icon;

            }
            else {
                
                color.a = 0;
                obj.GetComponent<Image>().color = color;
            }
            slotobj[i] = obj;
            if (playerInventory.itemSlot[i].slotId == 0)
            {
                slotobj[i].GetComponentInChildren<Text>().text = "";

            }
            else {
                slotobj[i].GetComponentInChildren<Text>().text = playerInventory.itemSlot[i].amount.ToString();
            }
            
            obj.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.UseItem(Array.IndexOf(slotobj, obj)); });
            
        }
        
    }

    public void UpdateDisplay() {
     
        for (int i = 0; i < playerInventory.itemSlot.Length; i++)
        {
            if (playerInventory.itemSlot[i].slotId != 0)
            {
                Color color = slotobj[i].GetComponent<Image>().color;
                color.a = 1;
                slotobj[i].GetComponent<Image>().color = color;
                slotobj[i].GetComponent<Image>().sprite = playerInventory.itemSlot[i].item.Icon;
                slotobj[i].GetComponentInChildren<Text>().text = playerInventory.itemSlot[i].amount.ToString();

            }
        }

       
    }

    public void DeleteDisplay(int index) {
        slotobj[index].GetComponent<Image>().sprite = null;
        slotobj[index].GetComponentInChildren<Text>().text = "";
        Color color = slotobj[index].GetComponent<Image>().color;
        color.a = 0;
        slotobj[index].GetComponent<Image>().color = color;
    }

 
}
