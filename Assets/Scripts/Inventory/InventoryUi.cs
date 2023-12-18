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
       


    }
    public void CreateDisplay() {
        
        for (int i = 0; i < playerInventory.itemSlot.Length; i++)
        {
            GameObject obj = Instantiate(slotPrefab, inventoryPanel.transform, false);
            if (playerInventory.itemSlot[i].slotId!=0)
            {
               
                obj.GetComponent<Image>().sprite = playerInventory.itemSlot[i].item.Icon;
            }
           
            slotobj[i] = obj;
            obj.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.UseItem(Array.IndexOf(slotobj, obj)); });
        }
        
    }

    public void UpdateDisplay() {
     
        for (int i = 0; i < playerInventory.itemSlot.Length; i++)
        {
            if (playerInventory.itemSlot[i].slotId != 0)
            {
              
                slotobj[i].GetComponent<Image>().sprite = playerInventory.itemSlot[i].item.Icon;
                slotobj[i].GetComponentInChildren<Text>().text = playerInventory.itemSlot[i].amount.ToString();

            }
       
           

        }
       
    }

 
}
