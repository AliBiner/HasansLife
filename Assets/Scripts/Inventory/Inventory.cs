using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory and Items/Inventory")]
public class Inventory : ScriptableObject
{
    public InventorySlot[] itemSlot = new InventorySlot[6];
    public void AddItem(Item _item, int _amount) {
        for (int i = 0; i < itemSlot.Length; i++)
        {
           
            if (itemSlot[i].slotId == _item.Id && _item.stackable)
            {
                itemSlot[i].AddAmount(_amount);
                return;
            }
            

        }
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].slotId == 0)
            {
                itemSlot[i].UpdateSlot(_item.Id, _item, _amount);
                return;
            }
        }
    }
    public void UseItem(int index) {
      
        if (itemSlot[index].slotId != 0 && itemSlot[index].amount>=0)
        {
            
            itemSlot[index].item.UseEffect();
            itemSlot[index].amount -= 1;

        }
        if (itemSlot[index].amount==0)
        {
            
            FindObjectOfType<InventoryUi>().DeleteDisplay(index);
            itemSlot[index].slotId = 0;
            itemSlot[index].item = null;
            
            
            
        }

    }
}
