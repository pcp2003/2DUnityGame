using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject [] EntitiesToSpawn; // Prefab do objeto (inimigos, etc.)
    public Vector2[] spawnDirections; // Direções específicas de spawn (opcional)
    private TileMap tilemap; 
    private float timer = 0.0f;
    public float timeBetweenSpawns = 60.0f; // 10 minuto
    public int lambda = 3;
    private GameObject playerReference;
    private int horda;

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
            }
        }
        
    }


    void SpawnEntities()
    {
        
        int numberOfEntities = GeneratePoisson(lambda);

        Debug.Log("Number of Entities to spawn: " + numberOfEntities);

        for (int i = 0; i != numberOfEntities; i++){

            tilemap.SpawnEnemy(EntitiesToSpawn[0], playerReference);
        }

        horda++;
    }

    public void SetPlayerReference ( GameObject player){
        playerReference = player;
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