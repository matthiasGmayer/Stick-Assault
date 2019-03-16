using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental;

public class PlayerController : MonoBehaviour
{

    public float speed, shiftSpeed, jumpForce;
    public float maxSpeed, shiftMaxSpeed, sneakMaxSpeed, friction, sneakFriction;

    Ragdoll ragdoll;


    Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponentInParent<Rigidbody2D>();
        ragdoll = GetComponentInParent<Ragdoll>();
    }
    float xAxis;
    public float xSpeed;
    public float sneak = 0;


    bool shift, jump, isSneaking;

    float time;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //var speed = Input.GetKey(KeyCode.LeftShift) ? shiftForce : force;//* Mathf.Exp(-Mathf.Abs(myRigidbody.velocity.x));
        shift = Input.GetKey(KeyCode.LeftShift);
        xAxis = Input.GetAxis("Horizontal");
        //if (Input.GetKey(KeyCode.D))
        //{
        //    //myRigidbody.velocity = new Vector2(speed, 0);
        //    //myRigidbody.AddForce(new Vector2(speed, 0));
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    //myRigidbody.velocity = new Vector2(speed, 0);

        //    //myRigidbody.AddForce(new Vector2(-speed, 0));
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (!ragdoll.inAnimation && !ragdoll.ragdoll)
        {
            if (myRigidbody.velocity.x > 0.1f)
                Flip(false);
            else if (myRigidbody.velocity.x < -0.1f)
                Flip(true);
        }
        isSneaking = Input.GetKey(KeyCode.LeftControl);



    }

    //(vel + speed) * frict = vel
    // => speed = vel / frict - vel

    public float lerpTime;
    void FixedUpdate()
    {
        sneak = Mathf.Lerp(sneak, isSneaking ? .7f : 0, lerpTime);

        var targetSpeed = (isSneaking ? sneakMaxSpeed : shift ? shiftMaxSpeed : maxSpeed) * (1 / (isSneaking ? sneakFriction : friction) - 1);
        speed = Mathf.Lerp(speed, targetSpeed * Mathf.Sign(xAxis), lerpTime);

        xSpeed = myRigidbody.velocity.x + Mathf.Abs(xAxis) * speed;
        xSpeed = xSpeed * (isSneaking ? sneakFriction : friction);

        //if (xAxis == 0 && isSneaking) xSpeed = 0;

        shiftSpeed = myRigidbody.velocity.x;

        var max = shift ? shiftMaxSpeed : maxSpeed;
        //if (xSpeed > max) xSpeed = max;
        //else if (xSpeed < -max) xSpeed = -max;
        myRigidbody.velocity = new Vector2(xSpeed, myRigidbody.velocity.y);

        if (jump)
        {
            myRigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }



    public void Flip(bool left)
    {
        transform.parent.localScale = new Vector3(left ? -1 : 1, 1, 1);
    }
}
