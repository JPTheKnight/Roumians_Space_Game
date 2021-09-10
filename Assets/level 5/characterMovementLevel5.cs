using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovementLevel5 : MonoBehaviour
{
    public float speed;
    public float speedAscending;
    public GameObject instructionsPanel, instructionsPanel2;
    public SpriteRenderer scannerPhoto;
    public Sprite scannedPhoto;
    public GameObject greenBar;
    public GameObject scannedBones;
    public GameObject vitamin;

    bool grounded = false;
    bool scanning = false;
    bool onVitamin = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (/*!FindObjectOfType<level5>().lost && FindObjectOfType<level4>().beginLevel && */!scanning)
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

        if (onVitamin)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Destroy(vitamin);
            }
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

        if (collision.gameObject.tag == "Scanner")
        {
            StartCoroutine(ScanningOperation());
            collision.enabled = false;
        }

        if (collision.gameObject.tag == "Vitamin")
        {
            onVitamin = true;
            vitamin.transform.GetChild(0).gameObject.SetActive(true);
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

        if (collision.gameObject.tag == "Vitamin")
        {
            onVitamin = false;
            vitamin.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    IEnumerator ScanningOperation()
    {
        scanning = true;
        transform.position = new Vector3(-16.855f, 1.754f, 0);
        greenBar.SetActive(true);
        greenBar.GetComponent<Animator>().enabled = true;
        scannerPhoto.sprite = scannedPhoto;
        GetComponent<Animator>().SetBool("running", false);
        GetComponent<Animator>().SetBool("jumping", false);
        yield return new WaitForSeconds(10f);
        scanning = false;
        Destroy(greenBar);
        scannedBones.SetActive(true);

    }
}
