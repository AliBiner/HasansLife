using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Food Item",menuName ="Inventory and Items/Food")]
public class Food : Item
{
    public ItemType itemType = ItemType.Food;
    public int health;
    public int mana;
    public PlayerHealth playerHealth;
    public override void UseEffect()
    {
        FindObjectOfType<PlayerHealth>().TakeHealth(health);
       
        
    }
}
