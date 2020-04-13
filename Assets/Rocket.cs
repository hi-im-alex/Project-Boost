using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    private Rigidbody rb;
    private AudioSource audio;

    [SerializeField]
    private float _speedOfRotation = 125f;

    [SerializeField]
    private float _mainTurstSpeed = 25f;

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update() {
        Thrust();
        Rotate();
    }

    private void Thrust() {
        if (Input.GetKey(KeyCode.Space)) {
            rb.AddRelativeForce(Vector3.up * _mainTurstSpeed);
            if (audio.isPlaying == false) {
                audio.Play();
            }
        } else {
            audio.Stop();
        }
    }

    private void Rotate() {
        rb.freezeRotation = true; // Take manual Control of rotation

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * Time.deltaTime * _speedOfRotation);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * Time.deltaTime * _speedOfRotation);
        }

        rb.freezeRotation = false; // Allow physics control of rotation
    }

    private void OnCollisionEnter(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Friendly":
                print("Collided friendly");
                break;

            default:
                print("dead");
                break;
        }
    }
}