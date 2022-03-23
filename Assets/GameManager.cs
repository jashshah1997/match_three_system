using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EasySettings
{
};

public class MediumSettings
{
};

public class HardSettings
{
};

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float GameTime = 30f;

    private float m_current_time;
    private bool m_reinitialize = false;
    private bool m_game_over = false;

    private GameObject m_game;
    private GameObject m_difficulty_dropdown;
    private GameObject m_skill_level;
    private GameObject m_start_game_button;
    private GameObject m_back_button;
    private GameObject m_time_left;
    private GameObject m_end_game_label;

    // Start is called before the first frame update
    void Start()
    {
        m_game = GameObject.Find("Game");
        m_difficulty_dropdown = GameObject.Find("DifficultyDropdown");
        m_skill_level = GameObject.Find("SkillLevel");
        m_start_game_button = GameObject.Find("StartGameButton");
        m_back_button = GameObject.Find("BackButton");
        m_time_left = GameObject.Find("TimeLeft");
        m_end_game_label = GameObject.Find("EndGameLabel");

        m_game.SetActive(false);
        m_back_button.SetActive(false);
        m_time_left.SetActive(false);
        m_end_game_label.SetActive(false);

        m_start_game_button.GetComponent<Button>().onClick.AddListener(OnStartGameClicked);
        m_back_button.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_skill_level.activeSelf)
        {
            int val = 20;
            try
            {
                val = Int32.Parse(m_skill_level.GetComponent<TMP_InputField>().text);
            } catch (Exception)
            {
                m_skill_level.GetComponent<TMP_InputField>().text = val + "";
            }
            
            if (val < 0)
            {
                m_skill_level.GetComponent<TMP_InputField>().text = "0";
            } 
            else if (val > 100)
            {
                m_skill_level.GetComponent<TMP_InputField>().text = "100";
            }
        }

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
    }

    public void SetEndGame(string label)
    {
        if (m_end_game_label.activeSelf) return;

        m_game_over = true;
        m_reinitialize = true;
        m_end_game_label.SetActive(true);
        m_end_game_label.GetComponent<TextMeshProUGUI>().text = label;

        // TODO: Pause game
    }

    void OnStartGameClicked()
    {
        int diff = m_difficulty_dropdown.GetComponent<Dropdown>().value;
        float lockpickBreakingSpeed = -1f;
        float lockThreshold = -1f;
        switch (diff)
        {
            case 0:
                // TODO: Set easy parameters
                break;
            case 1:
                // TODO: Set medium parameters
                break;
            case 2:
                // TODO: Set hard parameters
                break;
        }

        // TODO: Adjust for skill level
        // TODO: Set parameteres in game

        m_game_over = false;
        ToggleUI(true);

        // TODO: Reset the game

        if (m_reinitialize)
        {
            m_reinitialize = false;
            // TODO: Initialize the game, e.g. random parameters
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

        if (toggle)
        {
            m_current_time = GameTime;
            m_time_left.GetComponent<TextMeshProUGUI>().text = m_current_time + "";
        }

        m_difficulty_dropdown.SetActive(!toggle);
        m_skill_level.SetActive(!toggle);
        m_start_game_button.SetActive(!toggle);
    }
}