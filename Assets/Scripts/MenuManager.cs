using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    private StatTracker st;
    public bool GameEndScreen = false;

    public void Awake() {
        st = GameObject.Find("StatTracker").GetComponent<StatTracker>();

        if (GameEndScreen) {
            Text timeScore = GameObject.Find("TimeScore").GetComponent<Text>();
            timeScore.text = st.ReturnCount();
        }
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
        st.StartGame();
    }

    public void ExitGame() {
        Application.Quit();
    }
}