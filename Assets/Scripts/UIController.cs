﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    /// <summary>
    /// Upgrade button with information about 
    /// amount of honey to upgrade
    /// </summary>
    public Button Upgrade;

    /// <summary>
    /// Button for casting offensive spell
    /// </summary>
    public Button SpellOffensive;

    /// <summary>
    /// Button for casting earthquake
    /// </summary>
    public Button SpellEarthquake;

    /// <summary>
    /// Button for casting takeover spell
    /// </summary>
    public Button SpellTakeover;

    /// <summary>
    /// Label with information about current time scale
    /// </summary>
    public TextMeshProUGUI SpeedText;

    /// <summary>
    /// Label with information about current part
    /// of army that will be send between vertices
    /// </summary>
    public TextMeshProUGUI TransportPartText;

    /// <summary>
    /// Label with amount of honey
    /// required for upgrade
    /// </summary>
    public TextMeshProUGUI UpgradeCostText;

    /// <summary>
    /// Label with info about amount of player honey
    /// </summary>
    public TextMeshProUGUI HoneyText;

    /// <summary>
    /// Label with info about player mana
    /// </summary>
    public TextMeshProUGUI ManaText;

    /// <summary>
    /// Image object used as overlay on the screen
    /// when player spell is choosen
    /// </summary>
    public Image CastingSpellVignette;

    /// <summary>
    /// Reference to gameplay controller in Mechanism object
    /// </summary>
    GameplayController _gameplayController;

    /// <summary>
    /// Determine if menu should be visible
    /// </summary>
    public bool IsShowingPauseMenu = false;

    /// <summary>
    /// Canvas object with pause menu
    /// </summary>
    public GameObject PauseMenu;

    /// <summary>
    /// Object with win/lose canvas
    /// </summary>
    public GameObject WinLoseCanvas;

    /// <summary>
    /// Object with win text
    /// </summary>
    public GameObject Win;

    /// <summary>
    /// Object with fail text
    /// </summary>
    public GameObject Fail;

    private void Start()
    {
        if (_gameplayController == null)
        {
            _gameplayController = GameObject.FindWithTag("Mechanism").GetComponent<GameplayController>();
        }
    }

    /// <summary>
    /// Set text value of labels on the screen
    /// </summary>
    void SetPlayerUI()
    {
        HoneyText.text = _gameplayController.Honey[1].ToString();
        ManaText.text = _gameplayController.Mana[1].ToString();
        TransportPartText.text = $"{_gameplayController.TransportPart * 100}%";
        SpeedText.text = $"x{_gameplayController.GameplaySpeedMultiplier}";

        if (_gameplayController.Mana[1] >= 100)
        {
            SpellOffensive.interactable = true;
        }
        else
        {
            SpellOffensive.interactable = false;
        }

        if (_gameplayController.Mana[1] >= 300)
        {
            SpellEarthquake.interactable = true;
        }
        else
        {
            SpellEarthquake.interactable = false;
        }

        if (_gameplayController.Mana[1] >= 500)
        {
            SpellTakeover.interactable = true;
        }
        else
        {
            SpellTakeover.interactable = false;
        }

        if (_gameplayController.SpellToCast == 0)
        {
            Image image = SpellOffensive.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
        else
        {
            Image image = SpellOffensive.GetComponent<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }

        if (_gameplayController.SpellToCast == 1)
        {
            Image image = SpellEarthquake.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
        else
        {
            Image image = SpellEarthquake.GetComponent<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }

        if (_gameplayController.SpellToCast == 2)
        {
            Image image = SpellTakeover.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
        else
        {
            Image image = SpellTakeover.GetComponent<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
    }

    /// <summary>
    /// Check if only one vertex is selected and if level is below 5
    /// then activate or not upgrade button
    /// </summary>
    void SetUpgradeButton()
    {
        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexA.Level < 5)
        {
            Upgrade.gameObject.SetActive(true);
            UpgradeCostText.text = $"{_gameplayController.SelectedVertexA.Level * 25} HONEY";

            if (_gameplayController.Honey[1] >= _gameplayController.SelectedVertexA.Level * 25)
            {
                Upgrade.interactable = true;
            }
            else
            {
                Upgrade.interactable = false;
            }
        }
        else
        {
            Upgrade.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Check if spell to cast by player is choosen 
    /// and show or not spell overlay vignette
    /// </summary>
    void SetCastVignette()
    {
        if (_gameplayController.SpellToCast != -1)
        {
            CastingSpellVignette.gameObject.SetActive(true);
        }
        else
        {
            CastingSpellVignette.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        SetPlayerUI();
        SetUpgradeButton();
        SetCastVignette();
        SetPauseMenu();
    }

    /// <summary>
    /// Show win canvas message
    /// </summary>
    public void ShowWin()
    {
        WinLoseCanvas.SetActive(true);
        Win.SetActive(true);
    }

    /// <summary>
    /// Show fail canvas message
    /// </summary>
    public void ShowFail()
    {
        WinLoseCanvas.SetActive(true);
        Fail.SetActive(true);
    }

    /// <summary>
    /// Determine if showin pause menu
    /// </summary>
    void SetPauseMenu()
    {
        if (IsShowingPauseMenu)
        {
            PauseMenu.SetActive(true);
        }
        else
        {
            PauseMenu.SetActive(false);
        }
    }

    /// <summary>
    /// Triggered on continue button
    /// </summary>
    public void OnContinue()
    {
        IsShowingPauseMenu = false;
    }

    /// <summary>
    /// Triggered on restart button
    /// </summary>
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Triggered on exit to main menu button
    /// </summary>
    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    /// <summary>
    /// Triggered when button for upgrade is pressed
    /// </summary>
    public void OnUpgrade()
    {
        _gameplayController.UpgradeVertex(_gameplayController.SelectedVertexA);
    }

    /// <summary>
    /// Triggered when button for offensive spell is pressed
    /// </summary>
    public void OnOffensiveSpellButton()
    {
        _gameplayController.SetSpellToCastByPlayer(0);
    }

    /// <summary>
    /// Triggered when button for earthquake spell is pressed
    /// </summary>
    public void OnEarthquakeSpellButton()
    {
        _gameplayController.SetSpellToCastByPlayer(1);
    }

    /// <summary>
    /// Triggered when button for takeover spell is pressed
    /// </summary>
    public void OnTakeoverSpellButton()
    {
        _gameplayController.SetSpellToCastByPlayer(2);
    }

    /// <summary>
    /// Triggered when button for pause menu is pressed
    /// </summary>
    public void OnMenuButton()
    {
        IsShowingPauseMenu = true;
    }

    /// <summary>
    /// Triggered when button for army transport part is pressed
    /// </summary>
    public void OnTransportPartButton()
    {
        float currentPart = _gameplayController.TransportPart;

        switch (currentPart)
        {
            case 0.25f:
                _gameplayController.TransportPart = 0.5f;
                break;
            case 0.5f:
                _gameplayController.TransportPart = 0.75f;
                break;
            case 0.75f:
                _gameplayController.TransportPart = 1f;
                break;
            case 1f:
                _gameplayController.TransportPart = 0.25f;
                break;
        }
    }

    /// <summary>
    /// Triggered when button for time scale speed is pressed
    /// </summary>
    public void OnSpeedButton()
    {
        float currentSpeed = _gameplayController.GameplaySpeedMultiplier;

        switch (currentSpeed)
        {
            case 0.5f:
                _gameplayController.GameplaySpeedMultiplier = 1f;
                break;
            case 1.0f:
                _gameplayController.GameplaySpeedMultiplier = 1.5f;
                break;
            case 1.5f:
                _gameplayController.GameplaySpeedMultiplier = 2f;
                break;
            case 2f:
                _gameplayController.GameplaySpeedMultiplier = 2.5f;
                break;
            case 2.5f:
                _gameplayController.GameplaySpeedMultiplier = 3f;
                break;
            case 3f:
                _gameplayController.GameplaySpeedMultiplier = 3.5f;
                break;
            case 3.5f:
                _gameplayController.GameplaySpeedMultiplier = 4f;
                break;
            case 4f:
                _gameplayController.GameplaySpeedMultiplier = 0.5f;
                break;
        }
    }
}
