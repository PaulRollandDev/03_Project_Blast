using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool mySound;
    bool myToggle;
    [SerializeField] float rcsTrust = 100f;
    [SerializeField] float mainTrust = 100f;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        Rotate();
        Trusting();
    }

    private void Trusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainTrust);
            mySound = true;
        }
        else
        {
            mySound = false;
        }
        if (mySound == true && myToggle == false)
        {
            audioSource.Play();
            myToggle = true;
        }
        if (mySound == false && myToggle == true)
        {
            audioSource.Stop();
            myToggle = false;
        }
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true;  // take manual control of rotation

        
        float rotationThisFrame = rcsTrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics control
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Fuel":
                print("Fuel");
                break;
            default:
                print("Dead");
                break;
        }
    }

}
