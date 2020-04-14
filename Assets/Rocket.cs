using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    private int currentScene = 0;
    private Rigidbody rb;
    private AudioSource audio;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem winParticles;

    private enum State { Alive, Dying, Transcending };

    private State state = State.Alive;

    [SerializeField]
    private float _speedOfRotation = 125f;

    [SerializeField]
    private float _mainTurstSpeed = 25f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotationInput();
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
        rb.freezeRotation = true; // Take manual Control of rotation

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * Time.deltaTime * _speedOfRotation);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * Time.deltaTime * _speedOfRotation);
        }

        rb.freezeRotation = false; // Allow physics control of rotation
    }

    private void StartSuccessSequence() {
        state = State.Transcending;
        audio.Stop();
        audio.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextScene", 1f);
    }

    private void StartLossSequence() {
        state = State.Dying;
        audio.Stop();
        audio.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadNextScene", 1f);
    }

    private void OnCollisionEnter(Collision collision) {
        if (state != State.Alive) {
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
            currentScene++;
        } else if (state == State.Dying) {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);
    }
}