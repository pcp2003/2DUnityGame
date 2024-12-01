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
    private float timeBetweenSpawns= 10.0f; // 1 minuto
    public int lambda = 3;
    
    //quando começa o jogo
    void Start()
    {
        SpawnObjectsAtDistance();
    }

    
    //de x em x tempo spwan mais arcas
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
        //Ex with poisson
        //int numberOfObjects = GeneratePoisson(lambda);
        
        //Ex with binomial
        //int numberOfObjects = GenerateRandomNumberOfCrates(1,0.50f);

       
       //ex with normal 
       double numberOfObjects = GenerateNormal(5, 2);   // media e desvio padrão
       
        
        Debug.LogError("Numero de objetos: " +numberOfObjects);
       
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

    
    
    
    
    
    
    
    
    
    
    
    // Poisson
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
    
    //Exponecial, determinar a frequencia de ataques de inimigos
    private int exponecial(float lambda)
    {
        // Generate U ~ [0,1]
        float U = UnityEngine.Random.value;
        // Generate X ~ Exp(lambda)
        float X = -Mathf.Log(1 - U) / lambda;

        Debug.Log("Com a exponecial gerei: " + Mathf.RoundToInt(X));
        return Mathf.RoundToInt(X);
    }
    
    
    // Binomial
    
    private int GenerateRandomNumberOfCrates(int n, float p)
    {
        // Generate a random variable U in the interval [0, 1]
        float U = UnityEngine.Random.value;

        // Initialize X and cumulative distribution function (CDF) to 0
        int X = 0;
        float cdf = 0;

        // Loop through all possible outcomes (from 0 to n)
        for (int k = 0; k <= n; k++)
        {
            // Calculate the probability mass function (PMF) for the current outcome
            float pmf = BinomialPMF(n, p, k);

            // Add the PMF to the CDF
            cdf += pmf;
            
            // If the random variable U is less than or equal to the CDF,
            // set X to the current outcome and break the loop
            if (U <= cdf)
            {
                X = k;
                break;
            }
        }

        // Return the generated random number of crates
        return X;
    }
    
    
    private float BinomialPMF(int n, float p, int k)
    {
        return Combination(n, k) * Mathf.Pow(p, k) * Mathf.Pow(1 - p, n - k);
    }

    private int Combination(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }


    private int Factorial(int n)
    {
        int result = 1;
        for (int i = 1; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
    
    
    // Normal pode ser preciso truncar mas matematicamante e chato
    public static double GenerateNormal(double mean, double stdDev)
    {
        float U1 = UnityEngine.Random.value; // UnityEngine.Random
        float U2 = UnityEngine.Random.value;
        double z0 = Math.Sqrt(-2.0 * Math.Log(U1)) * Math.Cos(2.0 * Math.PI * U2);
        return mean + z0 * stdDev;
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