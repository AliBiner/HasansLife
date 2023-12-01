using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] //inspector'da gösterir
    private float movementDirection;
    //[HideInInspector] -- inspector'da gizler
    public float movementSpeed;
    public float jumpPower;
    public float groundCheckRadius;
    public float attackRate = 2f;
    private float nextAttack = 0f;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isNextLevel = false;

    Rigidbody2D rb;
    public GameObject groundCheck;
    public LayerMask groundLayer;
    public LayerMask nextLevelLayer;

    Animator anim;

    public Transform attackPoint;
    public float attackRadius;
    public LayerMask enemyLayer;
    public float damage;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        CheckRotation();
        Jump();
        CheckSurFace();
        CheckAnimations();
        AttackInput();
        NextLevelCheck();
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
    void Jump(){
        if(isGrounded){
            if(Input.GetKeyDown(KeyCode.Space)){
                rb.velocity = new Vector2(rb.velocity.x,jumpPower);
            }
        }
        
    }

    public void Attack(){
        float numb =  Random.Range(0,2);
        if(numb == 0){
            anim.SetTrigger("Attack1");
        }
        else{
            anim.SetTrigger("Attack2");
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRadius,enemyLayer);

        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<EnemyStats>().TakeDamage(damage);
        }
        
    }
    public void NextLevelCheck() {
        isNextLevel = Physics2D.OverlapCircle(attackPoint.position, attackRadius, nextLevelLayer);
        if (isNextLevel == true)
        {
            SceneManager.LoadScene(2);
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
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }

}
