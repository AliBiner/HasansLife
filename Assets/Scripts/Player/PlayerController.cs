using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] //inspector'da gösterir
    private float movementDirection;
    //[HideInInspector] -- inspector'da gizler
    public float movementSpeed;
    public float jumpPower;
    public float groundCheckRadius;
    public float trapCheckRadius;
    public float attackRate = 2f;
    private float nextAttack = 0f;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isNextLevel = false;
    private bool isTrap;

    public LayerMask trapLayer;
    public GameObject trapCheck;

    Rigidbody2D rb;
    public GameObject groundCheck;
    public LayerMask groundLayer;
    public LayerMask nextLevelLayer;


    Animator anim;

    public Transform attackPoint;
    public float attackRadius;
    public LayerMask enemyLayer;
    public float damage;
    public float skillCast;
    public float maxMana;
    float currentMana;
    bool isImmune;
    public float immunityTime;

    public Image manaBar;

    
    // Start is called before the first frame update
    void Start()
    {
   
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentMana = maxMana;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckRotation();
        Jump();
        CheckSurFace();
        TakeDamageOnTrap();
        CheckTrap();
        CheckAnimations();
        AttackInput();
        NextLevelCheck();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
            Destroy(PlayerStats.instance.gameManager);
            SceneManager.LoadScene(0,LoadSceneMode.Single);
            
        }
       
    }

    private void FixedUpdate(){
        Movement();  
    }

    void Movement(){
        movementDirection=Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(movementDirection*movementSpeed,rb.velocity.y);
        anim.SetFloat("runSpeed", Mathf.Abs(movementDirection*movementSpeed));

    }
    void CheckAnimations(){
        anim.SetBool("isGrounded",isGrounded);
        anim.SetFloat("yVelocity",rb.velocity.y);
    }

    void CheckRotation(){
        if(!isFacingRight && movementDirection>0){
            Flip();
        }
        else if(isFacingRight && movementDirection<0){
            Flip();
        }
    }

    void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void CheckSurFace(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position,groundCheckRadius,groundLayer);
    }
    void CheckTrap()
    {
        isTrap = Physics2D.OverlapCircle(trapCheck.transform.position, trapCheckRadius, trapLayer);
    }
    void Jump(){
        if(isGrounded){
            if(Input.GetKeyDown(KeyCode.Space)){
                rb.velocity = new Vector2(rb.velocity.x,jumpPower);
            }
        }
        
    }
    public void Attack(){
        
        if (currentMana <= 0)
        {
            return;
        }
        else {
            if (currentMana<skillCast)
            {
                return;
            }
            else
            {
                float numb = Random.Range(0, 2);
                if (numb == 0)
                {
                    anim.SetTrigger("Attack1");
                }
                else
                {
                    anim.SetTrigger("Attack2");
                }

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyStats>().TakeDamage(damage);
                    currentMana -= skillCast;
                    manaBar.fillAmount = currentMana / 100;
                }
            }
           
        }
        


        
    }
    public void TakeMana(int mana) {
        currentMana += mana;
        manaBar.fillAmount = currentMana / 100;
    } 
    public void NextLevelCheck() {
        isNextLevel = Physics2D.OverlapCircle(attackPoint.position, attackRadius, nextLevelLayer);
        if (isNextLevel == true)
        {
            if (PlayerStats.instance.nextLevelIndex==4)
            {
                Destroy(gameObject);
                Destroy(PlayerStats.instance.gameManager);
                SceneManager.LoadScene(0);
            }
            transform.position = PlayerStats.instance.playerPosition;
            SceneManager.LoadScene(PlayerStats.instance.nextLevelIndex);
            PlayerStats.instance.nextLevelIndex += 1;
        }
    }
    public void AttackInput(){
        if(Time.time > nextAttack){
            if(Input.GetKeyDown(KeyCode.LeftControl)){
                Attack();
                nextAttack = Time.time + 1f / attackRate;
            }
        }
        
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(groundCheck.transform.position,groundCheckRadius);
        Gizmos.DrawWireSphere(trapCheck.transform.position, trapCheckRadius);
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }

    void TakeDamageOnTrap() {
        if (isTrap && !isImmune)
        {
            if (PlayerStats.instance.health<=0)
            {
                PlayerStats.instance.health = 0;
                Destroy(gameObject);
            }
            PlayerStats.instance.health -= 10;
            StartCoroutine(Immunity()); 
        }
        
    }

    IEnumerator Immunity()
    {
        isImmune = true;
        isGrounded = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        yield return new WaitForSeconds(immunityTime);
        isImmune = false;
        isGrounded = false;
    }

}
