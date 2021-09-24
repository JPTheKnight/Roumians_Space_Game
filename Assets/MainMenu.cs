using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject[] levelButtons;
    public Slider musicSlider, volumeSlider;
    public AudioSource Music;

    int count = 0;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
        for (int i = 1; i < PlayerPrefs.GetInt("LevelsUnlocked", 1); i++)
        {
            levelButtons[i].GetComponent<Button>().interactable = true;
            levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        Music.volume = PlayerPrefs.GetFloat("Music", 1);
    }

    public void Next()
    {
        count++;
        if (count > levelButtons.Length - 1)
        {
            count = 0;
        }
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].SetActive(false);
        }
        levelButtons[count].SetActive(true);
    }

    public void Back()
    {
        count--;
        if (count < 0)
        {
            count = levelButtons.Length - 1;
        }
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].SetActive(false);
        }
        levelButtons[count].SetActive(true);
    }

    public void SendScene(int id)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(id);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
