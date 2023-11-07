using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHandMove : MonoBehaviour
{
    private Rigidbody rb;
    int speed = 250;
    bool grounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        System.Random rnd = new System.Random();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
       
        
        float moveUp = speed * 50;
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        if (Input.GetKey(KeyCode.Space) && grounded) //ןנûזמד
        {
            rb.AddForce(new Vector3(0f, moveUp, 0f));
        }

        if (Input.GetKey(KeyCode.LeftShift) && grounded) //ןנûזמד
        {
            rb.AddForce(movement * speed * 5);
        }
        //if (Input.GetKey(KeyCode.F) && grounded) //ןנûזמד
        //{
        //    //GetComponent<Rigidbody>().MoveRotation(new Vector3(0, 0, 0));
        //    //rb.AddForce(new Vector3(0f, 0f, 0f));
        //}
        rb.AddForce(movement * speed);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true; 
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}