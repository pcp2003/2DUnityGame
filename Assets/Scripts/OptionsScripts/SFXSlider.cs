using UnityEngine;

public class SFXSlider : MonoBehaviour
{
    public void OnValueChange(float value) {
        PlayerController.volume = value;
        PlayerHealth.volume = value;
        EnemiesHealth.volume = value;
        Debug.Log(value);
    }
}
