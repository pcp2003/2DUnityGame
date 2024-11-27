using System;
using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Spawner spawner;
    public float destroyCooldown;
    private String color;



    void Start(){
        color = gameObject.tag.Split(' ')[0];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Detected");
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {  
            Key key = player.GetKeyByColor(color);
            
            if (key != null ){
                
                player.UseKey(key);
                spawner.GetComponent<Spawner>().decreaseNumberOfEntities();
                StartCoroutine(DestroyCooldown());
            }
            
            
        }

    }

    IEnumerator DestroyCooldown()
    {
        yield return new WaitForSeconds(destroyCooldown); // Aguarda o tempo especificado
        Debug.Log("Destruindo objeto: " + gameObject.name);
        Destroy(gameObject); // Destrói o objeto após o tempo
    }

    public String GetColor (){
        return color;
    }

}
