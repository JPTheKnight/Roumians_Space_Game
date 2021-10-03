using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterMovementLevel8 : MonoBehaviour
{
    public float speed;
    public float speedAscending;
    public GameObject instructionsPanel, instructionsPanel2;
    public GameObject eyePanel, circle;
    public GameObject[] Letters;
    public GameObject agencyPuzzle, agencyFuncsPuzzle;
    public GameObject PinPuzzle;
    public AudioSource walkingSound, doorOpen, doorClose, smallWin;
    public GameObject balanceText;

    bool fixCamera = false;
    bool grounded = false;
    bool tawezon = false, tawezonActivated = false;
    bool eye = false, eyeActivated = false;
    bool ship = false;
    bool pin = false;

    bool isWalking = false;

    float tawezonTimer = 20f;
    [HideInInspector]
    public float minCamX = -17.79f, maxCamX = -15.6f;

    level8 lvl8;
    levelsManager lm;
    void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        lvl8 = FindObjectOfType<level8>();
    }

    int[] lettersIndex = { 4, 1, 7, 5, 2 };
    char[] letters = { 'E', 'A', 'R', 'T', 'H' };
    char[] weirdChars = { 'β', 'ζ', 'η', 'ι', 'λ', 'μ', 'ξ', 'φ' };
    int curLetterID = 0;
    float TimerWait = 1f;
    void Update()
    {
        if (lm.PausePanel.activeInHierarchy) { walkingSound.Pause(); doorOpen.Pause(); doorClose.Pause(); smallWin.Pause(); return; }
        else { walkingSound.UnPause(); doorOpen.UnPause(); doorClose.UnPause(); smallWin.UnPause(); }

        transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "00:" + tawezonTimer.ToString("00");

        if (!lvl8.lost && lvl8.beginLevel && !tawezonActivated && !eyeActivated && !lvl8.shipActivated && !lvl8.pinActivated)
        {
            if (!fixCamera)
            {
                if (transform.position.y > Camera.main.transform.position.y && Camera.main.transform.position.y < 0.84f)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, -10f);
                }

                if (transform.position.y < Camera.main.transform.position.y && Camera.main.transform.position.y > -0.6f)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, -10f);
                }

                if (transform.position.x < Camera.main.transform.position.x && Camera.main.transform.position.x > minCamX)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, -10f);
                }

                if (transform.position.x > Camera.main.transform.position.x && Camera.main.transform.position.x < maxCamX)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, -10f);
                }
            }

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && grounded)
            {
                if (!isWalking)
                {
                    StartCoroutine(WalkingSound());
                    isWalking = true;
                }
            }
            if (!grounded || !GetComponent<Animator>().GetBool("running"))
            {
                walkingSound.Stop();
                isWalking = false;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.rotation = new Quaternion(0, 0, 0, 1);
                transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 1);
                GetComponent<Animator>().SetBool("running", true);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                GetComponent<Animator>().SetBool("running", false);
                walkingSound.Stop();
                isWalking = false;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.rotation = new Quaternion(0, 180f, 0, 1);
                transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 1);
                GetComponent<Animator>().SetBool("running", true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                GetComponent<Animator>().SetBool("running", false);
                walkingSound.Stop();
                isWalking = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speedAscending);
                GetComponent<Animator>().SetBool("jumping", false);
            }
        }

        if (tawezon && Input.GetKeyDown(KeyCode.Return))
        {
            tawezonActivated = true;
            transform.GetChild(0).gameObject.SetActive(true);
            tawezonCollisionSaved.transform.GetChild(0).gameObject.SetActive(false);
            tawezon = false;
            balanceText.SetActive(true);
            GetComponent<Animator>().SetBool("rehab", true);
        }

        if (tawezonActivated)
        {
            tawezonTimer -= Time.deltaTime;

            if (tawezonTimer < 0)
            {
                tawezonActivated = false;
                tawezonCollisionSaved.enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                lvl8.winningThings[1] = true;
                smallWin.Play();
                balanceText.SetActive(false);
                GetComponent<Animator>().SetBool("rehab", false);
            }
        }

        if (eye && Input.GetKeyDown(KeyCode.Return))
        {
            eyePanel.SetActive(true);
            eye = false;
            eyeActivated = true;
        }

        if (eyeActivated)
        {
            if (TimerWait > 0)
            {
                TimerWait -= Time.deltaTime;
            }

            nearestLetter();

            if (curLetterID == lettersIndex.Length)
            {
                lvl8.winningThings[2] = true;
                eyePanel.SetActive(false);
                eyeActivated = false;
                eyeCollisionSaved.enabled = false;
                smallWin.Play();
            }

            if (Input.GetKeyDown(KeyCode.Return) && TimerWait < 0)
            {
                if (Vector2.Distance(circle.transform.position, Letters[lettersIndex[curLetterID]].transform.position) < 40f)
                {
                    curLetterID++;
                    for (int i = 0; i < Letters.Length; i++)
                    {
                        Letters[i].GetComponent<TextMeshProUGUI>().text = weirdChars[i].ToString();
                    }
                    Letters[lettersIndex[curLetterID]].GetComponent<TextMeshProUGUI>().text = letters[curLetterID].ToString();
                }
                else
                {
                    eyeActivated = false;
                    eyePanel.SetActive(false);
                    lvl8.lost = true;
                }
            }
        }

        if (ship && Input.GetKeyDown(KeyCode.Return))
        {
            agencyPuzzle.SetActive(true);
            agencyFuncsPuzzle.SetActive(true);
            shipCollisionSaved.enabled = false;
            ship = false;
            lvl8.shipActivated = true;
            lvl8.TimerWait = 1f;
        }

        if (pin && Input.GetKeyDown(KeyCode.Return))
        {
            lvl8.pinActivated = true;
            pin = false;
            PinPuzzle.SetActive(true);
            lvl8.TimerWait = 1f;
            lvl8.pinTimerText.gameObject.SetActive(true);
        }
    }

    int id = 0;
    void nearestLetter()
    {
        Debug.Log("GI");

        for (int i = 0; i < Letters.Length; i++)
        {
            if (Vector2.Distance(circle.transform.position, Letters[i].transform.position) < Vector2.Distance(circle.transform.position, Letters[id].transform.position))
            {
                id = i;
            }
        }

        for (int i = 0; i < Letters.Length; i++)
        {
            Letters[i].SetActive(false);
        }

        Letters[id].SetActive(true);
    }

    Collider2D tawezonCollisionSaved, eyeCollisionSaved, shipCollisionSaved, pinCollisionSaved;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && grounded == false)
        {
            grounded = true;
            GetComponent<Animator>().SetBool("jumping", false);
        }

        if (collision.gameObject.tag == "Door")
        {
            collision.gameObject.transform.parent.GetChild(0).GetComponent<Animator>().SetBool("on", true);
            doorClose.Stop();
            doorOpen.Play();
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(true);

        }

        if (collision.gameObject.tag == "ship")
        {
            ship = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            shipCollisionSaved = collision;
        }

        if (collision.gameObject.tag == "Tawezon")
        {
            tawezon = true;
            tawezonCollisionSaved = collision;
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Eye")
        {
            eye = true;
            eyeCollisionSaved = collision;
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "PinBoard")
        {
            pin = true;
            pinCollisionSaved = collision;
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && grounded == true)
        {
            grounded = false;
            GetComponent<Animator>().SetBool("jumping", true);
        }

        if (collision.gameObject.tag == "Door")
        {
            collision.gameObject.transform.parent.GetChild(0).GetComponent<Animator>().SetBool("on", false);
            doorOpen.Stop();
            doorClose.Play();
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(false);
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(false);
        }

        if (collision.gameObject.tag == "ship")
        {
            ship = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Tawezon")
        {
            tawezon = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Eye")
        {
            eye = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "PinBoard")
        {
            pin = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    IEnumerator WalkingSound()
    {
        walkingSound.Stop();
        yield return new WaitForSeconds(0.2f);
        walkingSound.Play();
    }
}
