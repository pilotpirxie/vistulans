using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Button UpgradeButton;
    public TextMeshProUGUI HoneyText;
    public TextMeshProUGUI ManaText;
    public TextMeshProUGUI ArmyText;

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

        if (ArmyText.text != _gameplayController.Army[0].ToString())
        {
            ArmyText.text = _gameplayController.Army[0].ToString();
        }
    }

    public void OnUpgrade()
    {

    }
}
