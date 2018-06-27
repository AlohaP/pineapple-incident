using UnityEngine.SceneManagement;
using UnityEngine;

public class PineappleRocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;  // rcs - reaction control system
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine; 
    [SerializeField] AudioClip deathEngine; 
    [SerializeField] AudioClip winEngine; 

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        //todo stop sound while dead
        if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive){ return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(winEngine);
                Invoke("LoadNextScene", 1f);  //parametirize time
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(deathEngine);
                Invoke("LoadFirstScene", 2f);
                break;
        }
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
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
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
