using UnityEngine;

public class MusicSlider : MonoBehaviour
{
    public void OnValueChange(float value) {
        BackgroundMusicManager.volume = value;
        MainMenuMusic.volume = value;
    }

}
