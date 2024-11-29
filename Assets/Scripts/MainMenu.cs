using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public float volume = 1.0f;

    public void ClickPlay() {
        SceneManager.LoadSceneAsync("PlayingScene");
    }
}