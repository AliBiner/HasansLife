using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public Image healthBar;
    bool isImmune;
    public float immunityTime;
    Animator anim;
    public Inventory playerInventory;
    InventoryUi inventoryUi;
    public GameObject transferObject;
    //float currentHelath;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        inventoryUi = GetComponent<InventoryUi>();
        //if (PlayerStats.instance!=null)
        //{
        //        PlayerStats.instance.health = maxHealth;
        //}
        PlayerStats.instance.health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.instance.health >= maxHealth)
        {
            PlayerStats.instance.health = maxHealth;
        }
        healthBar.fillAmount = PlayerStats.instance.health / 100;
        if (PlayerStats.instance.health<=0)
        {
            Destroy(gameObject);
            Destroy(PlayerStats.instance.gameManager);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
           if (collision.CompareTag("Enemy") && !isImmune)
        {
            PlayerStats.instance.health -= collision.GetComponent<EnemyStats>().damage;
            StartCoroutine(Immunity());
            anim.SetTrigger("Hit");
            if (PlayerStats.instance.health <= 0)
            {
                PlayerStats.instance.health = 0;
                Destroy(gameObject);
                Destroy(PlayerStats.instance.gameManager);
                SceneManager.LoadScene(0,LoadSceneMode.Single);
            }
        }
       
        if (collision.CompareTag("Inventory"))
        {
            WorldItem worldItem = collision.GetComponent<WorldItem>();
            playerInventory.AddItem(worldItem.item, worldItem.amount);
            Destroy(worldItem.gameObject);
            inventoryUi.UpdateDisplay();

        }
       
    }

    IEnumerator Immunity() { 
        isImmune = true;
        yield return new WaitForSeconds(immunityTime);
        isImmune=false;
    }

    public void TakeHealth(int health) {
        PlayerStats.instance.health += health;
        inventoryUi.UpdateDisplay();
    }
}
