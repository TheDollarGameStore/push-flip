using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsLoader : MonoBehaviour
{
    [SerializeField] private Text highscoreText;

    private void Start()
    {
        highscoreText = GetComponent<Text>();
        highscoreText.text = PlayerPrefs.GetString("Highscore", "0");
    }
}
