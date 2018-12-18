using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Slider = UnityEngine.UI.Slider;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Rigidbody2D myRigidbody2D;
    public Animator myAnimator;

    [SerializeField] private float jumpPowerX, jumpPowerY, jumpPowerXMax = 6.5f, jumpPowerYMax = 13.5f;
    [SerializeField] private bool ifCanShoot;
    [SerializeField] private bool ifCanJump;

    private float tresholdX = 7f;
    private float tresholdY = 14f;

    public bool setPower, didJump, isTimeSlow;

    private Slider powerBar;
    private float powerBarTreshold = 10f;
    private float powerBarValue = 0f;

    private GameObject jumpButton;

    //BowLogic
    [SerializeField] public float bowPower = 2f;
    [SerializeField] public float deadZone = 25f;
    [SerializeField] private GameObject arrowPrefab;

    private GameObject arrow;

    private GameObject dotsGameObject;
    private List<GameObject> arrowPath;

    public int dotsNumber = 10;

    private Vector2 startPosition;
    private bool shoot, aiming = false;

    void Awake()
    {
        Initialization();
        SetUpInstance();
    }

    void Start()
    {
        dotsGameObject = GameObject.Find("Dots");
        startPosition = transform.position;
        arrowPath = dotsGameObject.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);

        for (int i = 0; i < arrowPath.Count; i++)
        {
            arrowPath[i].GetComponent<Renderer>().enabled = false;
        }

        myAnimator.SetBool("isFalling", true);
    }

    private void Initialization()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        powerBar = GameObject.Find("PowerBar").GetComponent<Slider>();
        jumpButton = GameObject.Find("JumpButton");

        powerBar.minValue = 0f;
        powerBar.maxValue = 10f;
        powerBar.value = powerBarValue;
    }

    void Update()
    {
        //ShootAfterJump(.1f);

        if (ifCanShoot)
        {
            Aim();
            Debug.Log(Input.mousePosition.x);
        }

        if (ifCanJump)
        {
            SetPower();
        }
    }

    void Aim()
    {
        //if (shoot)
        //    return;
        if (Input.GetAxis("Fire1") == 1)
        {
            if (!aiming)
            {
                ShootAfterJump(.1f);

                aiming = true;
                startPosition = Input.mousePosition;

                arrow = Instantiate(
                arrowPrefab,
                new Vector2(transform.position.x + 0.5f, transform.position.y),
                Quaternion.identity) as GameObject;

                //arrow.GetComponent<Rigidbody2D>().isKinematic = true;
                //arrow.GetComponent<Renderer>().enabled = false;

                arrow.transform.Find("ArrowTip").GetComponent<Renderer>().enabled = false;
                arrow.transform.Find("ArrowTale").GetComponent<Renderer>().enabled = false;
                arrow.transform.Find("ArrowTip").GetComponent<Rigidbody2D>().isKinematic = true;
                arrow.transform.Find("ArrowTale").GetComponent<Rigidbody2D>().isKinematic = true;

                //myAnimator.SetTrigger("Bow");
                //myAnimator.SetBool("BowIdle", true);

                CalculatePath();
                ShowPath();
            }

            else
            {
                CalculatePath();
            }
        }


        else if (aiming && !shoot)
        {
            if (inDeadZone(Input.mousePosition) /*|| inRealeseZone(Input.mousePosition)*/)
            {
                aiming = false;
                HidePath();
                return;
            }

            //arrow.GetComponent<Rigidbody2D>().isKinematic = false;
            //arrow.GetComponent<Renderer>().enabled = true;

            shoot = true;
            aiming = false;

            //arrow.GetComponent<Rigidbody2D>().AddForce(GetForce(Input.mousePosition));


            if (Input.mousePosition.x >= 190f)
            {
                arrow.transform.localScale = new Vector2(-1f, 1f);
                transform.localScale = new Vector2(-0.5f, 0.5f);
            }

            arrow.transform.Find("ArrowTip").GetComponent<Renderer>().enabled = true;
            arrow.transform.Find("ArrowTale").GetComponent<Renderer>().enabled = true;
            arrow.transform.Find("ArrowTip").GetComponent<Rigidbody2D>().isKinematic = false;
            arrow.transform.Find("ArrowTale").GetComponent<Rigidbody2D>().isKinematic = false;

            arrow.transform.Find("ArrowTip").GetComponent<Rigidbody2D>().AddForce(GetForce(Input.mousePosition));
            arrow.transform.Find("ArrowTale").GetComponent<Rigidbody2D>().AddForce(GetForce(Input.mousePosition));

            //myAnimator.SetBool("BowIdle", false);
            myAnimator.SetTrigger("BowShot");

            ResumeTime();

            Destroy(arrow, 5f);
            HidePath();
        }
    }

    private bool inDeadZone(Vector2 mouse)
    {
        if (Mathf.Abs(startPosition.x - mouse.x) <= deadZone && Mathf.Abs(startPosition.y - mouse.y) <= deadZone)
        {
            return true;
        }

        else
        {
           return false;
        }
    }

    //private bool inRealeseZone(Vector2 mouse)
    //{
    //    if (mouse.x >= 150f)
    //    {
    //        return true;
    //    }

    //    else
    //    {
    //        return false;
    //    }
    //}

    private void ShowPath()
    {
        for (int i = 0; i < arrowPath.Count; i++)
        {
            arrowPath[i].GetComponent<Renderer>().enabled = true;
        }
    }

    private void HidePath()
    {
        for (int i = 0; i < arrowPath.Count; i++)
        {
            arrowPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    Vector2 GetForce(Vector3 mouse)
    {
        return (new Vector2(startPosition.x, startPosition.y) - new Vector2(mouse.x, mouse.y)) * bowPower;
    }

    void CalculatePath()
    {
        Vector2 vel = GetForce(Input.mousePosition) * Time.fixedDeltaTime / arrow.transform.Find("ArrowTip").GetComponent<Rigidbody2D>().mass;

        for (int i = 0; i < arrowPath.Count; i++)
        {
            arrowPath[i].GetComponent<Renderer>().enabled = true;
            float t = i / 30f;
            Vector3 point = PathPoint(transform.position, vel, t);
            point.z = 1f;
            arrowPath[i].transform.position = point;
        }
    }

    Vector2 PathPoint(Vector2 startP, Vector2 startVel, float t)
    {
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
    }

    private void SetUpInstance()
    {
        if (instance == null)
            instance = this;
    }

    private void SetPower()
    {
        if (setPower)
        {
            jumpPowerX += tresholdX * 1.5f * Time.deltaTime;
            jumpPowerY += tresholdY * 2.5f * Time.deltaTime;

            if (jumpPowerX > jumpPowerXMax)
                jumpPowerX = jumpPowerXMax;

            if (jumpPowerY > jumpPowerYMax)
                jumpPowerY = jumpPowerYMax;

            powerBarValue += powerBarTreshold * Time.deltaTime;
            powerBar.value = powerBarValue;
        }
    }

    public void SetPower(bool setPower)
    {
        this.setPower = setPower;

        if (!setPower)
        {
            Jump();
        }
    }

    private void Jump()
    {
        ifCanShoot = true;

        myRigidbody2D.velocity = new Vector2(jumpPowerX, jumpPowerY);
        myAnimator.SetTrigger("Jump");
        myAnimator.SetBool("isFalling", true);

        didJump = true;

        jumpPowerX = jumpPowerY = 0f;

        powerBarValue = 0f;
        powerBar.value = powerBarValue;
    }

    private void ShootAfterJump( float speed)
    {
        isTimeSlow = true;

        //if (transform.position.y >= -0.7f)
        //{
        Time.timeScale = speed;
        //}
    }

    public void ResumeTime()
    {
        if (isTimeSlow)
        {
            isTimeSlow = false;

            ifCanShoot = false;
            Time.timeScale = 1f;

            myAnimator.SetBool("isFalling", true);
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (didJump)
        {
            didJump = false;
        }

        if (target.tag == "Platform")
        {
            transform.localScale = new Vector2(0.5f, 0.5f);

            ifCanShoot = false;
            shoot = false;
            myAnimator.SetBool("isFalling", false);
            //myAnimator.SetBool("BowIdle", false);
        }

        if (target.tag == "EffectorCollider")
        {
            myAnimator.SetBool("isFalling", false);
        }

        if (target.tag == "DeathCollider")
        {
            if (GameOverManager.instance != null)
            {
                GameOverManager.instance.GameOver();
            }

            ResumeTime();
            Destroy(gameObject, 0.1f);
        }
    }

    void OnTriggerStay2D(Collider2D target)
    {
        if (target.tag == "Platform")
        {
            ifCanJump = true;
            jumpButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Platform")
        {
            ifCanJump = false;
            jumpButton.SetActive(false);
        }
    }
}
