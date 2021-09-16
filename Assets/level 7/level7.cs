using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class level7 : MonoBehaviour
{
    public Animator fade;
    public float playTime = 1f;
    public Toggle[] seeToggles;
    public Sprite[] seeXray;

    public GameObject instructionsPanel;
    [HideInInspector]
    public bool beginLevel = false;
    public bool lost { get; set; }
    float waitSecs = 0;
    float waitForLost = 0;

    [HideInInspector]
    public bool[] winningThings = { false, false, false, false };
    bool fullyWon = false;

    void Update()
    {
        if (fullyWon)
        {
            Debug.Log("Won bitch");
        }

        if (lost)
        {
            FindObjectOfType<characterMovementLevel7>().GetComponent<Animator>().SetBool("dying", true);
            fade.gameObject.SetActive(true);
            fade.Play("fadeInAnim");
            waitForLost += Time.deltaTime;
            if (waitForLost > 4f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Test");
            }
        }

        if (waitSecs < 4f)
        {
            waitSecs += Time.deltaTime;
        }

        if (waitSecs > 3f && !beginLevel)
        {
            instructionsPanel.SetActive(true);
            fade.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                instructionsPanel.SetActive(false);
                beginLevel = true;
            }
        }

        if (winningThings[0] && winningThings[1] && winningThings[2] && winningThings[3])
        {
            fullyWon = true;
        }
    }

    public void seeOrgan()
    {
        FindObjectOfType<characterMovementLevel7>().xray_result.SetActive(true);

        for (int i = 0; i < seeToggles.Length; i++)
        {
            if (seeToggles[i].isOn)
            {
                FindObjectOfType<characterMovementLevel7>().xray_result.GetComponent<Image>().sprite = seeXray[i];
            }
        }
    }

    public void exitScan()
    {
        FindObjectOfType<characterMovementLevel7>().xray = false;
        FindObjectOfType<characterMovementLevel7>().scannedBones.SetActive(false);
        FindObjectOfType<characterMovementLevel7>().checkTV.SetActive(true);
        winningThings[0] = true;
    }
}
