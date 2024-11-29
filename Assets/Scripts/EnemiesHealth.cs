using UnityEngine;
using System.Collections;
using System.Data.Common;

public class EnemiesHealth : MonoBehaviour
{
    public int currentHealth;
    private bool isDying; // Controle para evitar múltiplas chamadas

    public AudioSource hitAudioSource;

    public AudioSource deathAudioSource;

    void Start(){
         
        isDying = false;
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDying)
        {
            if (deathAudioSource != null) deathAudioSource.Play();
            isDying = true; // Marca que o objeto está em processo de destruição
            Debug.Log("Iniciando cooldown para destruir " + gameObject.name);
            gameObject.GetComponent<Animator>().SetBool("isDead", true);
             
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (hitAudioSource != null && currentHealth != 0) hitAudioSource.Play();
    }
}
