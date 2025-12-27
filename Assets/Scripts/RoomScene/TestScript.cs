using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class TestScript : MonoBehaviour
{

    public LeapProvider mProvider;

    //Attribute
    private Rigidbody rb;

    //Value
    public float jumpForce = 2f; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }


    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
    }
    public void Down()
    {
        rb.velocity = Vector3.zero;
    }


    /*void Update()
    {
        Frame frame = provider.CurrentFrame;

        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {

                transform.position = hand.PalmPosition.ToVector3() +

                                     hand.PalmNormal.ToVector3() *

                                    (transform.localScale.y * .5f + .02f);

                transform.rotation = hand.Basis.CalculateRotation();
            }
        }

    }*/
}
