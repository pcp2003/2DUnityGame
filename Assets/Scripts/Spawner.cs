using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject [] EntitiesToSpawn; // Prefab do objeto (inimigos, etc.)
    private TileMap tilemap; 
    private float timer = 0.0f;
    public float timeBetweenSpawns = 60.0f; // 10 minuto
    public int poissonLambda = 3;
    public int exponencialLambda = 1;
    private GameObject playerReference;
    private int horda;

    public int avarageSpawnDistanceFromPlayer;

    void Start(){

        tilemap = gameObject.GetComponent<TileMap>();

        if (EntitiesToSpawn == null){
            Debug.LogError("Spawner: EntitiesToSpawn not assigned.");
            return;
        }

        horda = 0;

    }

    private void Update()
    {


        if (playerReference != null ){

             // Escreve as coordenadas do playerReference no console
            Vector3 playerPosition = playerReference.transform.position;
            Debug.Log($"Player Position: ({playerPosition.x}, {playerPosition.y}, {playerPosition.z})");


            timer += Time.deltaTime;

            Debug.Log(timer);

            if (timer >= timeBetweenSpawns)

            {
                Debug.Log("Spawnando mais inimigos!");
                timer = 0.0f;
                SpawnEntities();
                Debug.Log($"Horda: {horda}");
            }
        }
        
    }


    void SpawnEntities()
    {
        
        int numberOfEntities = GeneratePoisson(poissonLambda);

        Debug.Log("Number of Entities to spawn: " + numberOfEntities);

        for (int i = 0; i != numberOfEntities; i++){

            int exp = Exponecial(exponencialLambda);

            tilemap.SpawnEnemy(EntitiesToSpawn[UnityEngine.Random.Range(0, EntitiesToSpawn.Length)], playerReference, avarageSpawnDistanceFromPlayer - exp);
        }

        horda++;
    }

    public void SetPlayerReference ( GameObject player){
        playerReference = player;
    }   

    public static int GeneratePoisson(double poissonLambda)
    {
        // Verifica se lambda é válido
        if (poissonLambda <= 0)
        {
            throw new ArgumentOutOfRangeException("Lambda deve ser maior que 0.");
        }

        // Método da soma cumulativa
        double l = Math.Exp(-poissonLambda);
        double p = 1.0;
        int k = 0;

        do
        {
            k++;
            p *= UnityEngine.Random.value; // Usa um número aleatório entre 0 e 1
        } while (p > l);

        return k - 1;
    }

    private int Exponecial(float exponencialLambda)
    {
        // Generate U ~ [0,1]
        float U = UnityEngine.Random.value;
        // Generate X ~ Exp(lambda)
        float X = -Mathf.Log(1 - U) / exponencialLambda;

        Debug.Log("Com a exponecial gerei: " + Mathf.RoundToInt(X));
        return Mathf.RoundToInt(X);
    }


}