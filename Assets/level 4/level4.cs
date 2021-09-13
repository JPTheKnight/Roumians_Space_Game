using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level4 : MonoBehaviour
{
    public GameObject[] lines;
    public GameObject[] wrongLines;
    public Animator fade;
    public GameObject[] numsWinsCanvas;
    public GameObject lostPanel;
    public Sprite[] lostMessage;

    public GameObject instructionsPanel;
    [HideInInspector]
    public bool beginLevel = false;

    bool firstChosen = false;
    Collider2D firstCollider;
    string firstName = "";

    bool[] wireWins = { false, false, false, false, false, false, false, false };

    bool[] A = { false, false, false, false };
    bool[] B = { false, false, false, false };
    bool[] C = { false, false, false, false };
    bool[] D = { false, false, false, false };

    float waitSecs = 0;   
    public bool lost { get; set; }
    float waitForLost = 0;

    private void Start()
    {
        fade.Play("fadeOutAnim");
    }

    void Update()
    {
        if (lost)
        {
            lostPanel.SetActive(true);
            FindObjectOfType<characterMovement>().GetComponent<Animator>().SetBool("dying", true);
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

        if (Input.GetMouseButtonUp(0) && !lost && (transform.position.x > -7.40565f && transform.position.x < 7.546844f) && beginLevel)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit.collider != null)
            {
                if (firstChosen)
                {
                    if (firstName == hit.collider.name)
                    {
                        hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                        firstChosen = false;
                        return;
                    }

                    Debug.Log(firstName.Remove(firstName.Length - 1));

                    switch(firstName.Remove(firstName.Length - 1))
                    {
                        case "red": 
                            if (hit.collider.name.StartsWith("red") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[0].SetActive(true);
                                wrongLines[5].SetActive(false);
                                wrongLines[6].SetActive(false);
                                wireWins[0] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "cyan":
                            if (hit.collider.name.StartsWith("cyan") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[4].SetActive(true);
                                wrongLines[0].SetActive(false);
                                wrongLines[3].SetActive(false);
                                wireWins[1] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "darkblue":
                            if (hit.collider.name.StartsWith("darkblue") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[7].SetActive(true);
                                wrongLines[1].SetActive(false);
                                wrongLines[2].SetActive(false);
                                wireWins[2] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "darkred":
                            if (hit.collider.name.StartsWith("darkred") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[3].SetActive(true);
                                wrongLines[1].SetActive(false);
                                wrongLines[2].SetActive(false);
                                wireWins[3] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "green":
                            if (hit.collider.name.StartsWith("green") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[1].SetActive(true);
                                wrongLines[3].SetActive(false);
                                wrongLines[7].SetActive(false);
                                wireWins[4] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "orange":
                            if (hit.collider.name.StartsWith("orange") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[6].SetActive(true);
                                wrongLines[4].SetActive(false);
                                wrongLines[6].SetActive(false);
                                wireWins[5] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "pink":
                            if (hit.collider.name.StartsWith("pink") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[5].SetActive(true);
                                wrongLines[0].SetActive(false);
                                wrongLines[5].SetActive(false);
                                wireWins[6] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;
                        case "yellow":
                            if (hit.collider.name.StartsWith("yellow") && firstName != hit.collider.name)
                            {
                                firstCollider.enabled = false;
                                hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                                hit.collider.enabled = false;
                                lines[2].SetActive(true);
                                wrongLines[4].SetActive(false);
                                wrongLines[7].SetActive(false);
                                wireWins[7] = true;
                                Debug.Log("Win");
                            }
                            else
                            {
                                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                                lost = true;
                                lostPanel.GetComponent<Image>().sprite = lostMessage[1];
                                Debug.Log("Lost");
                            }
                            break;

                    }

                    firstChosen = false;
                }
                else
                {
                    firstName = hit.collider.name;
                    firstCollider = hit.collider;
                    Debug.Log(firstName + " Selected");
                    hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                    firstChosen = true;
                }
            }

            for (int i = 0; i < wireWins.Length; i++)
            {
                if (!wireWins[i])
                    return;
            }

            numsWinsCanvas[1].SetActive(true);
            FindObjectOfType<characterMovement>().testNumbers();
        }

        //A: 2 and 4 |B: 1| C: 0| D: 3

        if (Input.GetMouseButtonUp(0) && !lost && transform.position.x > 10.83f && !numsWinsCanvas[2].activeInHierarchy && beginLevel)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit.collider != null)
            {
                hit.collider.transform.GetChild(0).gameObject.SetActive(!hit.collider.transform.GetChild(0).gameObject.activeInHierarchy);
                char letter = hit.collider.name[hit.collider.name.Length - 1];
                int number = int.Parse(hit.collider.name[hit.collider.name.Length - 2].ToString());

                switch (letter)
                {
                    case 'A':
                        A[number-1] = !A[number-1];
                        break;
                    case 'B':
                        B[number-1] = !B[number-1];
                        break;
                    case 'C':
                        C[number-1] = !C[number-1];
                        break;
                    case 'D':
                        D[number-1] = !D[number-1];
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && transform.position.x > 10.83f && beginLevel)
        {
            if (!A[0] && A[1] && !A[2] && A[3]
                && B[0] && !B[1] && !B[2] && !B[3]
                && !C[0] && !B[1] && !B[2] && !B[3]
                && !D[0] && !D[1] && D[2] && !D[3])
            {
                numsWinsCanvas[2].SetActive(true);
                FindObjectOfType<characterMovement>().testNumbers();
            }
            else
            {
                lost = true;
                lostPanel.GetComponent<Image>().sprite = lostMessage[2];
            }
           
        }
    }
}
