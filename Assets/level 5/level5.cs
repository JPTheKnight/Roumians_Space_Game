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
    public Sprite numberSelectedSprite, numberDeselectedSprite;
    public Button[] NumbersTV;
    public GameObject redRoom1, redRoom2;
    public GameObject goToPharma;
    public GameObject timeLost, sunLost, codeLost;
    public GameObject WonPanel;
    public AudioSource solarHit, room1WarningSound, room2WarningSound;

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

    levelsManager lm;

    //sun angles: -45.068 24.958 / 166.968 210.726

    bool redRoom1Anim = false, redRoom2Anim = false;

    characterMovementLevel5 cml5;

    private void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        fade.Play("fadeOutAnim");
        timeFromSun = timeToRunFromSun;
        cml5 = FindObjectOfType<characterMovementLevel5>();
    }

    bool fairouz5 = false;
    private void Update()
    {
        if (lm.PausePanel.activeInHierarchy) { solarHit.Pause(); room1WarningSound.Pause(); room2WarningSound.Pause(); return; }
        else { solarHit.UnPause(); room1WarningSound.UnPause(); room2WarningSound.UnPause(); }

        room2WarningSound.volume = ((1 - (Vector2.Distance(cml5.transform.position, room2WarningSound.transform.position) + 27f) / 44.151f) * 3f) *PlayerPrefs.GetFloat("Volume");
        room1WarningSound.volume = ((1 - (Vector2.Distance(cml5.transform.position, room1WarningSound.transform.position) + 12.75f) / 25.5211f) * 3f) *PlayerPrefs.GetFloat("Volume");

        if (lost)
        {
            lm.pauseButton.SetActive(false);
            cml5.GetComponent<Animator>().SetBool("dying", true);
            cml5.transform.GetChild(0).gameObject.SetActive(false);
            cml5.transform.GetChild(1).gameObject.SetActive(false);
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
                room2WarningSound.Play();
                cml5.TVPanel.SetActive(false);
                cml5.TV.GetComponent<Collider2D>().enabled = false;
                redRoom2Anim = true;
            }
        }
        else if (((((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) < 24.958f && (((sunCenterRotation.transform.rotation.eulerAngles.z + 540) % 360) - 180) > -65.538f))
        {
            if (!redRoom1Anim)
            {
                StartCoroutine(redRoomAnim(redRoom1));
                room1WarningSound.Play();
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
            room1WarningSound.Stop();
            room2WarningSound.Stop();

            if (!winningThings[3])
            {
                cml5.TV.GetComponent<Collider2D>().enabled = true;
            }
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
                solarHit.Play();
                goToPharma.SetActive(true);
                fairouzesPlay(4);
                fairouz5 = true;
            }
            Destroy(goToPharma, 3f);

            if (timeFromSun < 0 && !lost)
            {
                lost = true;
                solarHit.Stop();
                sunLost.SetActive(true);
                fairouzesPlay(7);
                sunTimer1.text = "";
                sunTimer2.text = "";
            }
        }
        else if (hitBySun && firstSun && !lost)
        {
            lost = true;
            solarHit.Stop();
            sunLost.SetActive(true);
            fairouzesPlay(7);
        }
        if (!hitBySun)
        {
            cml5.transform.GetChild(1).gameObject.SetActive(false);
            sunTimer1.text = "";
            sunTimer2.text = "";
        }

        if (numbersChosen == 3 && !winningThings[3])
        {
            cml5.TVPanel.SetActive(false);
            if (numbers[0] && !numbers[1] && !numbers[2] && numbers[3] && numbers[4] && !numbers[5] && !numbers[6] && !numbers[7]
                 && !numbers[8] && !winningThings[3])
            {
                winningThings[3] = true;
                cml5.smallWin.Play();
                cml5.TV.GetComponent<Collider2D>().enabled = false;
                cml5.fairouzes[3].Stop();
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
            lm.pauseButton.SetActive(false);
            if (PlayerPrefs.GetInt("LevelsUnlocked") < 6)
                PlayerPrefs.SetInt("LevelsUnlocked", 6);
        }
    }

    public void resetSunTime()
    {
        timeFromSun = timeToRunFromSun;
    }

    public bool[] numbers = { false, false, false, false, false, false, false, false, false };
    int numbersChosen = 0;
    public void selectNumberTV(int id)
    {
        numbers[id] = !numbers[id];
        NumbersTV[id].GetComponent<Image>().sprite = numbers[id] ? numberSelectedSprite : numberDeselectedSprite;
        numbersChosen = (numbers[id]) ? numbersChosen + 1 : numbersChosen - 1;
    }

    IEnumerator redRoomAnim(GameObject redRoom)
    {
        while(true)
        {
            redRoom.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            redRoom.SetActive(false);
            yield return new WaitForSeconds(0.5f);
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
