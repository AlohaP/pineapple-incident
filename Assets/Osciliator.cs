using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Osciliator : MonoBehaviour {
    
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);

    //todo remove from inspector
    [Range(0, 1)] [SerializeField] float movementFactor; //0 for not moved, 1 for fully moved

    Vector3 startingPos;
    [SerializeField] float period = 2f;

    // Use this for initialization
    void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float cycles = Time.time / period;  //grows continually from 0

        const float tau = Mathf.PI * 2; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);  //2Pi is full sin wave so it will vary for -1  and +1
        movementFactor = rawSinWave / 2f + 0.5f; //now we go from 0 to 1

        Vector3 displacement = movementFactor * movementVector;
        transform.position = startingPos + displacement;
	}
}
