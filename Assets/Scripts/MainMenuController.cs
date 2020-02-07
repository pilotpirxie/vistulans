using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public int IsMusicPlaying = 1;

    public TextMeshProUGUI MusicButtonText;

    public GameObject Tutorial;

    public GameObject SelectLevel;

    void Start()
    {
        if (PlayerPrefs.HasKey("IsMusicPlaying"))
        {
            IsMusicPlaying = PlayerPrefs.GetInt("IsMusicPlaying", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IsMusicPlaying", 1);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (MusicButtonText != null)
        {
            if (IsMusicPlaying == 0)
            {
                MusicButtonText.text = "Music: Off";
            }
            else
            {
                MusicButtonText.text = "Music: On";
            }
        }
    }

    /// <summary>
    /// Triggered on play button 
    /// </summary>
    public void OnPlay()
    {
        SelectLevel.SetActive(true);
    }

    /// <summary>
    /// Triggered on how to play button
    /// </summary>
    public void OnHowToPlay()
    {
        Tutorial.SetActive(true);
    }

    /// <summary>
    /// Hide tutorial information
    /// </summary>
    public void OnCloseTutorial()
    {
        Tutorial
            .SetActive(false);
    }

    /// <summary>
    /// Triggered on music button
    /// </summary>
    public void OnMusic()
    {
        if (IsMusicPlaying == 0)
        {
            PlayerPrefs.SetInt("IsMusicPlaying", 1);
            IsMusicPlaying = 1;
        }
        else
        {
            PlayerPrefs.SetInt("IsMusicPlaying", 0);
            IsMusicPlaying = 0;
        }

        PlayerPrefs.Save();
    }
}
