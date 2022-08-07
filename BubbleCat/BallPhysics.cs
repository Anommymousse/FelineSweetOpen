using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public Rigidbody rb_ball;
    // Start is called before the first frame update
    void Start()
    {
    }

    void PlayerInput()
    {
        var akey = Input.GetKey("a");
        var skey = Input.GetKey("s");
        var dkey = Input.GetKey("d");
        var wkey = Input.GetKey("w");
        var jumpkey =  Input.GetKey("space");
        if (akey)
            rb_ball.AddForce(-20f,0,0);
        if (wkey)
            rb_ball.AddForce(0,0,2.5f);
        if (dkey)
            rb_ball.AddForce(20f,0,0);
        if (skey)
            rb_ball.AddForce(0,0,-2.5f);
        if(jumpkey)
            rb_ball.AddForce(0,-100f,0);
    }
    
    void FixedUpdate()
    {
        if (Time.timeScale > 0.01f)
        {
            rb_ball.AddForce(0, 0, 10.0f);
            rb_ball.useGravity = true;
            PlayerInput();
        }
        else
        {
            rb_ball.useGravity = false;
            rb_ball.velocity = new Vector3(0, 0, 0);
        }
    }
    
}
