using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public GameObject deathEffect;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth<=0){
            Instantiate(deathEffect, transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}
