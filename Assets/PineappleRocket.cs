using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineappleRocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        ProccesInput();

    }

    private void ProccesInput()
    {
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        } else
        {
            audioSource.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            //we acces transform component in Unity, wich is available to every game object
            transform.Rotate(Vector3.forward);      // Vector3.forward makes object rotates on z axis        
        } else if (Input.GetKey(KeyCode.D)){
            transform.Rotate(-Vector3.forward);
        }
    }
}
