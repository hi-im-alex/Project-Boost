using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    private int currentScene;
    private Rigidbody rb;
    private AudioSource audio;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem winParticles;

    private StatTracker st;

    private enum State { Alive, Dying, Transcending };

    private State state = State.Alive;

    [SerializeField]
    private float _speedOfRotation = 125f;

    [SerializeField]
    private float _mainTurstSpeed = 25f;

    [SerializeField]
    private float _levelLoadDelay = 1f;

    private bool ignoreCollision = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        st = GameObject.Find("StatTracker").GetComponent<StatTracker>();
    }

    private void Update() {
        if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotationInput();
        }

        if (Debug.isDebugBuild) {
            RespondToDebugKeys();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }

    private void RespondToDebugKeys() {
        if (Input.GetKeyDown(KeyCode.C)) {
            ignoreCollision = !ignoreCollision;
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            state = State.Transcending;
            LoadNextScene();
        }
    }

    private void RespondToThrustInput() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        } else {
            audio.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust() {
        rb.AddRelativeForce(Vector3.up * _mainTurstSpeed);
        if (audio.isPlaying == false) {
            audio.PlayOneShot(mainEngine);
        }
        if (mainEngineParticles.isPlaying == false) {
            mainEngineParticles.Play();
        }
    }

    private void RespondToRotationInput() {
        rb.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * Time.deltaTime * _speedOfRotation);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * Time.deltaTime * _speedOfRotation);
        }
    }

    private void StartSuccessSequence() {
        state = State.Transcending;
        audio.Stop();
        audio.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextScene", _levelLoadDelay);
    }

    private void StartLossSequence() {
        state = State.Dying;
        audio.Stop();
        audio.PlayOneShot(deathSound);
        deathParticles.Play();
        st.IncreaseDeathCount();
        Invoke("LoadNextScene", _levelLoadDelay);
    }

    private void OnCollisionEnter(Collision collision) {
        if (state != State.Alive || ignoreCollision) {
            return;
        }

        switch (collision.gameObject.tag) {
            case "Friendly":
                // Do nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartLossSequence();
                break;
        }
    }

    private void LoadNextScene() {
        if (state == State.Transcending) {
            if (currentScene == SceneManager.sceneCountInBuildSettings - 2) {
                st.FinishedGame = true;
                currentScene++;
            } else {
                currentScene++;
            }
        }
        SceneManager.LoadScene(currentScene);
    }
}