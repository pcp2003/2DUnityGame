using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int currentHealth;

    private int maxHealth;
    public float destroyCooldown = 3.0f; // Tempo de cooldown em segundos

    private bool isDying; // Controle para evitar múltiplas chamadas

    void Start(){
         
        maxHealth = currentHealth;
        isDying = false;
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDying)
        {
            isDying = true; // Marca que o objeto está em processo de destruição
            Debug.Log("Iniciando cooldown para destruir " + gameObject.name);
            gameObject.GetComponent<Animator>().SetBool("isDead", true);
            StartCoroutine(DestroyCooldown());
        }
    }

    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(destroyCooldown); // Aguarda o tempo especificado
        Debug.Log("Destruindo objeto: " + gameObject.name);
        Destroy(gameObject); // Destrói o objeto após o tempo
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (gameObject.tag.Equals("Player"))
            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

    }
}
