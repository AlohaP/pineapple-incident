using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PineappleRocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;  // rcs - reaction control system
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine; 
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip succes;

    [SerializeField] ParticleSystem mainEngineParticles; 
    [SerializeField] ParticleSystem deathParticles; 
    [SerializeField] ParticleSystem succesParticles; 

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    bool collisionsDisabled = true;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionsDisabled){ return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        succesParticles.Play();
        Invoke("LoadFirstScene", levelLoadDelay);
    }

    private void StartSuccesSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(succes);
        deathParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);  //parametirize time
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust *Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; //take manual control of rotation
        if (Input.GetKey(KeyCode.A))
        {
            //we acces transform component in Unity, wich is available to every game object
            transform.Rotate(Vector3.forward * rotationThisFrame);      // Vector3.forward makes object rotates on z axis        
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }
}
