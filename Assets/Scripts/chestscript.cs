using System;
using UnityEngine;
using Random = System.Random;

// Exemplo de como usar a weibull nas arcas pode ser utilizada em inimigos que n e possivel matar tmb
// inimigos morrem passado um tempo


public class ChestScript : MonoBehaviour
{
    private float timer = 0.0f; // Contador de tempo que passou
    private float lifetime;     // Tempo de vida da arca

    private void Start()
    {
        // Definir o tempo de vida da arca quando ela for criada
        lifetime = Weibull();
        Debug.Log($"A arca ficará viva por {lifetime:F2} segundos.");
    }

    private void Update()
    {
        // Aumenta o timer com o tempo que passou desde o último frame
        timer += Time.deltaTime;

        // Se o timer ultrapassar o tempo de vida, destruir a arca
        if (timer >= lifetime)
        {
            Debug.Log("A arca foi destruída após o tempo de vida");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o jogador colidiu com a arca
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            // Adicionar o que a arca dá ao jogador aqui
            Debug.Log("O jogador pegou a arca");
            Destroy(gameObject);
        }
    }

    
    // falta ver melhor depois que valores meter para o ppts
    // Função para gerar um número aleatório segundo a distribuição Weibull
    float Weibull()
    {
        // Parâmetros da distribuição Weibull
        double lambda = 1; // Parâmetro de escala em minutos  // no ppt igual ao b
        double k = 1.5;      // Parâmetro de forma variaçao  // igual ao C

        // Gerar um número aleatório uniforme [0, 1]
        float U = UnityEngine.Random.value;



        // Usando a fórmula da distribuição Weibull: X = λ * (-ln(1-U))^(1/k)
        double lifetimeMinutes = lambda * Math.Pow(-Math.Log(1 - U), 1 / k);
        double lifetimeSeconds = lifetimeMinutes * 60; // passar a segundos
        
        return (float)lifetimeSeconds;
    }
}