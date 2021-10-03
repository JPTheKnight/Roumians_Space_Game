using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class level8 : MonoBehaviour
{
    public Animator fade, transitionFade;
    public GameObject Lost;
    public GameObject WonPanel;
    public GameObject spacePlayer, hospPlayer;
    public AudioSource smallWin;
    public TextMeshProUGUI pinTimerText;

    [HideInInspector]
    public bool[] winningThings = { false, false, false, false };

    public GameObject instructionsPanel;
    [HideInInspector]
    public bool beginLevel = false;
    [HideInInspector]
    public bool sunShown1 = false, sunShown2 = false, hitBySun = false, firstSun = false;
    [HideInInspector]
    public bool shipActivated = false;
    [HideInInspector]
    public bool pinActivated = false;
    bool pinInHand = false;
    GameObject pinHit;
    int pinCount = 0;
    float pinTimer = 20f;

    bool fullyWon = false;
    public bool lost { get; set; }
    float waitSecs = 0;
    float waitForLost = 0;

    bool[] rightIDS = { false, false, false, false, false, false, false, false, false, false };

    int minutes = 4;
    float seconds = 0;

    levelsManager lm;

    characterMovementLevel8 cml8;

    private void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        fade.Play("fadeOutAnim");
        cml8 = FindObjectOfType<characterMovementLevel8>();
    }

    [HideInInspector]
    public float TimerWait = 1f;
    private void Update()
    {
        if (lost)
        {
            lm.pauseButton.SetActive(false);
            Lost.SetActive(true);
            cml8.GetComponent<Animator>().SetBool("dying", true);
            cml8.transform.GetChild(0).gameObject.SetActive(false);
            cml8.transform.GetChild(1).gameObject.SetActive(false);
            fade.gameObject.SetActive(true);
            fade.Play("fadeInAnim");
            waitForLost += Time.deltaTime;
            if (waitForLost > 3f)
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

        if (shipActivated && TimerWait > 0)
        {
            TimerWait -= Time.deltaTime;
        }

        if (pinActivated && TimerWait > 0)
        {
            TimerWait -= Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) && !lost && shipActivated && beginLevel)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit.collider != null && hit.collider.tag == "AgencyID")
            {
                hit.collider.transform.GetChild(0).gameObject.SetActive(!hit.collider.transform.GetChild(0).gameObject.activeInHierarchy);
                int number = int.Parse(hit.collider.name[hit.collider.name.Length - 1].ToString());

                rightIDS[number - 1] = !rightIDS[number - 1];
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && beginLevel && shipActivated && TimerWait < 0)
        {
            if (!rightIDS[0] && rightIDS[1] && !rightIDS[2] && !rightIDS[3] && !rightIDS[4] && rightIDS[5] && rightIDS[6] && !rightIDS[7] && !rightIDS[8] && !rightIDS[9])
            {
                winningThings[0] = true;
                cml8.agencyFuncsPuzzle.SetActive(false);
                cml8.agencyPuzzle.SetActive(false);
                shipActivated = false;
                smallWin.Play();
                StartCoroutine(Transition());
            }
            else
            {
                if (!lost)
                {
                    lost = true;
                }
            }
        }

        if (pinInHand)
        {
            pinHit.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

                if (hit.collider != null && hit.collider.tag == "Slot")
                {
                    if (hit.transform.name == pinHit.transform.name)
                    {
                        pinHit.transform.parent = hit.transform;
                        pinHit.transform.localPosition = Vector2.zero;
                        hit.transform.name = hit.transform.name + "DONE";
                        pinHit = null;
                        pinInHand = false;
                        pinCount++;
                    }
                    else
                    {
                        lost = true;
                    }
                }
                else
                {
                    lost = true;
                }
            }

            if (pinCount == 16)
            {
                winningThings[3] = true;
                cml8.PinPuzzle.SetActive(false);
                pinInHand = false;
                pinActivated = false;
                smallWin.Play();
                pinTimerText.gameObject.SetActive(false);
            }
        }

        if (pinActivated)
        {
            pinTimerText.text = "00:" + pinTimer.ToString("00");
            pinTimer -= Time.deltaTime;

            if (pinTimer < 0)
            {
                lost = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && !lost && pinActivated && beginLevel)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit.collider != null && hit.collider.tag == "Pin")
            {
                pinInHand = true;
                pinHit = hit.collider.gameObject;
                hit.collider.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && beginLevel && pinActivated && TimerWait < 0)
        {
            if (!rightIDS[0] && rightIDS[1] && !rightIDS[2] && !rightIDS[3] && !rightIDS[4] && rightIDS[5] && rightIDS[6] && !rightIDS[7] && !rightIDS[8] && !rightIDS[9])
            {
                winningThings[0] = true;
                cml8.agencyFuncsPuzzle.SetActive(false);
                cml8.agencyPuzzle.SetActive(false);
                shipActivated = false;
                smallWin.Play();
                StartCoroutine(Transition());
            }
            else
            {
                if (!lost)
                {
                    lost = true;
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
        }
    }

    IEnumerator Transition()
    {
        transitionFade.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        transitionFade.Play("fadeOutAnim");
        Destroy(transitionFade.gameObject, 2f);
        Camera.main.transform.position = new Vector3(16.6f, -0.6f, -10f);
        cml8.minCamX = 16.6f;
        cml8.maxCamX = 58.15f;
        Destroy(spacePlayer);
        hospPlayer.SetActive(true);
        hospPlayer.GetComponent<characterMovementLevel8>().enabled = true;
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().enabled = false;
    }
}
