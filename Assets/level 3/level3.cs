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

    public GameObject instructionsPanel;
    bool beginLevel = false;

    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

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
                return;

            Destroy(GameObject.FindGameObjectWithTag("Finish"));
            zRot = ((solarPanel.transform.rotation.eulerAngles.z + 540) % 360) - 180;

            if (zRot < -6.907 && zRot > -19.832)
            {
                GameObject result1 = Instantiate(resultText, canvas.transform);
                Destroy(result1, 4.5f);
                result1.GetComponent<TextMeshProUGUI>().text = "Level 3 accomplished!";
                result1.GetComponent<TextMeshProUGUI>().color = new Color32(52, 77, 255, 255);
                solarPanel.GetComponent<Animator>().enabled = false;
                won = true;
            }
            else
            {
                GameObject result = Instantiate(resultText, canvas.transform);
                Destroy(result, 4.5f);
                result.GetComponent<TextMeshProUGUI>().text = "FAIL!";
                result.GetComponent<TextMeshProUGUI>().color = new Color32(52, 77, 255, 255);
                solarPanel.GetComponent<Animator>().enabled = false;
                fail = true;
            }
        }
    }
}
