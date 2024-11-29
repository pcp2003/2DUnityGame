using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soldier : MonoBehaviour
{
    private Transform playerReference;
    public float speed = 2.0f;
    public float attackDistance = 1.0f;
    public float attackRange = 1.0f;
    public float attackInterval = 1.0f;
    public int health = 3; // Vida do Soldier
    public int attackDamage = 1; // Dano ao jogador
    private float raycastOffset = 0.5f;        // Offset do raycast para detectar obstáculos
    public float avoidObstacleDistance = 1.0f; // Distância para evitar obstáculos
    public List<GameObject> possibleKeys; // Lista de prefabs de chaves possíveis
    private float attackCooldown = 0.25f;
    private GameObject attackPoint;
    public LayerMask enemyLayer;
    public float destroyCooldown = 3.0f; // Tempo de cooldown em segundos
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;
    private float distance;
    private float nextAttackTime = 0f;
    private bool isAttacking;
    public float chanceToDropKey;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameObject.GetComponent<Animator>().SetBool("isDead", false);

        // Criando o ponto de ataque
        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = this.transform; // Define o Player como pai
        attackPoint.transform.localPosition = Vector3.zero; // Define a posição relativa ao Player como (0, 0, 0)
    }

    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetBool("isDead"))
        {   
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            StartCoroutine(DestroyCooldown()); 

            return;
        }

        if (playerReference != null)
        {
            Vector2 direction = playerReference.position - transform.position;

            // Detectar e evitar obstáculos
            Vector2 adjustedDirection = AvoidObstacles(direction);

            // Atualiza a direção do movimento
            moveDirection = adjustedDirection;

            distance = moveDirection.magnitude;

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

                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackInterval;
                }
            }

            if (!isAttacking)
                spriteRenderer.flipX = direction.x > 0 ? false : true;
        }
    }

    public void SetPlayerReference (GameObject player) {
        playerReference = player.transform;
    }

    private Vector2 AvoidObstacles(Vector2 direction)
    {
        // Raycasts para detectar obstáculos
        Vector2 leftRayOrigin = (Vector2)transform.position + Vector2.left * raycastOffset;
        Vector2 rightRayOrigin = (Vector2)transform.position + Vector2.right * raycastOffset;

        RaycastHit2D hitFront = Physics2D.Raycast(transform.position, direction, avoidObstacleDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(leftRayOrigin, direction, avoidObstacleDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(rightRayOrigin, direction, avoidObstacleDistance);

        // Se houver obstáculo à frente, ajusta a direção
        if (hitFront.collider != null)
        {
            Debug.Log("Obstacle detected ahead: " + hitFront.collider.name);

            // Tenta desviar para a esquerda ou direita
            if (hitLeft.collider == null)
                return Vector2.Perpendicular(direction); // Direção à esquerda
            else if (hitRight.collider == null)
                return -Vector2.Perpendicular(direction); // Direção à direita
        }

        // Caso não detecte obstáculos, mantém a direção
        return direction;
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

        float differentAttacksChance = 50.0f;

        float randomChance = UnityEngine.Random.Range(0f, 100f);

        if (randomChance > differentAttacksChance)
        {
            animator.SetTrigger("Attack01");
        }else {
            animator.SetTrigger("Attack02");
        }

        

        if (moveDirection.x < 0)
        {
            attackPoint.transform.localPosition = new Vector3(-0.15f, 0.12f, 0);
        }
        else
        {
            attackPoint.transform.localPosition = new Vector3(0.15f, 0.12f, 0);
        }

        // Fiz um array para possivel multiplayer  

        Collider2D[] playersColliders = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);

        if (playersColliders != null)
        {

            foreach (Collider2D playerCollider in playersColliders)
            {

                playerCollider.GetComponent<Health>().TakeDamage(attackDamage);

                playerCollider.GetComponent<Animator>().SetTrigger("Hit");

            }
        }

        StartCoroutine(ResetAttackState()); // Espera o cooldown antes de permitir outro ataque
    }

    void DropKey()
    {

        // Gera um número aleatório entre 0 e 100
        float randomChance = UnityEngine.Random.Range(0f, 100f);

        // Se a chance não for atingida, não dropa nenhuma chave
        if (randomChance > chanceToDropKey)
        {
            Debug.Log("No key dropped. Chance: " + randomChance + "%");
            return;
        }

        // Continua com o drop de uma chave aleatória
        if (possibleKeys != null && possibleKeys.Count > 0)
        {
            // Escolhe uma chave aleatória da lista
            int randomIndex = UnityEngine.Random.Range(0, possibleKeys.Count);
            GameObject selectedKey = possibleKeys[randomIndex];

            // Instancia a chave selecionada na posição do Soldier
            Instantiate(selectedKey, transform.position, Quaternion.identity);
            Debug.Log($"Key dropped: {selectedKey.name} at position: {transform.position}");
        }
        else
        {
            Debug.LogWarning("No possible keys assigned to Soldier.");
        }
    }


    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(destroyCooldown); // Aguarda o tempo especificado
        Debug.Log("Destruindo objeto: " + gameObject.name);
        DropKey();
        Destroy(gameObject); // Destrói o objeto após o tempo
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown); // Tempo do HasExitTime
        isAttacking = false; // Permite um novo ataque
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
