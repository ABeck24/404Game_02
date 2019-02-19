using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text deadText;
    public GameObject pause;
    public GameObject Guffin;
    bool cooldown;
    public float cooldownTimer;
    float savedCooldown;
    private bool dead;
    private Camera cam;
    public Transform shockOrigin;
    private LineRenderer shockLine;
    public float shockRange;
    public GameObject drone;
    public Material hand;
    public GameObject cd;
    private float cdSaved;
    bool on;
    bool winGame;

    void Start() {
        savedCooldown = cooldownTimer;
        cooldown = false;
        deadText.text = "";
        hand.EnableKeyword("_Emission");
        shockLine = GetComponent<LineRenderer>();
        cam = GetComponentInChildren<Camera>();
        cdSaved = cd.GetComponent<Slider>().value;


    }

    // Update is called once per frame
    void Update()
    {

        if (winGame == true && Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
        PauseGame();

        if ((cooldown == false && Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Joystick1Button10)))
        {
            StartCoroutine(ShotEffect());
            StartCoroutine(HandCooldown(true));
            Vector3 aiming = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            shockLine.SetPosition(0, shockOrigin.position);
            Debug.Log("Pew");
            cooldown = true;
            Physics.Raycast(aiming, cam.transform.forward, out hit, Mathf.Infinity);//shockRange);
            Collider col = hit.collider;

            if (col.gameObject == drone && col.isTrigger != true)
            {
                shockLine.SetPosition(1, hit.point);
                Debug.Log("Hit Drone");
                StartCoroutine(GameObject.Find("Drone").GetComponent<PlayerDetection>().DisableDrone());

            }
            else
            {
                shockLine.SetPosition(1, aiming + (cam.transform.forward * shockRange));
            }


        }
        /*if (Physics.Raycast(transform.forward, cam.transform.position, shockRange, 8, QueryTriggerInteraction.Collide) && Input.GetKey(KeyCode.E))
            {
            Guffin.SetActive(false);
            this.GetComponent<AudioSource>().Play();
            }*/

        if (dead == true)
        {
            if ((Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button3)))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(1);

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Guffin)
        {
            if ((Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Joystick1Button1)))
            {
                Guffin.SetActive(false);
                this.GetComponent<AudioSource>().Play();
                winGame = true;
                Time.timeScale = 0.0f;
                deadText.text = "You Win! Press Escape to Close";


            }

        }
    }
    public void Death()
    {
        deadText.text = "Game Over! Press R to restart";
        Time.timeScale = 0;
        cooldown = true;
        dead = true;


    }
    private IEnumerator ShotEffect()
    {
        GetComponentInChildren<AudioSource>().Play();

        shockLine.enabled = true;
        hand.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.01f);

        shockLine.enabled = false;

    }
    IEnumerator HandCooldown(bool on)
    {
        
        while (on == true)
        {
            cd.SetActive(true);
            //cd.GetComponent<Slider>().enabled = true;
            InvokeRepeating("Sliding", 0, Time.deltaTime);
            //Debug.Log(cd.GetComponent<Slider>().value);
            if (cd.GetComponent<Slider>().value == 0)
            {
                CancelInvoke();
                hand.EnableKeyword("_EMISSION");
                Debug.Log("Off Cooldown");
                cd.GetComponent<Slider>().value = 1;
                cd.SetActive(false);
                cooldown = false;
                on = false;
            }
            yield return null;
        }
        /*while(cooldown == true)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            if(cooldownTimer <= 0)
            {
                cooldown = false;
                cooldownTimer = savedCooldown;
                Debug.Log("Off Cooldown");
                
                
            }
            yield return null;
        }
        StopCoroutine(coroutine);*/
        Debug.Log("End Cooldown");

    }
    private void PauseGame()
    {
            if ((Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button3)) && on == true)
            {
                SceneManager.LoadScene(1);
                Time.timeScale = 1;
            }
            if ((Input.GetKeyUp(KeyCode.Escape)|| Input.GetKey(KeyCode.Joystick1Button7)) && on == false && winGame == false)
            {
                Time.timeScale = 0;
                on = true;
                pause.SetActive(true);
                deadText.text = "Paused \n" + "R to Restart \n" + "ESC to Unpause \n" + "X to Quit"; 
            }
            else if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKey(KeyCode.Joystick1Button7)) && on == true && winGame == false)
            {
                Time.timeScale = 1;
                on = false;
                pause.SetActive(false);
                deadText.text = "";
            }
            if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Joystick1Button2)))
            {
                Application.Quit();
            }
    }
    void Sliding()
    {
        cd.GetComponent<Slider>().value -= Time.deltaTime*cooldownTimer;
    }
}
