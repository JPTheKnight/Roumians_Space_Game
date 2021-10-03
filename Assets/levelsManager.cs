using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelsManager : MonoBehaviour
{
    public GameObject PausePanel;
    public Slider musicSlider;
    public Slider volumeSlider;
    public GameObject pauseButton;
    public Toggle fullScreen;

    bool notChange = false;

    AudioSource Music;
    float music;
    public GameObject[] audioSources;
    float[] volumes;
    float pauseTimer = 5f;

    private void Start()
    {
        fullScreen.isOn = Screen.fullScreen ? true : false;
        notChange = true;
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
        volumes = new float[audioSources.Length];
        Music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
            volumes[i] = audioSources[i].GetComponent<AudioSource>().volume;
        music = Music.volume;
        Time.timeScale = 1;
        setVolumes();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        PausePanel.SetActive(true);      
        Time.timeScale = 0;
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void FullWindowedScreen()
    {
        if (notChange)
        {
            notChange = false;
            return;
        }
        Screen.fullScreen = !Screen.fullScreen;
    }

    private void Update()
    {
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pauseTimer < 0)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }

        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);

        if (PausePanel.activeInHierarchy)
        {
            setVolumes();
        }
    }

    void setVolumes()
    {
        for (int i = 0; i < audioSources.Length; i++)
            audioSources[i].GetComponent<AudioSource>().volume = volumes[i] * PlayerPrefs.GetFloat("Volume", 1);
        Music.volume = music * PlayerPrefs.GetFloat("Music", 1);
    }
}
