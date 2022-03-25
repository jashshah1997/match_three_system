using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EasySettings
{
    public static readonly int MatchCount = 3;
};

public class MediumSettings
{
    public static readonly int MatchCount = 4;
};

public class HardSettings
{
    public static readonly int MatchCount = 5;
};

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float GameTime = 60f;
    public int Score = 0;

    private float m_current_time;
    private bool m_reinitialize = false;
    private bool m_game_over = false;

    private GameObject m_game;
    private GameObject m_difficulty_dropdown;
    private GameObject m_start_game_button;
    private GameObject m_back_button;
    private GameObject m_time_left;
    private GameObject m_score;
    private GameObject m_end_game_label;

    // Start is called before the first frame update
    void Start()
    {
        m_game = GameObject.Find("Game");
        m_difficulty_dropdown = GameObject.Find("DifficultyDropdown");
        m_start_game_button = GameObject.Find("StartGameButton");
        m_back_button = GameObject.Find("BackButton");
        m_time_left = GameObject.Find("TimeLeft");
        m_score = GameObject.Find("Score");
        m_end_game_label = GameObject.Find("EndGameLabel");

        m_game.SetActive(false);
        m_back_button.SetActive(false);
        m_time_left.SetActive(false);
        m_score.SetActive(false);
        m_end_game_label.SetActive(false);

        m_start_game_button.GetComponent<Button>().onClick.AddListener(OnStartGameClicked);
        m_back_button.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_time_left.activeSelf)
        {
            if (!m_game_over)
                m_current_time -= Time.deltaTime;

            m_time_left.GetComponent<TextMeshProUGUI>().text = (int)m_current_time + "";

            if (m_current_time < 0)
            {
                SetEndGame("Time is Up!");
            }
        }

        m_score.GetComponent<TextMeshProUGUI>().text = Score + "";
        if (Score >= 800)
        {
            SetEndGame("You Won!");
        }
    }

    public void SetEndGame(string label)
    {
        if (m_end_game_label.activeSelf) return;

        m_game_over = true;
        m_reinitialize = true;
        m_end_game_label.SetActive(true);
        m_end_game_label.GetComponent<TextMeshProUGUI>().text = label;

        m_game.GetComponentInChildren<BoardController>().Paused();
    }

    void OnStartGameClicked()
    {
        int diff = m_difficulty_dropdown.GetComponent<Dropdown>().value;
        int matchCount = 3;
        switch (diff)
        {
            case 0:
                matchCount = EasySettings.MatchCount;
                break;
            case 1:
                matchCount = MediumSettings.MatchCount;
                break;
            case 2:
                matchCount = HardSettings.MatchCount;
                break;
        }

        m_game_over = false;
        ToggleUI(true);

        m_game.GetComponentInChildren<BoardController>().SetMatchCount(matchCount);
        m_game.GetComponentInChildren<BoardController>().Initialize();
        Score = 0;

        if (m_reinitialize)
        {
            m_reinitialize = false;
        }
    }

    void OnBackButtonClicked()
    {
        ToggleUI(false);
    }

    private void ToggleUI(bool toggle)
    {
        m_end_game_label.SetActive(false);

        m_game.SetActive(toggle);
        m_back_button.SetActive(toggle);
        m_time_left.SetActive(toggle);
        m_score.SetActive(toggle);

        if (toggle)
        {
            m_current_time = GameTime;
            m_time_left.GetComponent<TextMeshProUGUI>().text = m_current_time + "";
        }

        m_difficulty_dropdown.SetActive(!toggle);
        m_start_game_button.SetActive(!toggle);
    }
}
