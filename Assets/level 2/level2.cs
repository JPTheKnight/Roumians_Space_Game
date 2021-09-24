using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class level2 : MonoBehaviour
{
    public GameObject ship;
    public GameObject horBar, horArrow;
    public GameObject canvas;
    public LineRenderer line;
    public Slider fuelSlider;
    public GameObject chooseFuelBG;
    public GameObject Fade;
    public GameObject tooExpensive;
    public TextMeshProUGUI countdown;
    public GameObject WonPanel;
    public GameObject lostPanel;
    public AudioSource flySound;

    public float shipSpace;
    public float rotationSpeed;
    public float toEmptyFuel;
    public float maxShortD;

    public GameObject instructionsPanel;
    bool beginLevel = false;

    float fuelCapacity = 0;

    levelsManager lm;

    //3108

    //arrow: -1.268 1.268

    void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        fuelCapacity = toEmptyFuel;
        Fade.GetComponent<Animator>().Play("fadeOutAnim");
    }

    bool fuelChosen = false;

    float waitSecs = 0;
    bool start = false, timerDone = false;
    bool won = false;
    bool waitWon = false;
    float wonSecs = 0;

    bool lost = false;
    bool waitLost = false;
    float lostSecs = 0;
    string lostMsg = "FAIL!\nOUT OF ORBIT!";

    private void Update()
    {
        if (lm.PausePanel.activeInHierarchy) { flySound.Pause(); return; }
        else { flySound.UnPause(); }

        if (won)
        {
            wonSecs += Time.deltaTime;
            lm.pauseButton.SetActive(false);

            if (wonSecs > 3f)
            {
                flySound.Stop();
            }

            if (wonSecs > 4f && !waitWon)
            {
                ship.transform.GetChild(0).GetComponent<Animator>().SetBool("set", true);
                WonPanel.SetActive(true);
                if (PlayerPrefs.GetInt("LevelsUnlocked") < 3)
                    PlayerPrefs.SetInt("LevelsUnlocked", 3);
                waitWon = true;
            }
            return;
        }

        if (lost)
        {
            lostSecs += Time.deltaTime;
            lm.pauseButton.SetActive(false);

            if (!waitLost)
            {
                Fade.SetActive(true);
                Fade.GetComponent<Animator>().Play("fadeInAnim");
                lostPanel.SetActive(true);
                lostPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lostMsg;
                waitLost = true;
            }

            if (lostSecs > 4.5f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }

            return;
        }

        if (waitSecs < 4f)
        {
            waitSecs += Time.deltaTime;
        }

        if (waitSecs > 3f && !beginLevel)
        {
            instructionsPanel.SetActive(true);
            Fade.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                instructionsPanel.SetActive(false);
                beginLevel = true;
                chooseFuelBG.SetActive(true);
            }
        }

        if (fuelChosen)
        {
            fuelChosen = false;
            StartCoroutine(SpaceShipCountDown());
        }

        if (beginLevel && !fuelChosen)
        {
            fuelSlider.transform.GetChild(2).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = fuelSlider.value.ToString("F1") + "k";
        }

        if (beginLevel && timerDone)
        {
            Camera.main.transform.Translate(Vector3.up * shipSpace * Time.deltaTime);
            start = true;
        }

        if (start && timerDone)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
            }

            fuelCapacity -= Time.deltaTime * 80;
            horArrow.transform.localPosition = new Vector2(-0.084f, (fuelCapacity / toEmptyFuel) * 1.268f * 2f - 1.268f);

            if (horArrow.transform.localPosition.y < -1.268f)
            {
                lost = true;
                lostMsg = "NO MORE FUEL!";
            }

            if (GetShortestDistance())
            {
                lost = true;
            }

            if (Camera.main.transform.position.x < -7)
            {
                Camera.main.GetComponent<Animator>().enabled = true;
                start = false;
                won = true;
            }
        }
    }

    IEnumerator SpaceShipCountDown()
    {
        countdown.GetComponent<Animator>().enabled = true;
        countdown.text = "3";
        yield return new WaitForSeconds(1f);
        countdown.text = "2";
        yield return new WaitForSeconds(1f);
        countdown.text = "1";
        yield return new WaitForSeconds(1f);
        Destroy(countdown.gameObject);
        yield return new WaitForSeconds(0.3f);
        timerDone = true;

    }

    bool GetShortestDistance()
    {
        float shortestDistance = Vector2.Distance(line.GetPosition(0), ship.transform.position);

        for (int i = 0; i < line.positionCount; i++)
        {
            if (shortestDistance > Vector2.Distance(line.GetPosition(i), ship.transform.position))
            {
                shortestDistance = Vector2.Distance(line.GetPosition(i), ship.transform.position);
            }
        }

        if (shortestDistance < maxShortD)
        {
            return false;
        }

        return true;
    }

    public void Submit()
    {
        fuelCapacity = fuelSlider.value;
        if (fuelCapacity > 620)
        {
            StartCoroutine(submitIE());
            return;
        }
        fuelChosen = true;
        chooseFuelBG.SetActive(false);
        flySound.Play();
        ship.transform.GetChild(0).GetComponent<Animator>().enabled = true;
    }

    IEnumerator submitIE()
    {
        tooExpensive.SetActive(true);
        yield return new WaitForSeconds(2f);
        tooExpensive.SetActive(false);
    }

    public void setLostTrue()
    {
        lost = true;
        lostMsg = "BE CAREFUL!";
    }

    public bool getLost()
    {
        return lost;
    }
}
