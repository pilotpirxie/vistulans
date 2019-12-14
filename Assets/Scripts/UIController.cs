using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Button UpgradeButton;
    public TextMeshProUGUI UpgradeCostText;
    public TextMeshProUGUI HoneyText;
    public TextMeshProUGUI ManaText;

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
    }

    public void OnUpgrade()
    {
        _gameplayController.UpgradeVertex(_gameplayController.SelectedVertexA);
    }
}
