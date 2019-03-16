using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ragdoll : MonoBehaviour
{
    public bool ragdoll;
    private List<Rigidbody2D> ragdolls;
    private Rigidbody2D myRigidbody;
    public GameObject bone;
    private Rigidbody2D boneRigidbody;
    public PhysicsMaterial2D material;
    public GameObject toDisable;
    private Animator animator;
    private PlayerController playerController;

    public GameObject leftLeg, rightLeg, leftArm, rightArm, head, neck;
    public GameObject leftLegTarget, rightLegTarget, leftArmTarget, rightArmTarget, headTarget, neckTarget;

    void Awake()
    {
        playerController = GetComponentInChildren<PlayerController>();
        ragdolls = GetComponentsInChildren<Rigidbody2D>().ToList();
        myRigidbody = GetComponent<Rigidbody2D>();
        boneRigidbody = bone.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        var l = GetComponentsInChildren<Collider2D>().ToList();
        foreach(var a in l)
        {
            foreach(var b in l)
            {
                //if(!a.gameObject.name.Contains("Head") && !b.gameObject.name.Contains("Head"))
                    Physics2D.IgnoreCollision(a, b);
            }
        }

        animator.GetBehaviour<ResetPosition>().r = this;
    }


    void Initialize()
    {
        foreach (var at in bone.GetComponentsInChildren<Transform>()){
            {
                GameObject g = at.gameObject;
                if (at.name.Contains("Target")) continue;
                if (!at.name.Contains("Torso_1"))
                {
                    var a = g.GetComponent<HingeJoint2D>();
                    if (a == null) a = g.AddComponent<HingeJoint2D>();
                    a.autoConfigureConnectedAnchor = true;
                    a.autoConfigureConnectedAnchor = false;
                    a.connectedBody = a.transform.parent.GetComponent<Rigidbody2D>();
                    a.useLimits = true;
                    if ((g.name.Contains("Torso") || g.name.Contains("Head")) && !g.name.Contains("Torso_1"))
                    {
                        var offset = 0;
                        var j = new JointAngleLimits2D
                        {
                            max = 30 + offset,
                            min = -30 + offset
                        };
                        a.limits = j;
                    }
                }

                var r = g.GetComponent<Rigidbody2D>();
                if (r == null) r = g.AddComponent<Rigidbody2D>();
                r.useAutoMass = true;
                r.sharedMaterial = material;

                if (g.name.Contains("Head"))
                {
                    var c = g.GetComponent<CircleCollider2D>();
                    if (c == null) c = g.AddComponent<CircleCollider2D>();
                    c.offset = new Vector2(0.31f, 0);
                    c.radius = 0.31f;
                }
                else
                {
                    var c = g.GetComponent<BoxCollider2D>();
                    if (c == null) c = g.AddComponent<BoxCollider2D>();
                    if (g.name.Contains("Torso"))
                    {
                        c.offset = new Vector2(0.075f, 0);
                        c.size = new Vector2(0.2f, 0.1f);
                    }
                    else
                    {
                        c.offset = new Vector2(0.15f, 0);
                        c.size = new Vector2(0.4f, 0.1f);
                    }
                }

                //var c = g.GetComponent<EdgeCollider2D>();
                //if (c == null) g.AddComponent<EdgeCollider2D>();
            }
        }
    }

    void OnValidate()
    {
        animator = GetComponent<Animator>();

        Initialize();

        ragdolls = bone.GetComponentsInChildren<Rigidbody2D>().ToList();
        myRigidbody = GetComponent<Rigidbody2D>();
        boneRigidbody = bone.GetComponent<Rigidbody2D>();

        if(Application.isPlaying)
            EnableRagdoll(ragdoll);
    }

    private readonly Vector2 legReset = new Vector2(-1,0), headReset = new Vector2(1.5f,0), armReset = new Vector2(1.5f,0);

    bool ragdollOff;

    public void EnableRagdoll(bool r)
    {
        if (r == ragdoll) return;

        if (r)
        {
            ragdoll = true;
            ragdollOff = false;
            ragdolls.ForEach(a => a.velocity = myRigidbody.velocity);

            myRigidbody.simulated = false;
            animator.enabled = false;

            if(transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                FlipTransform(rightLeg, leftLeg, rightArm, leftArm, neck, head);
            }

        }
        else {
            ragdollOff = true;
            toDisable.SetActive(true);
            Invoke("Finish", 0.5f);

            //bone.transform.localEulerAngles = new Vector3(0,0,0);
            transform.position = bone.transform.position;
            bone.transform.position = transform.position;
            var euler = (bone.transform.localEulerAngles.z % 360) - 360;
            left = euler < -90 && euler > -270;


            rightLeg.transform.position = rightLegTarget.transform.position;
            leftLeg.transform.position = leftLegTarget.transform.position;
            rightArm.transform.position = rightArmTarget.transform.position;
            leftArm.transform.position = leftArmTarget.transform.position;
            neck.transform.position = neckTarget.transform.position;
            head.transform.position = headTarget.transform.position;
        }
        ragdolls.ForEach(a => a.simulated = r);
        //toDisable.SetActive(!r);
    }

    void FlipTransform(GameObject o) => o.transform.localPosition = new Vector3(o.transform.localPosition.x, o.transform.localPosition.y * -1, o.transform.localPosition.z);
    void FlipTransform(params GameObject[] os) { foreach (GameObject o in os) FlipTransform(o); }
    

    bool left;

    void Update()
    {
        if (ragdollOff)
        {
            var lerpTime = Time.deltaTime * 5;
            bone.transform.rotation = Quaternion.Lerp(bone.transform.rotation, Quaternion.Euler(0,0,left?180:0), lerpTime * 2);

            int factor = left ? -1 : 1;
            animator.enabled = false;

            rightLeg.transform.localPosition = Vector2.Lerp(rightLeg.transform.localPosition,  legReset, lerpTime);
            leftLeg.transform.localPosition = Vector2.Lerp(leftLeg.transform.localPosition,  legReset, lerpTime);
            rightArm.transform.localPosition = Vector2.Lerp(rightArm.transform.localPosition,  armReset, lerpTime);
            leftArm.transform.localPosition = Vector2.Lerp(leftArm.transform.localPosition, armReset, lerpTime);
            neck.transform.localPosition = Vector2.Lerp(neck.transform.localPosition, armReset, lerpTime);
            head.transform.localPosition = Vector2.Lerp(head.transform.localPosition, headReset, lerpTime);
        }
        if (disable)
        {
            disable = false;
            toDisable.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnableRagdoll(!ragdoll);
            if (ragdoll && !ragdollOff)
            {
                disable = true;
            }

        }
    }
    bool disable;

    void Finish()
    {
        if (!ragdollOff) return;
        ragdoll = false;
        inAnimation = true;
        ragdollOff = false;
        toDisable.SetActive(true);
        animator.enabled = true;
        animator.SetTrigger("Standup");
        playerController.Flip(!left);
        ragdoll = false;
    }

    public bool inAnimation;

    public void OnAnimationFinished()
    {
        inAnimation = false;
        myRigidbody.simulated = true;
        transform.position = bone.transform.position;
        bone.transform.position = transform.position;
    }

}
