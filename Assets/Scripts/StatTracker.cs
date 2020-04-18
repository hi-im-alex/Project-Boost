using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTracker : MonoBehaviour {
    public static StatTracker Instance;
    private int deathCount;
    [SerializeField] private Text deathCountText;

    private float secondsCount;
    private int minuteCount;
    private int hourCount;
    [SerializeField] private Text timerCount;

    public bool FinishedGame = true;

    // Initialize the singleton instance.
    private void Awake() {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null) {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this) {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseDeathCount() {
        deathCount++;
        deathCountText.text = "Deaths: " + deathCount;
    }

    public void UpdateTimerUi() {
        secondsCount += Time.deltaTime;
        if (secondsCount >= 60) {
            secondsCount = 0;
            minuteCount++;
        }
        if (minuteCount >= 60) {
            minuteCount = 0;
            hourCount++;
        }

        timerCount.text = hourCount.ToString("00") + ":" + minuteCount.ToString("00") + ":" + ((int)secondsCount).ToString("00");
    }

    public string ReturnCount() {
        return timerCount.text.ToString();
    }

    public void StartGame() {
        deathCount = 0;
        secondsCount = 0;
        minuteCount = 0;
        hourCount = 0;
        deathCountText.text = "Deaths: " + deathCount;
        FinishedGame = false;
    }

    private void Update() {
        if (!FinishedGame) {
            UpdateTimerUi();
        }
    }
}