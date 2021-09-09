using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    public float speed;
    public float speedAscending;
    public GameObject instructionsPanel, instructionsPanel2;
    public GameObject[] plantsMaafnin;

    public Animator[] plants;
    bool[] plantsChosen = { false, false, false, false };
    bool[] plantsDone = { false, false, false, false };
    bool wonPlants = false;
    bool grounded = false;

    void Start()
    {
        
    }

    void Update()
    {

        if (!FindObjectOfType<level4>().lost && FindObjectOfType<level4>().beginLevel)
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

            if (Input.GetKeyDown(KeyCode.Space) && grounded && transform.position.x < -7.40565f)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * speedAscending);
                GetComponent<Animator>().SetBool("jumping", false);
            }
        }

        if (plantsChosen[0] || plantsChosen[1] || plantsChosen[2] || plantsChosen[3])
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (plantsChosen[i])
                    {
                        if (i != 0 && !plantsDone[i - 1])
                        {
                            plantsMaafnin[i].SetActive(true);
                            plants[i].gameObject.SetActive(false);
                            FindObjectOfType<level4>().lost = true;
                            return;
                        }

                        Debug.Log("Won");
                        plants[i].transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        plantsDone[i] = true;
                        plants[i].enabled = true;
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
            FindObjectOfType<level4>().numsWinsCanvas[0].SetActive(true);
        }
    }

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
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(true);
        }

        if (collision.gameObject.name == "rosePlant")
        {
            plantsChosen[1] = true;
        }

        if (collision.gameObject.name == "redPlant")
        {
            plantsChosen[0] = true;
        }

        if (collision.gameObject.name == "yellowPlant")
        {
            plantsChosen[2] = true;
        }

        if (collision.gameObject.name == "bluePlant")
        {
            plantsChosen[3] = true;
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
        }

        if (collision.gameObject.tag == "Instructions")
        {
            instructionsPanel.SetActive(false);
        }

        if (collision.gameObject.tag == "Instructions2")
        {
            instructionsPanel2.SetActive(false);
        }

        if (collision.gameObject.name == "rosePlant")
        {
            plantsChosen[1] = false;
        }

        if (collision.gameObject.name == "redPlant")
        {
            plantsChosen[0] = false;
        }

        if (collision.gameObject.name == "yellowPlant")
        {
            plantsChosen[2] = false;
        }

        if (collision.gameObject.name == "bluePlant")
        {
            plantsChosen[3] = false;
        }
    }
}
