using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    public InputAction MoveAction;
    private Rigidbody2D rigidbody2d;
    private Vector2 move;
    public float speed = 3.0f;
    public int health = 3; // Vida do Player

    // Variáveis relacionadas ao ataque
    private GameObject attackPoint;
    public float attackRange = 1.0f; // Alcance do ataque
    public int attackDamage = 1;     // Dano do ataque
    public LayerMask enemyLayer;     // Camada dos inimigos
    private float attackCooldown = 1.0f; // Duração do ataque (igual ao HasExitTime)

    private bool isAttacking = false; // Controle se o jogador está atacando

    public int inventorySize;
    public List<Key> keys;

    public float destroyCooldown = 3.0f; // Tempo de cooldown em segundos

    private Animator animator;
    private Vector2 moveDirection = new Vector2(0, 0);

    // AudioSources para diferentes eventos
    public AudioSource walkAudioSource;
    public AudioSource attackAudioSource;
    public static float volume = 1.0f;

    private int kills;

    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameObject.GetComponent<Animator>().SetBool("isDead", false);
        keys = new List<Key>(); // Inicializa a lista
        
        // Criando o ponto de ataque
        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = this.transform; // Define o Player como pai
        attackPoint.transform.localPosition = Vector3.zero; // Define a posição relativa ao Player como (0, 0, 0)

        walkAudioSource.volume *= volume;
        attackAudioSource.volume *= volume;

        kills = 0;
    }

    public void addKill () {
        kills++;
    }

    public int getKills () {
        return kills;
    }

    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetBool("isDead"))
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            StartCoroutine(DestroyCooldown());
            return;
        }

        // Leitura do movimento
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.magnitude, 0.0f) && !isAttacking) // Permite mudar de direção somente fora do ataque
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
            
            if (!walkAudioSource.isPlaying) walkAudioSource.Play(); // Toca o som de andar se o jogador estiver se movendo
        }
        else
        {
            walkAudioSource.Stop(); // Para o som de andar se o jogador não estiver se movendo
        }

        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Detectar ataque
        if (Mouse.current.leftButton.wasPressedThisFrame && !isAttacking) // Apenas permite atacar se não estiver atacando
        {
            // Debug.Log("Mouse Left Button Pressed");
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Animator>().GetBool("isDead")) return;
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    void Attack()
    {
        isAttacking = true; // Impede que outros ataques sejam iniciados durante a animação
        animator.SetTrigger("Attack");

        if (attackAudioSource != null) attackAudioSource.Play(); // Toca o som de ataque

        // Determina a posição do ponto de ataque com base na direção de movimento
        Vector3 newAttackPointPosition = attackPoint.transform.localPosition;

        if (moveDirection.x > 0) // Indo para a direita
        {
            newAttackPointPosition = new Vector3(0.15f, 0.14f, 0f);
        }
        else if (moveDirection.x < 0) // Indo para a esquerda
        {
            newAttackPointPosition = new Vector3(-0.15f, 0.14f, 0f);
        }
        else if (moveDirection.y > 0) // Indo para cima
        {
            newAttackPointPosition = new Vector3(0f, 0.3f, 0f);
        }
        else if (moveDirection.y < 0) // Indo para baixo
        {
            newAttackPointPosition = new Vector3(0f, 0f, 0f);
        }

        // Atualiza a posição do ponto de ataque
        attackPoint.transform.localPosition = newAttackPointPosition;

        // Detecta inimigos na área de ataque
        Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);

        if (enemiesColliders != null)
        {
            // Aplica dano aos inimigos detectados
            foreach (Collider2D enemyCollider in enemiesColliders)
            {
                EnemiesHealth enemyHealth = enemyCollider.GetComponent<EnemiesHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);

                    enemyCollider.GetComponent<Animator>().SetTrigger("Hit");
                }
            }
        }

        StartCoroutine(ResetAttackState()); // Espera o cooldown antes de permitir outro ataque
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

    }

    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(destroyCooldown); // Aguarda o tempo especificado
        Debug.Log("Destruindo objeto: " + gameObject.name);
        Destroy(gameObject); // Destrói o objeto após o tempo
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown); // Tempo do HasExitTime
        isAttacking = false; // Permite um novo ataque
    }

    public bool GetIsInventoryFull()
    {
        return keys.Count >= inventorySize;
    }

    public void AddKey(Key key)
    {
        if (!GetIsInventoryFull())
        {
            keys.Add(key);
            Debug.Log($"Chave {key.GetColor()} adicionada ao inventário. Total de chaves: {keys.Count}");
        }
    }

    public void UseKey(Key key)
    {
        keys.Remove(key);
    }

    public Key GetKeyByColor(string Color)
    {
        foreach (Key key in keys)
        {
            if (string.Equals(key.GetColor(), Color, StringComparison.OrdinalIgnoreCase) || string.Equals(key.GetColor(), "Golden", StringComparison.OrdinalIgnoreCase)) // Ignora maiúsculas/minúsculas
            {  
                return key;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
