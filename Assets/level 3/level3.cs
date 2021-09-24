using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class level3 : MonoBehaviour
{
    public GameObject solarPanel; //49.157 -36.347    -6.907 -19.832
    public GameObject canvas;
    public GameObject resultText;
    public GameObject Fade;
    public GameObject dotted_line;
    public GameObject WonPanel;

    public GameObject instructionsPanel;
    bool beginLevel = false;

    levelsManager lm;

    void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        Fade.GetComponent<Animator>().Play("fadeOutAnim");
    }

    float zRot;
    bool won = false;
    float waitSecs = 0;
    float waitForFail = 0;
    bool fail = false;

    float maxwait = 3f;

    private void Update()
    {
        if (lm.PausePanel.activeInHierarchy) return;

        if (waitSecs < maxwait)
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
                maxwait = 3.5f;
            }
        }

        if (fail)
        {
            waitForFail += Time.deltaTime;
            if (waitForFail > 4f)
            {
                resultText.SetActive(false);
                waitForFail = 0;
                fail = false;
                solarPanel.GetComponent<Animator>().enabled = true;
            }
        }

        zRot = ((solarPanel.transform.rotation.eulerAngles.z + 540) % 360) - 180;

        if (zRot < -6.907 && zRot > -19.832)
        {
            dotted_line.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            dotted_line.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.Space) && waitSecs > 3.5f)
        {
            if (won || fail)
            {
                lm.pauseButton.SetActive(false);
                return;
            }

            Destroy(GameObject.FindGameObjectWithTag("Finish"));
            zRot = ((solarPanel.transform.rotation.eulerAngles.z + 540) % 360) - 180;

            if (zRot < -6.907 && zRot > -19.832)
            {
                WonPanel.SetActive(true);
                if (PlayerPrefs.GetInt("LevelsUnlocked") < 4)
                    PlayerPrefs.SetInt("LevelsUnlocked", 4);
                solarPanel.GetComponent<Animator>().enabled = false;
                won = true;
            }
            else
            {
                resultText.SetActive(true);
                solarPanel.GetComponent<Animator>().enabled = false;
                fail = true;
            }
        }
    }
}
