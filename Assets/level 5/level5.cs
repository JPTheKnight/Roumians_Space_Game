using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class level5 : MonoBehaviour
{
    public Animator fade;
    public TextMeshProUGUI timer;
    public float playTime = 1f;
    public GameObject sunCenterRotation;
    public TextMeshProUGUI sunTimer1, sunTimer2;
    public Sprite numberSelectedSprite;
    public Button[] NumbersTV;
    public GameObject redRoom1, redRoom2;
    public GameObject goToPharma;
    public GameObject timeLost, sunLost, codeLost;
    public GameObject WonPanel;

    [HideInInspector]
    public bool[] winningThings = { false, false, false, false };

    public GameObject instructionsPanel;
    [HideInInspector]
    public bool beginLevel = false;
    [HideInInspector]
    public bool sunShown1 = false, sunShown2 = false, hitBySun = false, firstSun = false;
    public float timeToRunFromSun = 10f;

    bool fullyWon = false;
    public bool lost { get; set; }
    float waitSecs = 0;
    float waitForLost = 0;
    float timeFromSun;

    int minutes = 4;
    float seconds = 0;

    //sun angles: -45.068 24.958 / 166.968 210.726

    bool redRoom1Anim = false, redRoom2Anim = false;

    characterMovementLevel5 cml5;

    private void Start()
    {
        fade.Play("fadeOutAnim");
        timeFromSun = timeToRunFromSun;
        cml5 = FindObjectOfType<characterMovementLevel5>();
    }

    bool fairouz5 = false;
    private void Update()
    {
        if (lost)
        {
            FindObjectOfType<characterMovementLevel5>().GetComponent<Animator>().SetBool("dying", true);
            fade.gameObject.SetActive(true);
            fade.Play("fadeInAnim");
            waitForLost += Time.deltaTime;
            if (waitForLost > 5f)
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

        if (beginLevel && !fullyWon && !cml5.SunMsg.activeInHierarchy)
        {
            if (seconds > 0f)
            {
                seconds -= Time.deltaTime;
            }
            if (seconds <= 0 && minutes > 0)
            {
                minutes -= 1;
                seconds = 59.5f;
            }

            timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            if (minutes == 0 && seconds < 0 && !lost)
            {
                lost = true;
                timeLost.SetActive(true);
                fairouzesPlay(6);
            }
        }

        if (((((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) < -150f || (((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) > 145.196f))
        {
            if (!redRoom2Anim)
            {
                StartCoroutine(redRoomAnim(redRoom2));
                redRoom2Anim = true;
            }
        }
        else if (((((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) < 24.958f && (((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) > -65.538f))
        {
            if (!redRoom1Anim)
            {
                StartCoroutine(redRoomAnim(redRoom1));
                redRoom1Anim = true;
            }
        }
        else
        {
            StopAllCoroutines();
            redRoom1.SetActive(false);
            redRoom2.SetActive(false);
            redRoom1Anim = false;
            redRoom2Anim = false;
        }

        if (((((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) < -150f || (((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) > 166.968f))
        {
            sunShown1 = true;
        }
        else if (((((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) < 24.958f && (((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) > -45.068f))
        {
            sunShown2 = true;
        }
        else
        {
            sunShown1 = false;
            sunShown2 = false;
        }

        if (hitBySun && !firstSun)
        {
            FindObjectOfType<characterMovementLevel5>().transform.GetChild(1).gameObject.SetActive(true);
            if (timeFromSun > 0)
                timeFromSun -= Time.deltaTime;
            sunTimer1.text = timeFromSun.ToString("00");
            sunTimer2.text = timeFromSun.ToString("00");
            if (!fairouz5)
            {
                goToPharma.SetActive(true);
                fairouzesPlay(4);
                fairouz5 = true;
            }
            Destroy(goToPharma, 3f);

            if (timeFromSun < 0 && !lost)
            {
                lost = true;
                sunLost.SetActive(true);
                fairouzesPlay(7);
                sunTimer1.text = "";
                sunTimer2.text = "";
            }
        }
        else if (hitBySun && firstSun && !lost)
        {
            lost = true;
            sunLost.SetActive(true);
            fairouzesPlay(7);
        }
        if (!hitBySun)
        {
            FindObjectOfType<characterMovementLevel5>().transform.GetChild(1).gameObject.SetActive(false);
            sunTimer1.text = "";
            sunTimer2.text = "";
        }

        if (numbersChosen == 3)
        {
            FindObjectOfType<characterMovementLevel5>().TVPanel.SetActive(false);
            if (numbers[0] && !numbers[1] && !numbers[2] && numbers[3] && numbers[4] && !numbers[5] && !numbers[6] && !numbers[7]
                 && !numbers[8])
            {
                winningThings[3] = true;
            }
            else
            {
                if (!lost)
                {
                    lost = true;
                    codeLost.SetActive(true);
                    fairouzesPlay(8);
                }
            }
        }

        if (winningThings[0] && winningThings[1] && winningThings[2] && winningThings[3])
        {
            fullyWon = true;
        }

        if (fullyWon)
        {
            WonPanel.SetActive(true);
            if (PlayerPrefs.GetInt("LevelsUnlocked") < 6)
                PlayerPrefs.SetInt("LevelsUnlocked", 6);
        }
    }

    public void resetSunTime()
    {
        timeFromSun = timeToRunFromSun;
    }

    bool[] numbers = { false, false, false, false, false, false, false, false, false };
    int numbersChosen = 0;
    public void selectNumberTV(int id)
    {
        numbers[id] = !numbers[id];
        NumbersTV[id].GetComponent<Image>().sprite = numbers[id] ? numberSelectedSprite : null;
        numbersChosen++;
    }

    IEnumerator redRoomAnim(GameObject redRoom)
    {
        while(true)
        {
            redRoom.SetActive(true);
            yield return new WaitForSeconds(1f);
            redRoom.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }

    public void fairouzesPlay(int id)
    {
        for (int i = 0; i < cml5.fairouzes.Length; i++)
        {
            cml5.fairouzes[i].Stop();
        }
        cml5.fairouzes[id].Play();
    }
}
