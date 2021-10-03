using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URL : MonoBehaviour
{
    public void OpenURL(string url)
    {
        FindObjectOfType<levelsManager>().Pause();
        Screen.fullScreen = false;
        FindObjectOfType<levelsManager>().fullScreen.isOn = false;
        Application.OpenURL(url);
    }
}
