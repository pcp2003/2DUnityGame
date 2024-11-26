using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int health;
    public float destroyColldown = 3.0f; // Tempo de cooldown em segundos

    private bool isDying = false; // Controle para evitar múltiplas chamadas

    void Update()
    {
        if (health <= 0 && !isDying)
        {
            isDying = true; // Marca que o objeto está em processo de destruição
            Debug.Log("Iniciando cooldown para destruir " + gameObject.name);
            gameObject.GetComponent<Animator>().SetBool("isDead", true);
            StartCoroutine(DestroyColldown());
        }
    }

    IEnumerator DestroyColldown()
    {
        yield return new WaitForSeconds(destroyColldown); // Aguarda o tempo especificado
        Debug.Log("Destruindo objeto: " + gameObject.name);
        Destroy(gameObject); // Destrói o objeto após o tempo
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.tag + " took damage");
    }
}
