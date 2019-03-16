using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    public GameObject leftLeg, rightLeg, leftArm, rightArm, head, neck, torso;
    private Vector2 legTarget, rightLegTarget, leftLegTarget, leftArmTarget, rightArmTarget, headTarget, neckTarget, torsoTarget;
    private Rigidbody2D myRigidbody;

    public float animationSpeed;
    public float lerpSpeed;

    private float side = 0.5f;


    /* 
     Reference

     Bottom: -0,75 ; -0,65
     Head: 1,4
     ArmSide: 0,7
     ArmDown: -0,1
     */
    // Start is called before the first frame update
    void Start()
    {


        myRigidbody = GetComponentInParent<Rigidbody2D>();
        //LeftLeg.transform.position = new Vector2(-0.05f, -0.75f);
        //RightLeg.transform.position = new Vector2(0.05f, -0.75f);
        //LeftArm.transform.position = new Vector2(-0.05f, -0.1f);
        //RightArm.transform.position = new Vector2(0.05f, -0.1f);
        //Head.transform.position = new Vector2(0.05f, 1.4f);
        //Neck.transform.position = new Vector2(0.05f, 1.4f);
        legTransform = left ? leftLeg.transform : rightLeg.transform;
        SwitchLegs();
    }

    public float floor;

    float time = 0;
    // Update is called once per frame
    public float rigspeed, torsoMaxY;

    float torsoY, torsoSpeed;
    public float supportX;

    Transform legTransform;
    Transform otherLegTransform;
    void Update()
    {
        time += Time.deltaTime;

        myRigidbody.velocity = new Vector2(rigspeed, 0);


        var speed = (myRigidbody.velocity.x) / animationSpeed;

        //RightLeg.transform.localPosition = new Vector2(Mathf.Sin(time * 5f) / 2f, floor + Mathf.Cos(time * 5f) / 4f);

        //var newX = LeftLeg.transform.localPosition.x + speed;

        //legTarget = new Vector2(((int)time % 2) * 2- 1,0);

        var x = legTarget.x + speed;

        if(otherLegTransform.localPosition.x - torso.transform.position.x > supportX)
        {
            torsoSpeed = 0f;
            torsoY = 0;
        }
        else
        {
            torsoSpeed -= Time.deltaTime;
            torsoSpeed = Mathf.Max(torsoSpeed, -1);
        }

        torsoY += torsoSpeed;

        torsoTarget = new Vector2(0, torsoY * torsoMaxY);

        if (x > offset)
        {
            SwitchLegs();
        }
        else
        {
            legTarget = new Vector2(x, YPosition(x));
        }
        //RightArm.transform.position = new Vector2();



        float lerpTime = Time.deltaTime * lerpSpeed * rigspeed;


        legTransform.localPosition = Vector2.Lerp(legTransform.localPosition, legTarget, lerpTime);

        //if (left)
        //    leftLeg.transform.localPosition = Vector2.Lerp(leftLeg.transform.localPosition, leftArmTarget, lerpTime);
        //else
        //    rightLeg.transform.localPosition = Vector2.Lerp(rightLeg.transform.localPosition, rightArmTarget, lerpTime);


        //rightArm.transform.localPosition = Vector2.Lerp(rightArm.transform.localPosition, rightArmTarget, lerpTime);
        //leftArm.transform.localPosition = Vector2.Lerp(leftArm.transform.localPosition, leftArmTarget, lerpTime);

        //head.transform.localPosition = Vector2.Lerp(head.transform.localPosition, headTarget, lerpTime);
        //neck.transform.localPosition = Vector2.Lerp(neck.transform.localPosition, neckTarget, lerpTime);

        torso.transform.localPosition = Vector2.Lerp(torso.transform.localPosition, torsoTarget, lerpTime);

    }

    bool left = true;
    void SwitchLegs()
    {
        //legTransform.localPosition = new Vector3(legTransform.localPosition.x, floor);
        otherLegTransform = legTransform;
        legTarget = new Vector2(-side,floor);
        legTransform.parent = null;
        left = !left;
        legTransform = left ? leftLeg.transform : rightLeg.transform;
        legTransform.parent = transform;
    }

    public float offset;
    public float yScalar;
    public float xScalar;
    float YPosition(float x)
    {
        return Mathf.Max(Mathf.Sin(Mathf.Sqrt(Mathf.Max(-x + offset, 0) * xScalar)) * yScalar, 0) + floor;
        //return -.5f;
    }
}
