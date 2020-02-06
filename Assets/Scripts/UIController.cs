using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private GameplayController _gameplayController;

    private void Start()
    {
        _gameplayController = GameObject.FindWithTag("Mechanism").GetComponent<GameplayController>();
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
        _gameplayController.IsShowingMenu = true;
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
