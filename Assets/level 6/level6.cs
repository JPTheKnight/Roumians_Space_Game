using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class level6 : MonoBehaviour
{
    public GameObject capsule;
    public GameObject bar, arrowCenter, arrow;
    public GameObject vertBar, vertArrow;
    public GameObject canvas;
    public GameObject resultText;
    public GameObject shipShadow;
    public GameObject Fade;
    public GameObject WonPanel;

    public float capsuleSpeed;
    public float rotationSpeed;
    public float maxZRot;

    public GameObject instructionsPanel;
    bool beginLevel = false;

    levelsManager lm;

    //ycoord: -4.42
    //angle: -10 10

    //arrow:  -41 41   scale: 0.3372366 0.249251

    //vert arrow: 1.272 -1.272

    //distance from ground: 14.78

    void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        capsule.transform.Rotate(-Vector3.forward * Random.Range(-.05f, .05f));
        Fade.GetComponent<Animator>().Play("fadeOutAnim");
    }

    float waitSecs = 0;
    bool camOn = false;

    bool won = false;
    bool lost = false;
    bool waitLost = false;
    float lostSecs = 0;

    bool shadowOn = false;
    private void Update()
    {

        if (won)
        {
            return;
        }

        if (lost)
        {
            lostSecs += Time.deltaTime;
            lm.pauseButton.SetActive(false);

            if (!waitLost)
            {
                resultText.SetActive(true);
                Fade.SetActive(true);
                Fade.GetComponent<Animator>().Play("fadeInAnim");
                waitLost = true;
            }

            if (lostSecs > 4.5f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }

            return;
        }

        if (waitSecs < 3f)
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
            }
        }

        if (beginLevel && !camOn)
        {
            capsule.transform.Translate(new Vector2(0, -1f * capsuleSpeed * Time.deltaTime));
            vertBar.SetActive(true);
        }

        if (capsule.transform.position.y < Camera.main.transform.position.y)
        {
            capsule.transform.parent = Camera.main.transform;
            bar.SetActive(true);
            arrowCenter.SetActive(true);
            camOn = true;
        }

        vertArrow.transform.localPosition = new Vector2(-0.06680977f, (Vector2.Distance(capsule.transform.position,
        new Vector2(-3.41f, -5f)) / 11.61f) * 2f - 1.272f);

        if (camOn)
        {
            Camera.main.transform.Translate(new Vector2(0, -1f * capsuleSpeed * Time.deltaTime));
            arrowCenter.transform.rotation = new Quaternion(0, 0, capsule.transform.rotation.z / 2f, 1);
            arrow.transform.localScale = Vector3.one * (Mathf.Abs((capsule.transform.rotation.z/6f)) + 0.249251f);

            if (((arrowCenter.transform.rotation.eulerAngles.z + 540) % 360) - 180 < 0)
            {
                capsule.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
            }

            if (((arrowCenter.transform.rotation.eulerAngles.z + 540) % 360) - 180 > 0)
            {
                capsule.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }

            if (Mathf.Abs(((arrowCenter.transform.rotation.eulerAngles.z + 540) % 360) - 180) > 40.77)
                lost = true;

            if (capsule.transform.position.y < -4.63f)
            {
                float zRot = Mathf.Abs(((arrowCenter.transform.rotation.eulerAngles.z + 540) % 360) - 180);

                if (zRot > maxZRot)
                {
                    lost = true;
                }
                else
                {
                    WonPanel.SetActive(true);
                    lm.pauseButton.SetActive(false);
                    if (PlayerPrefs.GetInt("LevelsUnlocked") < 7)
                        PlayerPrefs.SetInt("LevelsUnlocked", 7);
                    won = true;
                }
            }

            if (Vector2.Distance(capsule.transform.position, new Vector2(-3.41f, -4.43f)) < 0.05f && !shadowOn)
            {
                GameObject shadow = Instantiate(shipShadow, new Vector2(capsule.transform.GetChild(0).transform.position.x, capsule.transform.GetChild(0).transform.position.y - 0.25f), Quaternion.identity);
                shadowOn = true;
            }
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && camOn)
        {
            capsule.transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime * 1.2f);
        }

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && camOn)
        {
            capsule.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime * 1.2f);
        }
    }

}
