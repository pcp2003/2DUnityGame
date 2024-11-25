using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject player; // Referência ao jogador n sei se é preciso mudar
    public GameObject objectToSpawn; // Prefab do objeto (inimigos, etc.)
    public float spawnDistance = 10.0f; // Distância do jogador onde os objetos serão criados
    public TileBase[] grassTiles;   // tiles validas para spawnar     
    public Tilemap spawnTilemap;
    public Vector2[] spawnDirections; // Direções específicas de spawn (opcional)
    private float timer = 0.0f;
    public float timeBetweenSpawns= 60.0f*10; // 1 minuto
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
            Debug.Log("Spawnei mais arcas");
            timer = 0.0f;
            SpawnObjectsAtDistance();
        }
    }
    
    
    void SpawnObjectsAtDistance()
    {
        if (player == null || objectToSpawn == null || grassTiles == null || spawnTilemap == null )
        {
            Debug.LogError("Spawner: Player or ObjectToSpawn not assigned.");
            return;
        }

        Vector3 playerPosition = player.transform.position;
        int numberOfObjects = GeneratePoisson(lambda);
       
        Debug.Log("Numero de objetos: " +numberOfObjects);
       
        int spawnedObjects = 0;
       
        // enquanto n tiver encontardo uma possissão valida continua a tentar
        while (spawnedObjects < numberOfObjects)
        {
            Vector2 spawnDirection = spawnDirections.Length > 0
                ? spawnDirections[spawnedObjects % spawnDirections.Length]
                : Random.insideUnitCircle.normalized;

            Vector3 spawnPosition = playerPosition + (Vector3)spawnDirection * spawnDistance;


            Vector3Int cellPosition = spawnTilemap.WorldToCell(spawnPosition);
            Debug.unityLogger.Log(cellPosition.ToString());
            TileBase tile = spawnTilemap.GetTile(cellPosition);
            if (IsGrassTile(tile))
            {
                // Converte de volta para posição no mundo para instanciar
                Vector3 finalPosition = spawnTilemap.CellToWorld(cellPosition) + spawnTilemap.tileAnchor;

                // Instancia o objeto
                Instantiate(objectToSpawn, finalPosition, Quaternion.identity);
                Debug.Log($"Objeto spawnado em {finalPosition}");
                spawnedObjects++;
            }

            
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
    
    bool IsGrassTile(TileBase tile)
    {
        foreach (TileBase grassTile in grassTiles)
        {
            if (tile == grassTile)
                return true;
        }
        return false;
    }
    
    
    
}