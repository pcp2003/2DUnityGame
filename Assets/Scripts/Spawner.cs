using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject player; // Referência ao jogador n sei se é preciso mudar
    public GameObject objectToSpawn; // Prefab do objeto (inimigos, etc.)
    public float spawnDistance = 10.0f; // Distância do jogador onde os objetos serão criados
   // public int numberOfObjects = 3; // Número de objetos para spawnar
    public Vector2[] spawnDirections; // Direções específicas de spawn (opcional)
    private float timer = 0.0f;
    public float timeBetweenSpawns= 60.0f; // 1 minuto
    public int lambda = 3;
    
    //quando começa o jogo
    void Start()
    {
        SpawnObjectsAtDistance();
    }

    
    //de x em x tempo chama
    private void Update()
    {
        timer+=Time.deltaTime;
        if (timer >= timeBetweenSpawns)
        {
            Debug.Log("Spawnei mais inemigos");
            timer = 0.0f;
            SpawnObjectsAtDistance();
        }
    }
    
    
    void SpawnObjectsAtDistance()
    {
        if (player == null || objectToSpawn == null)
        {
            Debug.LogError("Spawner: Player or ObjectToSpawn not assigned.");
            return;
        }

        Vector2 playerPosition = player.transform.position;
        int numberOfObjects = GeneratePoisson(lambda);
        Debug.Log("Numero de objetos: " +numberOfObjects);
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Escolhe uma direção aleatória (ou específica, se fornecido)
            Vector2 spawnDirection = spawnDirections.Length > 0
                ? spawnDirections[i % spawnDirections.Length]
                : Random.insideUnitCircle.normalized;

            // Calcula a posição de spawn
            Vector2 spawnPosition = playerPosition + spawnDirection * spawnDistance;

            // Instancia o objeto no local calculado
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    
    public static int GeneratePoisson(double lambda)
    {
        // Verifica se lambda é válido
        if (lambda <= 0)
        {
            throw new ArgumentOutOfRangeException("Lambda deve ser maior que 0.");
        }

        // Método da soma cumulativa
        double l = Math.Exp(-lambda);
        double p = 1.0;
        int k = 0;

        do
        {
            k++;
            p *= UnityEngine.Random.value; // Usa um número aleatório entre 0 e 1
        } while (p > l);

        return k - 1;
    }
    
}