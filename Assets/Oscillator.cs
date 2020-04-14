using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {
    [SerializeField] private Vector3 movementVector = new Vector3(10f, 0, 0);
    [SerializeField] private float period = 2f;

    private float movementFactor;
    private Vector3 startingPos;

    private void Start() {
        startingPos = transform.position;
    }

    private void Update() {
        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}