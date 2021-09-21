using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterMovementLevel7 : MonoBehaviour
{
    public float speed;
    public float speedAscending;
    public GameObject instructionsPanel, instructionsPanel2, xray_result;
    public SpriteRenderer scannerPhoto;
    public Sprite scannedPhoto;
    public GameObject greenBar;
    public GameObject scannedBones, checkTV;
    public GameObject TV, TV_Error, TV_Suggest;
    public GameObject dishCenter;
    public float dishRotationSpeed;
    public GameObject Rover;
    public float roverSpeed;
    public GameObject[] roverRocks;
    public GameObject foundRocks, rocksInfo;
    public float timeForO2;
    public GameObject O2Instructions, O2Puzzle, O2Warning, O2Lost, O2Congrats;
    public float timeToSolveO2;
    public GameObject[] buttonGlow;
    public float timeChezDr;
    public TextMeshProUGUI timeO2Text1, timeO2Text2, timeDrText;
    public Animator dirtyEnviro;
    public GameObject doctorLost;
    public GameObject[] doctorQNA;
    public GameObject doctorDoor;
    public AudioSource[] fairouzes;
    public Image microphone;
    public Sprite microOff, microOn;
    public GameObject redRoom;

    bool O2Damaged = false;

    public int rocksCollected = 0;

    //rover: 25f 78.36f

    bool grounded = false;
    bool scanning = false;
    [HideInInspector]
    public bool xray = false;
    bool radar = false, radar_repaired = true;
    bool tv = false, tv_working = false, tv_suggest = false;
    bool roverWalking = false, rover = false;
    bool micro = false, micro_rocks = false, rock_name = false;
    bool o2instruct = false, o2tank = false, o2warning = false;
    bool enteredDoctor = false, talkDoctor = false, doctorPressed = false, doctorNext = false, answer1 = false, answer2 = false;
    bool dirtyEnviroOn = false;

    level7 lvl7;
    void Start()
    {
        lvl7 = FindObjectOfType<level7>();
    }

    float msgtimer = 1f;
    bool activateTimer = false;
    int qnaID = 0;

    void Update()
    {
        if (lvl7.beginLevel && timeForO2 > 0 && timeForO2 < 10000f)
        {
            timeForO2 -= Time.deltaTime;
        }
        if (!lvl7.lost && lvl7.beginLevel && !scanning && !xray && radar_repaired && !tv_suggest && !micro_rocks && !rock_name && !o2warning && !o2tank && !O2Puzzle.activeInHierarchy && !doctorPressed && timeForO2 <= 0)
        {
            O2Damaged = true;
            timeForO2 = 1000000f;
            o2warning = true;
            O2Warning.SetActive(true);
            StartCoroutine(RedRoom());
            lvl7.fairouzesPlay(5);
        }

        if (!lvl7.lost && lvl7.beginLevel && !scanning && !xray && radar_repaired && !tv_suggest && !micro_rocks && !rock_name && !o2warning && !o2tank && !O2Puzzle.activeInHierarchy && !doctorPressed)
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

            if (transform.position.x > Camera.main.transform.position.x && Camera.main.transform.position.x < 72f)
            {
                Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, -10f);
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
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speedAscending);
                GetComponent<Animator>().SetBool("jumping", false);
            }
        }

        if (xray && Input.GetKeyDown(KeyCode.Space) && xray_result.activeInHierarchy)
        {
            xray_result.SetActive(false);
        }

        if (radar && Input.GetKeyDown(KeyCode.Return))
        {
            radarCollisionSaved.enabled = false;
            radar_repaired = false;
            radar = false;
        }

        if (!radar_repaired)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                dishCenter.transform.Rotate(-Vector3.forward * dishRotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                dishCenter.transform.Rotate(Vector3.forward * dishRotationSpeed * Time.deltaTime);
            }
            if ((((dishCenter.transform.rotation.eulerAngles.z + 540) % 360) - 180) > 36.347f && (((dishCenter.transform.rotation.eulerAngles.z + 540) % 360) - 180) < 38.347f)
            {
                radar_repaired = true;
                lvl7.winningThings[1] = true;
                tv_working = true;
                TV.transform.GetChild(1).gameObject.SetActive(false);
                dishCenter.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        if (tv && Input.GetKeyDown(KeyCode.Return) && tv_working)
        {
            TV_Suggest.SetActive(true);
            tv_suggest = true;
            lvl7.fairouzesPlay(2);
        }

        if (tv_suggest && Input.GetKeyDown(KeyCode.Space))
        {
            TV_Suggest.SetActive(false);
            tv_suggest = false;
            fairouzes[2].Stop();
        }

        if (rover && Input.GetKeyDown(KeyCode.Return))
        {
            roverWalking = true;
            Rover.transform.Find("Canvas").gameObject.SetActive(false);
            roverCollisionSaved.enabled = false;
        }

        if (roverWalking)
        {
            if (Rover.transform.position.x < transform.position.x - 1f && transform.position.x < 77.36f && Vector2.Distance(Rover.transform.position, transform.position) < 7.5f)
            {
                Rover.transform.Translate(Vector2.right * roverSpeed * Time.deltaTime);
                Rover.transform.rotation = new Quaternion(0, 0, 0, 0);
                Rover.GetComponent<Animator>().SetBool("walking", true);
            }
            if (Rover.transform.position.x > transform.position.x + 1f && transform.position.x > 24f && Vector2.Distance(Rover.transform.position, transform.position) < 7.5f)
            {
                Rover.transform.Translate(Vector2.right * roverSpeed * Time.deltaTime);
                Rover.transform.rotation = new Quaternion(0, 180f, 0, 0);
                Rover.GetComponent<Animator>().SetBool("walking", true);
            }
            if (Rover.transform.position.x > transform.position.x - 0.1f && Rover.transform.position.x < transform.position.x + 0.1f || transform.position.x < 24f || transform.position.x > 77.36f)
            {
                Rover.transform.rotation = new Quaternion(0, 0, 0, 0);
                Rover.GetComponent<Animator>().SetBool("walking", false);
            }
        }

        if (micro && Input.GetKeyDown(KeyCode.Return) && rocksCollected == 6)
        {
            micro_rocks = true;
            micro = false;
            foundRocks.SetActive(true);
            lvl7.fairouzesPlay(4);
        }

        if (activateTimer)
        {
            msgtimer -= Time.deltaTime;
            if (msgtimer < 0)
            {
                rock_name = true;
                activateTimer = false;
            }
        }

        if (micro_rocks && Input.GetKeyDown(KeyCode.Space))
        {
            micro_rocks = false;
            foundRocks.SetActive(false);
            rocksInfo.SetActive(true);
            activateTimer = true;
        }

        if (rock_name && Input.GetKeyDown(KeyCode.Space))
        {
            rocksInfo.SetActive(false);
            rock_name = false;
            microCollisionSaved.enabled = false;
            lvl7.winningThings[2] = true;

            for (int i = 0; i < roverRocks.Length; i++)
            {
                roverRocks[i].SetActive(false);
            }
        }

        if (o2warning && Input.GetKeyDown(KeyCode.Space))
        {
            o2warning = false;
            O2Warning.SetActive(false);
            fairouzes[5].Stop();
        }

        if (o2instruct && Input.GetKeyDown(KeyCode.Return))
        {
            o2instruct = false;
            O2Instructions.SetActive(true);
            o2tank = true;
            lvl7.fairouzesPlay(6);
        }

        if (o2tank && Input.GetKeyDown(KeyCode.Space))
        {
            o2tank = false;
            O2Instructions.SetActive(false);
            fairouzes[6].Stop();
            O2Puzzle.SetActive(true);
        }

        if (O2Damaged && !O2Warning.activeInHierarchy)
        {
            if (timeToSolveO2 > 0)
                timeToSolveO2 -= Time.deltaTime;
            timeO2Text1.text = timeToSolveO2.ToString("00");
            timeO2Text2.text = timeToSolveO2.ToString("00");

            if (timeToSolveO2 < 0 && !lvl7.lost)
            {
                O2Lost.SetActive(true);
                lvl7.lost = true;
                lvl7.fairouzesPlay(9);
            }
        }
        else
        {
            timeO2Text1.text = "";
            timeO2Text2.text = "";
        }

        if ((transform.position.y > 1.73f && transform.position.x > 2f) || doctorDoor.GetComponent<Animator>().GetBool("on"))
        {
            enteredDoctor = true;
        }
        else
        {
            if (!doctorDoor.GetComponent<Animator>().GetBool("on"))
                enteredDoctor = false;
        }

        if (enteredDoctor)
        {
            if (timeChezDr > 0)
                timeChezDr -= Time.deltaTime;
            timeDrText.text = timeChezDr.ToString("00");
            dirtyEnviro.gameObject.SetActive(true);
            dirtyEnviro.enabled = true;
            dirtyEnviroOn = true;

            if (timeChezDr < 0 && !lvl7.lost)
            {
                doctorLost.SetActive(true);
                lvl7.lost = true;
                lvl7.fairouzesPlay(8);
            }
        }
        else
        {
            if (dirtyEnviroOn)
            {
                dirtyEnviro.gameObject.SetActive(false);
                dirtyEnviro.Play("showDirtyEnviro", -1, 0);
                timeDrText.text = "";
                dirtyEnviro.enabled = false;
                timeChezDr = 30f;
                dirtyEnviroOn = false;
            }
        }

        if (talkDoctor && Input.GetKeyDown(KeyCode.Return))
        {
            doctorPressed = true;
            doctorQNA[0].SetActive(true);
        }

        if (doctorPressed && qnaID <= 6)
        {
            if (answer1 && qnaID == 2)
            {
                return;
            }
            else if (!answer1 && qnaID == 2)
            {
                doctorQNA[1].SetActive(false);
                doctorQNA[2].SetActive(true);
            }

            if (answer2 && qnaID == 6)
            {
                return;
            }
            else if (!answer2 && qnaID == 6)
            {
                doctorQNA[5].SetActive(false);
                doctorPressed = false;
                doctorCollisionSaved.enabled = false;
                lvl7.winningThings[3] = true;
            }

            doctorNext = true;
            if (doctorNext && Input.GetKeyDown(KeyCode.Space) && qnaID != 1 && qnaID != 5)
            {
                doctorQNA[qnaID].SetActive(false);
                doctorQNA[qnaID + 1].SetActive(true);
                qnaID++;
                doctorNext = false;
            }
            if (qnaID == 1)
            {
                answer1 = true;
                doctorNext = false;
                qnaID++;
            }
            if (qnaID == 5)
            {
                answer2 = true;
                doctorNext = false;
                qnaID++;
            }
        }

        if (fairouzesPlaying())
        {
            microphone.sprite = microOn;
        }
        else
        {
            microphone.sprite = microOff;
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

    Collider2D tvCollisionSaved, radarCollisionSaved, roverCollisionSaved, microCollisionSaved, o2CollisionSaved, doctorCollisionSaved;
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
                fairouz1 = true;
                lvl7.fairouzesPlay(0);
            }
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(true);
            if (!fairouz2)
            {
                fairouz2 = true;
                lvl7.fairouzesPlay(3);
            }
        }

        if (collision.gameObject.tag == "Scanner")
        {
            collision.enabled = false;
            StartCoroutine(ScanningOperation());
        }

        if (collision.gameObject.tag == "RadarDish")
        {
            radar = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            radarCollisionSaved = collision;
        }

        if (collision.gameObject.tag == "Microscope")
        {
            micro = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            microCollisionSaved = collision;
        }

        if (collision.gameObject.tag == "Rover")
        {
            rover = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            roverCollisionSaved = collision;
        }

        if (collision.gameObject.tag == "TV")
        {
            tv = true;
            TV.transform.GetChild(0).gameObject.SetActive(true);
            tvCollisionSaved = collision;
            if (!tv_working)
            {
                TV_Error.SetActive(true);
                if (!fairouz3)
                {
                    lvl7.fairouzesPlay(1);
                    fairouz3 = true;
                }
            }
        }

        if (collision.gameObject.tag == "Rock" && roverWalking && Vector2.Distance(Rover.transform.position, transform.position) < 7.5f)
        {
            Destroy(collision.gameObject);
            roverRocks[rocksCollected].SetActive(true);
            rocksCollected++;
        }

        if (collision.gameObject.tag == "O2Tank" && O2Damaged)
        {
            if (!o2instruct)
            {
                o2instruct = true;
            }
            collision.transform.GetChild(0).gameObject.SetActive(true);
            o2CollisionSaved = collision;
        }

        if (collision.gameObject.tag == "Doctor")
        {
            talkDoctor = true;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            doctorCollisionSaved = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && grounded == true)
        {
            grounded = false;
            GetComponent<Animator>().SetBool("jumping", true);
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(false);
            fairouzes[0].Stop();
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(false);
            fairouzes[3].Stop();
        }

        if (collision.gameObject.tag == "Door")
        {
            collision.gameObject.transform.parent.GetChild(0).GetComponent<Animator>().SetBool("on", false);
        }

        if (collision.gameObject.tag == "RadarDish")
        {
            radar = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Microscope")
        {
            micro = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Rover")
        {
            rover = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "TV")
        {
            TV.transform.GetChild(0).gameObject.SetActive(false);
            if (!tv_working)
            {
                TV_Error.SetActive(false);
                fairouzes[1].Stop();
            }
            tv = false;
        }

        if (collision.gameObject.tag == "O2Tank" && O2Damaged)
        {
            o2instruct = false;
            o2tank = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Doctor")
        {
            talkDoctor = false;
            collision.transform.GetChild(0).gameObject.SetActive(false);
            doctorCollisionSaved = collision;
        }
    }

    public void a1(int id)
    {
        if (id == 2)
        {
            answer1 = false;
        }
        else
        {
            lvl7.lost = true;
            doctorLost.SetActive(true);
        }
    }

    public void a3(int id)
    {
        if (id == 0)
        {
            answer2 = false;
        }
        else
        {
            lvl7.lost = true;
            doctorLost.SetActive(true);
        }
    }

    public void Puzzle(int id)
    {
        buttonGlow[id].SetActive(true);

        if (id != 4)
        {
            O2Lost.SetActive(true);
            lvl7.lost = true;
            lvl7.fairouzesPlay(9);
        }
        if (id == 4)
        {
            O2Damaged = false;
            O2Congrats.SetActive(true);
            lvl7.fairouzesPlay(7);
            StopAllCoroutines();
            Destroy(redRoom);
            Destroy(O2Congrats, 6f);
        }

        O2Puzzle.SetActive(false);
        o2CollisionSaved.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void resetMics(int id)
    {
        fairouzes[id].Play();
    }

    IEnumerator ScanningOperation()
    {
        scanning = true;
        transform.position = new Vector3(-17.255f, 1.754f, 0);
        greenBar.SetActive(true);
        greenBar.GetComponent<Animator>().enabled = true;
        scannerPhoto.sprite = scannedPhoto;
        GetComponent<Animator>().SetBool("running", false);
        GetComponent<Animator>().SetBool("jumping", false);
        yield return new WaitForSeconds(10f);
        Destroy(greenBar);
        scannedBones.SetActive(true);
        xray = true;
        scanning = false;
    }

    IEnumerator RedRoom()
    {
        while(true)
        {
            redRoom.SetActive(true);
            yield return new WaitForSeconds(1f);
            redRoom.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }
}
