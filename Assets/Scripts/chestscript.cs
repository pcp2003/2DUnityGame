using UnityEngine;

public class chestscript : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null )
        {
            
            //todo decidir o que a arca vai dar ao jogdor
            Debug.Log("Chest entered");
            Destroy(gameObject);
            
        }



    }
}
