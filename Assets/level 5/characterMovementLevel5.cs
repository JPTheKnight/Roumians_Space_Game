using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterMovementLevel5 : MonoBehaviour
{
    public float speed;
    public float speedAscending;
    public GameObject instructionsPanel, instructionsPanel2, xray_result, little_gem;
    public SpriteRenderer scannerPhoto;
    public Sprite scannedPhoto, xray_explanation;
    public GameObject greenBar;
    public GameObject scannedBones;
    public GameObject vitamin, bluePills;
    public GameObject TV, TVPanel;
    public GameObject SunMsg;
    public AudioSource[] fairouzes;
    public Image micro;
    public Sprite microOff, microOn;
    public AudioSource walkingSound, scannerBeep, doorOpen, doorClose, smallWin;

    bool fixCamera = false;
    bool grounded = false;
    bool scanning = false;
    bool onVitamin = false, onBluePills = false;
    bool xray = false, solution = false;
    bool lgem = false;
    bool tv = false;
    bool sunMsg = false;

    bool isWalking = false;

    float runningTime = 10f;

    level5 lvl5;
    levelsManager lm;
    void Start()
    {
        lm = FindObjectOfType<levelsManager>();
        lvl5 = FindObjectOfType<level5>();
    }

    void Update()
    {
        if (lm.PausePanel.activeInHierarchy) { walkingSound.Pause(); doorOpen.Pause(); doorClose.Pause(); smallWin.Pause(); scannerBeep.Pause(); return; }
        else { walkingSound.UnPause(); doorOpen.UnPause(); doorClose.UnPause(); smallWin.UnPause(); scannerBeep.UnPause(); }

        transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "00:" + runningTime.ToString("00");

        if (!lvl5.lost && lvl5.beginLevel && !scanning && !xray && !SunMsg.activeInHierarchy && !TVPanel.activeInHierarchy)
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

                if (transform.position.x < Camera.main.transform.position.x && Camera.main.transform.position.x > -17.79f)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, -10f);
                }

                if (transform.position.x > Camera.main.transform.position.x && Camera.main.transform.position.x < 18.76f)
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

        if (onVitamin)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Camera.main.GetComponent<level5>().winningThings[1] = true;
                smallWin.Play();
                Destroy(vitamin);
                onVitamin = false;
            }
        }

        if (onBluePills)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Destroy(bluePills);
                lvl5.sunShown1 = false;
                lvl5.sunShown2 = false;
                lvl5.hitBySun = false;
                lvl5.firstSun = true;
                lvl5.resetSunTime();
                lvl5.solarHit.Stop();
                onBluePills = false;
            }
        }

        if (SunMsg.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            SunMsg.SetActive(false);
            fairouzes[1].Stop();
        }

        if (xray && Input.GetKeyDown(KeyCode.Space))
        {
            fairouzes[0].Stop();
            xray_result.GetComponent<Image>().sprite = xray_explanation;
            smallWin.Play();
            xray_result.transform.GetChild(1).gameObject.SetActive(true);
            xray_result.transform.GetChild(0).gameObject.SetActive(false);
            xray = false;
        }

        if ((lvl5.sunShown1 && transform.position.x > 10.53f) || 
            (lvl5.sunShown2 && transform.position.x > -7.40565f && transform.position.x < 7.888f))
        {
            FindObjectOfType<level5>().hitBySun = true;
            Debug.Log("Gi");
        }

        if (lgem && Input.GetKeyDown(KeyCode.Space))
        {
            little_gem.SetActive(false);
            lgem = false;
        }

        if (tv && Input.GetKeyDown(KeyCode.Return))
        {
            TVPanel.SetActive(true);
            if (!fairouz6)
            { 
                lvl5.fairouzesPlay(3);
                fairouz6 = true;
            }
        }

        if (TVPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            TVPanel.SetActive(false);

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

    public void fairouzMicPressed()
    {
        lvl5.fairouzesPlay(9);
    }

    Collider2D collisionSaved, tvCollisionSaved;
    bool fairouz1 = false, fairouz3 = false, fairouz6 = false;
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

            if (!sunMsg)
            {
                sunMsg = true;
                SunMsg.SetActive(true);
                lvl5.fairouzesPlay(1);
                isWalking = false;
                walkingSound.Stop();
                GetComponent<Animator>().SetBool("running", false);
            }
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(true);

            if (!fairouz1)
            {
                lvl5.fairouzesPlay(5);
                fairouz1 = true;
            }
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(true);

            if (!fairouz3)
            {
                lvl5.fairouzesPlay(2);
                fairouz3 = true;
            }
        }

        if (collision.gameObject.tag == "Scanner")
        {
            if (!solution)
            {
                collisionSaved = collision;
                StartCoroutine(ScanningOperation());
                solution = true;
            }
            else
            {
                xray_result.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "Vitamin")
        {
            onVitamin = true;
            vitamin.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "BluePills")
        {
            onBluePills = true;
            bluePills.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Littlegem")
        {
            little_gem.SetActive(true);
            lgem = true;
        }

        if (collision.gameObject.tag == "TV")
        {
            tv = true;
            TV.transform.GetChild(0).gameObject.SetActive(true);
            tvCollisionSaved = collision;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Treadmill")
        {
            GetComponent<Animator>().SetBool("jumping", false);
            GetComponent<Animator>().SetBool("running", true);
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * 300f * Time.deltaTime);
            fixCamera = true;
            if (runningTime > 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                runningTime -= Time.deltaTime;
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
                Camera.main.GetComponent<level5>().winningThings[2] = true;
                smallWin.Play();
                Destroy(collision.gameObject);
            }
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
            fairouzes[5].Stop();
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(false);
            fairouzes[2].Stop();
        }

        if (collision.gameObject.tag == "Scanner")
        {
            if (solution)
            {
                xray_result.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "Vitamin")
        {
            onVitamin = false;
            vitamin.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "BluePills")
        {
            onBluePills = false;
            bluePills.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Treadmill")
        {
            fixCamera = false;
        }

        if (collision.gameObject.tag == "TV")
        {
            tv = false;
            TV.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void resetMics(int id)
    {
        fairouzes[id].Play();
    }

    IEnumerator ScanningOperation()
    {
        collisionSaved.enabled = false;
        scannerBeep.Play();
        scanning = true;
        transform.position = new Vector3(-16.855f, 1.754f, 0);
        greenBar.SetActive(true);
        greenBar.GetComponent<Animator>().enabled = true;
        scannerPhoto.sprite = scannedPhoto;
        GetComponent<Animator>().SetBool("running", false);
        GetComponent<Animator>().SetBool("jumping", false);
        yield return new WaitForSeconds(10f);
        Destroy(greenBar);
        scannedBones.SetActive(true);
        yield return new WaitForSeconds(3f);
        xray = true;
        scanning = false;
        Camera.main.GetComponent<level5>().winningThings[0] = true;
        lvl5.fairouzesPlay(0);
        xray_result.SetActive(true);
        collisionSaved.enabled = true;
    }

    IEnumerator WalkingSound()
    {
        walkingSound.Stop();
        yield return new WaitForSeconds(0.2f);
        walkingSound.Play();
    }
}
