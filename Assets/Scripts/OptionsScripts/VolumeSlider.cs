using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    public void OnValueChange(float value) {
        PlayerController.volume = value;
        PlayerHealth.volume = value;
        EnemiesHealth.volume = value;
        BackgroundMusicManager.volume = value;
        Debug.Log(value);
    }

}
