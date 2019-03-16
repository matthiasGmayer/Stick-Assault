using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float lerpTime;
    public Vector2 target;
    // Update is called once per frame
    void FixedUpdate()
    {
        target = Vector2.Lerp(target, player.transform.position, Time.deltaTime * lerpTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z), Time.deltaTime * lerpTime);
    }
}
