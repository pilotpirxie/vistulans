using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Button UpgradeButton;
    public Button SpellOffensive;
    public Button SpellEarthquake;
    public Button SpellTakeover;

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
        if (HoneyText.text != _gameplayController.Honey[0].ToString())
        {
            HoneyText.text = _gameplayController.Honey[0].ToString();
        }

        if (ManaText.text != _gameplayController.Mana[0].ToString())
        {
            ManaText.text = _gameplayController.Mana[0].ToString();
        }

        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexA.Level < 5)
        {
            UpgradeButton.gameObject.SetActive(true);
            UpgradeCostText.text = $"{_gameplayController.SelectedVertexA.Level * 25} HONEY";
        }
        else
        {
            UpgradeButton.gameObject.SetActive(false);
        }

        if (_gameplayController.SpellToCast != -1)
        {
            CastingSpellVignette.gameObject.SetActive(true);
        }
        else
        {
            CastingSpellVignette.gameObject.SetActive(false);
        }
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
}
