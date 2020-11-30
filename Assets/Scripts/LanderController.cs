using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanderController : MonoBehaviour
{
    public Text textAlt;
    public Text textSpeed;
    public Text textPitch;
    public GameObject exhaust;
    public Transform leftUp;
    public Transform leftDown;
    public Transform leftLeft;
    public Transform rightUp;
    public Transform rightDown;
    public Transform rightRight;
    public Text endMessage;

    public float engineForce;
    public float thrusterForce;

    SpriteRenderer exhaustSprite;
    Rigidbody2D rb;

    float speed;
    float pitch;

    Vector3 thrustCalc(Vector3 input)
    {
        float pi = 3.1415926535897932384f;
        float angle = transform.localRotation.eulerAngles.z; //Gets user rotation in degrees

        //Finds angle of rocket thruster after lander has rotated
        if(input.x != 0)
        {
            if (input.x > 0)
            {
                angle -= 90;
            }
            else if (input.x < 0)
            {
                angle += 90;
            }
        }
        else if (input.y != 0)
        {
            if (input.y > 0)
            {
                angle += 0;
            }
            else if (input.y < 0)
            {
                angle += 180;
            }
        }

        if (angle > 360)
        {
            angle -= 360;
        }
        else if (angle < 0)
        {
            angle += 360;
        }


        angle = (angle * pi) / 180; //Converts rotation to radians

        //Calculats x and y components of thrust
        float x = Mathf.Sin(angle) * Mathf.Abs(thrusterForce);
        float y = Mathf.Cos(angle) * Mathf.Abs(thrusterForce);
        
        return new Vector3(x, y, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        exhaustSprite = exhaust.GetComponent<SpriteRenderer>();
        exhaustSprite.enabled = false;
        textPitch.text = "";
        endMessage.text = "";

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Custom gravity force: force of gravity = k/r^2 (k = some constant, r = distance from ground)
        float k = 80;
        float r = transform.position.y;
        rb.gravityScale = k / (r * r);


        //Main engine
        if (Input.GetKey(KeyCode.Space)&&(Time.timeScale!=0))
        {
            exhaustSprite.enabled = true;
            rb.AddForce(transform.up * engineForce);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            exhaustSprite.enabled = false;
        }

        //Notes:    Change thrusterCalc() function
        
        //Thruster code
        Vector3 thrustInput;
        Vector3 thrustOutput;

        //Left left thruster
        if (Input.GetKey(KeyCode.D))
        {
            //rb.AddForceAtPosition(transform.TransformPoint(new Vector3(thrusterForce, 0, 0)), leftLeft.position, ForceMode2D.Force);
            thrustInput = new Vector3(thrusterForce, 0f, 0f);
            thrustOutput = thrustCalc(thrustInput);
            rb.AddForceAtPosition(thrustOutput, leftLeft.position, ForceMode2D.Force);
            Debug.Log("D");
        }

        //Left Up thruster
        if (Input.GetKey(KeyCode.W))
        {
            //rb.AddForceAtPosition(transform.TransformPoint(new Vector3(0, thrusterForce, 0)), leftUp.position, ForceMode2D.Force);
            thrustInput = new Vector3(0f, thrusterForce, 0f);
            thrustOutput = thrustCalc(thrustInput);
            rb.AddForceAtPosition(thrustOutput, leftUp.position, ForceMode2D.Force);
            Debug.Log("W");
        }

        //Left Down thruster
        if (Input.GetKey(KeyCode.S))
        {
            //rb.AddForceAtPosition(transform.TransformPoint(new Vector3(0, (-1f * thrusterForce), 0)), leftDown.position, ForceMode2D.Force);
            thrustInput = new Vector3(0, (-1 * thrusterForce), 0);
            thrustOutput = thrustCalc(thrustInput);
            rb.AddForceAtPosition(thrustOutput, leftDown.position, ForceMode2D.Force);
            Debug.Log("S");
        }


        // Right right thruster
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //rb.AddForceAtPosition(transform.TransformPoint(new Vector3(-1 * thrusterForce, 0, 0)), rightRight.position, ForceMode2D.Force);
            thrustInput = new Vector3((-1 * thrusterForce), 0, 0);
            thrustOutput = thrustCalc(thrustInput);
            rb.AddForceAtPosition(thrustOutput, rightRight.position, ForceMode2D.Force);
            Debug.Log("Left");
        }

        //Right Up thruster
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //rb.AddForceAtPosition(transform.TransformPoint(new Vector3(0, thrusterForce, 0)), rightUp.position, ForceMode2D.Force);
            thrustInput = new Vector3(0, thrusterForce, 0);
            thrustOutput = thrustCalc(thrustInput);
            rb.AddForceAtPosition(thrustOutput, rightUp.position, ForceMode2D.Force);
            Debug.Log("Right");
        }
        
        //Right down thruster
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //rb.AddForceAtPosition(transform.TransformPoint(new Vector3(0, (-1f * thrusterForce), 0)), rightDown.position, ForceMode2D.Force);
            thrustInput = new Vector3(0, (-1 * thrusterForce), 0);
            thrustOutput = thrustCalc(thrustInput);
            rb.AddForceAtPosition(thrustOutput, rightDown.position, ForceMode2D.Force);
            Debug.Log("Down");
        }

        //Restart level
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }

        //Exit gamer
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        //Update altitude
        textAlt.text = "Altitude: " + string.Format("{0:0.00}", (transform.position.y-11.79)) + "m";
        if(transform.position.y - 11.79 >= 20)
        {
            textAlt.color = Color.white;
        }
        else if ((transform.position.y - 11.79 < 20) && (transform.position.y - 11.79 >= 10))
        {
            textAlt.color = Color.yellow;
        }
        else
        {
            textAlt.color = Color.red;
        }

        //Update speed
        speed = rb.velocity.y;
        if(speed > 0)
        {
            textSpeed.color = Color.red;
            textSpeed.text = "Ascending: " + string.Format("{0:0.00}", speed) + "m/s";
        }
        else
        {
            if (speed <= -5)
            {
                textSpeed.color = Color.red;
            }
            else if ((speed > -5)&&(speed < -2))
            {
                textSpeed.color = Color.yellow;
            }
            else if(speed <= 0)
            {
                textSpeed.color = Color.white;
            }
            textSpeed.text = "Descending: " + string.Format("{0:0.00}", (speed*-1)) + "m/s";
        }

        //Update pitch
        pitch = Mathf.Abs(transform.localRotation.eulerAngles.z);

        if ((pitch > 10)&&(pitch < 350))
        {
            textPitch.color = Color.red;
        }
        else if (((pitch>5)&&(pitch<10))||((pitch>350)&&(pitch<355)))
        {
            textPitch.color = Color.yellow;
        }
        else if ((pitch <= 5)||(pitch>=355))
        {
            textPitch.color = Color.white;
        }
        textPitch.text = "Pitch: " + string.Format("{0:0.00}", Mathf.Abs(transform.localRotation.eulerAngles.z)) + " deg";

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //TODO: Freeze game and press enter t go to main screen

        endMessage.color = Color.red;
        
        //If other is ground then it checks rotation and speed values to determine crash or succesful landing
        if(other.gameObject.layer == 8)
        {
            if ((speed > -5)&& ((pitch <= 10) || (pitch >= 350)))
            {
                endMessage.color = Color.green;
                endMessage.text = "SUCCESSFUL LANDING!!!";
            }
            else
            {
                endMessage.text = "CRASH!!!";
            }
            Time.timeScale = 0;            
        }

        //If other is not ground then it must be the CSM and crash is determined
        else
        {
            endMessage.text = "CRASH!!!";
            Time.timeScale = 0;            
        }

        if(exhaustSprite.enabled == true)
        {
            exhaustSprite.enabled = false;
        }
    }
}
