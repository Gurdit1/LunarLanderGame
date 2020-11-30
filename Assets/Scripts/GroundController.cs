using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Transform target;

    float width = 20.48f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the target is too far away
        if (Mathf.Abs(target.position.x - transform.position.x) > (2*width))
        {
            //If player is right of 
            if((target.position.x - transform.position.x) > 0)
            {
                transform.position = new Vector3(transform.position.x + (3 * width), transform.position.y, transform.position.z);
            }
            //If player is left of player
            else if ((target.position.x - transform.position.x) < 0)
            {
                transform.position = new Vector3(transform.position.x - (3 * width), transform.position.y, transform.position.z);
            }
        }
    }
}
