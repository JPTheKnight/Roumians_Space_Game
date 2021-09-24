using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class level1 : MonoBehaviour
{
    public GameObject canvas;
    public GameObject resultText;
    public GameObject[] glowImages;
    public GameObject Fade;
    public GameObject WinPanel;
    bool[] objects = new bool[7] {true, false, false, true, false, true, false};

    public GameObject instructionsPanel;
    bool beginLevel = false;

    levelsManager lm;

    void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        Fade.GetComponent<Animator>().Play("fadeOutAnim");
    }

    int objectCounter = 0;
    int[] objectIds = new int[3] { -1, -1, -1 };
    bool won = false;
    float waitSecs = 0;
    private void Update()
    {
        if (lm.PausePanel.activeInHierarchy) return;

        if (waitSecs < 4f)
        {
            waitSecs += Time.deltaTime;
        }

        if (waitSecs > 3f && !beginLevel)
        {
            Fade.SetActive(false);
            instructionsPanel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                instructionsPanel.SetActive(false);
                beginLevel = true;
            }
        }
    }

    public void objectCheck(int id)
    {
        if (won || !beginLevel)
            return;

        for (int i = 0; i < 3; i++)
        {
            if (objectIds[i] == id)
                return;
        }

        glowImages[id].SetActive(true);
        objectIds[objectCounter] = id;
        objectCounter++;
        if (objectCounter == 3)
        {
            Destroy(GameObject.FindGameObjectWithTag("Finish"));

            for (int i = 0; i < 3; i++)
            {
                if (!objects[objectIds[i]])
                {
                    lm.pauseButton.SetActive(false);
                    StartCoroutine(lostPanel());
                    for (int j = 0; j < 7; j++)
                        glowImages[j].SetActive(false);
                    for (int j = 0; j < 3; j++)
                        objectIds[j] = -1;
                    objectCounter = 0;

                    return;

                }
               
            }

            won = true;
            WinPanel.SetActive(true);
            lm.pauseButton.SetActive(false);
            if (PlayerPrefs.GetInt("LevelsUnlocked") < 2)
                PlayerPrefs.SetInt("LevelsUnlocked", 2);
            //Show next level button    

        }
    }

    IEnumerator lostPanel()
    {
        resultText.SetActive(true);
        yield return new WaitForSeconds(4f);
        resultText.SetActive(false);
        lm.pauseButton.SetActive(true);
    }


    
}
