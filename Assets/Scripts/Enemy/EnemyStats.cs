using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public GameObject deathEffect;
    public float timer;
    public float damage;

    HitEffect effect;
    Rigidbody2D rb;
    public float knockBackForceX, knockBackForceY;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        effect = GetComponent<HitEffect>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerStats.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage){
        currentHealth -= damage;
       
        if (player.position.x < transform.position.x)
        {
            rb.AddForce(new Vector2(knockBackForceX, knockBackForceY), ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(new Vector2(-knockBackForceX, knockBackForceY), ForceMode2D.Force);
        }
        GetComponent<SpriteRenderer>().material = effect.white;
        StartCoroutine(BackToNormal());
        if(currentHealth<=0){
            Instantiate(deathEffect, transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator BackToNormal() {
        yield return new WaitForSeconds(timer);
        GetComponent<SpriteRenderer>().material = effect.original;
    }
}
