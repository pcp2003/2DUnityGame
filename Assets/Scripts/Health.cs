using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int currentHealth;

    private int maxHealth;

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
             
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (gameObject.tag.Equals("Player"))
            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);

    }
}
