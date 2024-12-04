using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class Key : MonoBehaviour
{
    private String color;      
    private float timer = 0.0f; // Contador de tempo que passou
    private float lifetime;     // Tempo de vida da arca

    void Start()
    {
        // Definir o tempo de vida da arca quando ela for criada
        lifetime = Weibull(1);
        color = gameObject.name.Split(' ')[0];
    }

    private void Update() 
    {

         // Aumenta o timer com o tempo que passou desde o último frame
        timer += Time.deltaTime;

        // Se o timer ultrapassar o tempo de vida, destruir a arca
        if (timer >= lifetime)
        {
            Debug.Log("A chave foi destruída após o tempo de vida");
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && !player.GetIsInventoryFull())
        {   
            player.AddKey(this);
            gameObject.SetActive(false);
            
        }

    }

    public String GetColor (){
        return color;
    }

    public Sprite GetSprite () {
        return GetComponent<SpriteRenderer>().sprite;
    }


    // Função para gerar um número aleatório segundo a distribuição Weibull
    float Weibull( double l )
    {
        // Parâmetros da distribuição Weibull
        double lambda = l; // Parâmetro de escala em minutos  // no ppt igual ao b
        double k = 1.5;      // Parâmetro de forma variaçao  // igual ao C

        // Gerar um número aleatório uniforme [0, 1]
        float U = UnityEngine.Random.value;



        // Usando a fórmula da distribuição Weibull: X = λ * (-ln(1-U))^(1/k)
        double lifetimeMinutes = lambda * Math.Pow(-Math.Log(1 - U), 1 / k);
        double lifetimeSeconds = lifetimeMinutes * 60; // passar a segundos
        
        return (float)lifetimeSeconds;
    }

}
