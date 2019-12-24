using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Button Upgrade;
    public Button SpellOffensive;
    public Button SpellEarthquake;
    public Button SpellTakeover;

    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI TransportPartText;
    public TextMeshProUGUI UpgradeCostText;
    public TextMeshProUGUI HoneyText;
    public TextMeshProUGUI ManaText;

    public Image CastingSpellVignette;

    private GameplayController _gameplayController;

    private void Start()
    {
        _gameplayController = GameObject.FindWithTag("Mechanism").GetComponent<GameplayController>();
    }

    void Update()
    {
        HoneyText.text = _gameplayController.Honey[0].ToString();
        ManaText.text = _gameplayController.Mana[0].ToString();


        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexA.Level < 5)
        {
            Upgrade.gameObject.SetActive(true);
            UpgradeCostText.text = $"{_gameplayController.SelectedVertexA.Level * 25} HONEY";
        }
        else
        {
            Upgrade.gameObject.SetActive(false);
        }

        if (_gameplayController.SpellToCast != -1)
        {
            CastingSpellVignette.gameObject.SetActive(true);
        }
        else
        {
            CastingSpellVignette.gameObject.SetActive(false);
        }

        TransportPartText.text = $"{_gameplayController.TransportPart*100}%";
        SpeedText.text = $"x{_gameplayController.GameplaySpeedMultiplier}";
    }

    public void OnUpgrade()
    {
        _gameplayController.UpgradeVertex(_gameplayController.SelectedVertexA);
    }

    public void OnOffensiveSpellButton()
    {
        _gameplayController.SetSpellToCast(0);
    }   
        
    public void OnEarthquakeSpellButton()
    {
        _gameplayController.SetSpellToCast(1);
    }   
        
    public void OnTakeoverSpellButton()
    {
        _gameplayController.SetSpellToCast(2);
    }

    public void OnMenuButton()
    {
        _gameplayController.IsShowingMenu = true;
    }

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

        Time.timeScale = _gameplayController.GameplaySpeedMultiplier;
    }
}
