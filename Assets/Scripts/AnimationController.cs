using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D myRigidbody;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInChildren<PlayerController>();
        myRigidbody = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    public float speedxmult, animatorspeedmult, maxanimspeed;
    // Update is called once per frame
    void Update()
    {
        float speed = Mathf.Abs(myRigidbody.velocity.x);
        animator.SetFloat("SpeedX",speed * speedxmult);
        animator.SetFloat("Sneak", playerController.sneak);
        //animator.speed = Mathf.Sqrt(Mathf.Abs(myRigidbody.velocity.x));   
        animator.speed = Mathf.Max(1f, Mathf.Min(speed * animatorspeedmult, maxanimspeed));
    }
}
