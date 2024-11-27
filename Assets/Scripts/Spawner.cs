using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject [] EntitiesToSpawn; // Prefab do objeto (inimigos, etc.)
    public Tilemap tilemap;
    public Vector2[] spawnDirections; // Direções específicas de spawn (opcional)
    private float timer = 0.0f;
    private float timeBetweenSpawns = 60.0f * 10; // 10 minuto
    public int lambda = 3;

    private int numberOfEntitiesRemaining;

    //quando começa o jogo
    void Start()
    {
        SpawnEntitiesAtDistance();
    }

    //de x em x tempo chama
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenSpawns && numberOfEntitiesRemaining == 0)

        {
            Debug.Log("Spawnei mais arcas");
            timer = 0.0f;
            SpawnEntitiesAtDistance();
        }
    }


    void SpawnEntitiesAtDistance()
    {
        if (EntitiesToSpawn == null || tilemap == null)
        {
            Debug.LogError("Spawner: ObjectToSpawn or Tilemap not assigned.");
            return;
        }

        TerrainGenerator2D terrainGenerator = tilemap.GetComponent<TerrainGenerator2D>();

        int numberOfEntities = GeneratePoisson(lambda);

        Debug.Log("Number of Entities to spawn: " + numberOfEntities);

        for (int i = 0; i < numberOfEntities; i++)
        {
              
            numberOfEntitiesRemaining++;
        }
    }

    public void decreaseNumberOfEntities () {
        numberOfEntitiesRemaining--;
        Debug.Log("Decreasing number of Entities");
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