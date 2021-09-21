using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class characterMovement : MonoBehaviour
{
    public float speed;
    public float speedAscending;
    public GameObject instructionsPanel, instructionsPanel2;

    public GameObject[] plantsMaafnin;
    public Animator[] plants;
    public GameObject[] plantsGlow;

    public GameObject inputCodeSquare;
    public GameObject inputCodePanel;
    public GameObject littleInfoWon;
    public TextMeshProUGUI[] codeTexts;
    public AudioSource[] fairouzes;
    public Image micro;
    public Sprite microOff, microOn;
    public GameObject WonPanel;
    int[] codeNumbers = { 0, 0, 0 };

    bool[] plantsChosen = { false, false, false, false };
    bool[] plantsDone = { false, false, false, false };
    bool grounded = false;

    bool insertCode = false;
    bool fixedForCode = false;

    level4 lvl4;
    void Start()
    {
        lvl4 = FindObjectOfType<level4>();
    }

    int codeID = 0;

    void Update()
    {

        if (!lvl4.lost && lvl4.beginLevel && !fixedForCode)
        {
            if (transform.position.y > Camera.main.transform.position.y && Camera.main.transform.position.y < 0.84f)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, -10f);
            }

            if (transform.position.y < Camera.main.transform.position.y && Camera.main.transform.position.y > -0.6f)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, -10f);
            }

            if (transform.position.x < Camera.main.transform.position.x && Camera.main.transform.position.x > -17.79f)
            {
                Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, -10f);
            }

            if (transform.position.x > Camera.main.transform.position.x && Camera.main.transform.position.x < 18.76f)
            {
                Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, -10f);
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.rotation = new Quaternion(0, 0, 0, 1);
                GetComponent<Animator>().SetBool("running", true);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                GetComponent<Animator>().SetBool("running", false);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                transform.rotation = new Quaternion(0, 180f, 0, 1);
                GetComponent<Animator>().SetBool("running", true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                GetComponent<Animator>().SetBool("running", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded && transform.position.x < -9.50565f)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speedAscending);
                GetComponent<Animator>().SetBool("jumping", false);
            }
        }

        if (plantsChosen[0] || plantsChosen[1] || plantsChosen[2] || plantsChosen[3])
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < plantsGlow.Length; i++)
                {
                    plantsGlow[i].SetActive(false);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (plantsChosen[i])
                    {
                        if (i != 0 && !plantsDone[i - 1])
                        {
                            plantsMaafnin[i].SetActive(true);
                            plants[i].gameObject.SetActive(false);
                            if (!lvl4.lost)
                            {
                                lvl4.lost = true;
                                lvl4.fairouzesPlay(4);
                            }
                            return;
                        }

                        Debug.Log("Won");
                        plants[i].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        plantsDone[i] = true;
                        plants[i].enabled = true;
                        plants[i].GetComponent<BoxCollider2D>().enabled = false;
                        if (plants[i].transform.position.x > transform.position.x)
                        {
                            plants[i].transform.GetChild(1).gameObject.SetActive(true);
                            Destroy(plants[i].transform.GetChild(1).gameObject, 1f);
                        }
                        else
                        {
                            plants[i].transform.GetChild(2).gameObject.SetActive(true);
                            Destroy(plants[i].transform.GetChild(2).gameObject, 1f);
                        }
                    }
                }
            }
        }

        if (plantsDone[3])
        {
            lvl4.numsWinsCanvas[0].SetActive(true);
            testNumbers();
        }

        if (insertCode && Input.GetKeyDown(KeyCode.Return))
        {
            inputCodePanel.SetActive(true);
            fixedForCode = true;
        }

        if (fixedForCode && codeID < 3)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) { codeTexts[codeID].text = "0"; codeNumbers[codeID] = 0; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) { codeTexts[codeID].text = "1"; codeNumbers[codeID] = 1; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) { codeTexts[codeID].text = "2"; codeNumbers[codeID] = 2; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) { codeTexts[codeID].text = "3"; codeNumbers[codeID] = 3; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) { codeTexts[codeID].text = "4"; codeNumbers[codeID] = 4; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) { codeTexts[codeID].text = "5"; codeNumbers[codeID] = 5; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) { codeTexts[codeID].text = "6"; codeNumbers[codeID] = 6; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) { codeTexts[codeID].text = "7"; codeNumbers[codeID] = 7; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) { codeTexts[codeID].text = "8"; codeNumbers[codeID] = 8; codeID++; }
            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) { codeTexts[codeID].text = "9"; codeNumbers[codeID] = 9; codeID++; }

        }

        if (codeID == 3)
        {
            if (codeNumbers[0] == 9 && codeNumbers[1] == 6 && codeNumbers[2] == 1)
            {
                inputCodeSquare.GetComponent<SpriteRenderer>().color = Color.green;
                inputCodePanel.SetActive(false);
                littleInfoWon.SetActive(true);
            }
            else
            {
                lvl4.lost = true;
                lvl4.lostPanel.GetComponent<Image>().sprite = lvl4.lostMessage[1];
            }
        }

        if (littleInfoWon.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            littleInfoWon.SetActive(false);
            WonPanel.SetActive(true);
            if (PlayerPrefs.GetInt("LevelsUnlocked") < 5)
                PlayerPrefs.SetInt("LevelsUnlocked", 5);
        }

        if (fairouzesPlaying())
        {
            micro.sprite = microOn;
        }
        else
        {
            micro.sprite = microOff;
        }
    }

    public bool fairouzesPlaying()
    {
        for (int i = 0; i < fairouzes.Length; i++)
        {
            if (fairouzes[i].isPlaying)
                return true;
        }

        return false;
    }

    public void testNumbers()
    {
        for (int i = 0; i < Camera.main.GetComponent<level4>().numsWinsCanvas.Length; i++)
        {
            if (!Camera.main.GetComponent<level4>().numsWinsCanvas[i].activeInHierarchy)
            {
                return;
            }
        }

        inputCodeSquare.SetActive(true);
    }

    public void resetMics(int id)
    {
        fairouzes[id].Play();
    }

    bool fairouz1 = false, fairouz2 = false, fairouz3 = false;
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
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(true);

            if (!fairouz1)
            {
                lvl4.fairouzesPlay(0);
                fairouz1 = true;
            }
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(true);

            if (!fairouz3)
            {
                lvl4.fairouzesPlay(2);
                fairouz3 = true;
            }
        }

        if (collision.gameObject.name == "rosePlant")
        {
            plantsChosen[1] = true;
            plantsGlow[1].SetActive(true);
        }

        if (collision.gameObject.name == "redPlant")
        {
            plantsChosen[0] = true;
            plantsGlow[0].SetActive(true);
        }

        if (collision.gameObject.name == "yellowPlant")
        {
            plantsChosen[2] = true;
            plantsGlow[2].SetActive(true);
        }

        if (collision.gameObject.name == "bluePlant")
        {
            plantsChosen[3] = true;
            plantsGlow[3].SetActive(true);
        }

        if (collision.gameObject.tag == "InsertCode")
        {
            insertCode = true;
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

            if (!fairouz2 && transform.position.x > -6.84f)
            {
                lvl4.fairouzesPlay(1);
                fairouz2 = true;
            }
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(false);
            fairouzes[0].Stop();
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(false);
            fairouzes[2].Stop();
        }

        if (collision.gameObject.name == "rosePlant")
        {
            plantsChosen[1] = false;
            plantsGlow[1].SetActive(false);
        }

        if (collision.gameObject.name == "redPlant")
        {
            plantsChosen[0] = false;
            plantsGlow[0].SetActive(false);
        }

        if (collision.gameObject.name == "yellowPlant")
        {
            plantsChosen[2] = false;
            plantsGlow[2].SetActive(false);
        }

        if (collision.gameObject.name == "bluePlant")
        {
            plantsGlow[3].SetActive(false);
            plantsChosen[3] = false;
        }

        if (collision.gameObject.tag == "InsertCode")
        {
            insertCode = false;
        }
    }
}
