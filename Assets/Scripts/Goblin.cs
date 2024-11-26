using UnityEngine;
using System.Collections;

public class Goblin : MonoBehaviour
{
    public Transform player;
    public float speed = 2.0f;
    public float attackDistance = 1.0f;
    public float attackRange = 1.0f;
    public float attackInterval = 1.0f;
    public int health = 3; // Vida do Goblin
    public int attackDamage = 1; // Dano ao jogador

    private float attackCooldown = 0.25f;

    public GameObject attackPoint; 

    public LayerMask enemyLayer;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private float distance;
    private float nextAttackTime = 0f;

    private bool isAttacking;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameObject.GetComponent<Animator>().SetBool("isDead", false);

        if (player == null)
        {
            Debug.LogError("Player reference not set on Goblin.");
        }
    }

    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetBool("isDead")) return;

        if (player != null )
        {
            Vector2 direction = player.position - transform.position;
            distance = direction.magnitude;

            if (distance > attackDistance)
            {
                isAttacking = false;
                moveDirection = direction.normalized;
                animator.SetFloat("Speed", distance);
            }
            else
            {
                isAttacking = true;
                animator.SetFloat("Speed", 0);

                if ( Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackInterval;
                }
            }

            if (!isAttacking)
                spriteRenderer.flipX = direction.x > 0 ? false : true;
        }
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Animator>().GetBool("isDead")) return;

        if (!isAttacking)
        {
            Vector2 position = rigidbody2d.position + moveDirection * speed * Time.fixedDeltaTime;
            rigidbody2d.MovePosition(position);
        }
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        if (moveDirection.x < 0 ){
            attackPoint.transform.localPosition = new Vector3(-0.15f,0.12f,0);
        }else{
            attackPoint.transform.localPosition = new Vector3(0.15f,0.12f,0);
        }

        // Fiz um array para possivel multiplayer  

        Collider2D [] playersColliders = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);

        if (playersColliders != null){

            foreach (Collider2D playerCollider in playersColliders ){

            playerCollider.GetComponent<Health>().TakeDamage(attackDamage);

            playerCollider.GetComponent<Animator>().SetTrigger("Hit");

            }
        } 

        StartCoroutine(ResetAttackState()); // Espera o cooldown antes de permitir outro ataque
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown); // Tempo do HasExitTime
        isAttacking = false; // Permite um novo ataque
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
