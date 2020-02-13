using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectLevelController : MonoBehaviour
{
    /// <summary>
    /// Prefab of button, used for changing current level
    /// </summary>
    public GameObject LevelButtonPrefab;

    /// <summary>
    /// Panel behind buttons
    /// </summary>
    public GameObject Panel;

    /// <summary>
    /// Temporary info about player progress
    /// </summary>
    private int FinishedLevels = 0;

    void Start()
    {
        if (PlayerPrefs.HasKey("FinishedLevels"))
        {
            FinishedLevels = PlayerPrefs.GetInt("FinishedLevels", 0);
        }
        else
        {
            PlayerPrefs.SetInt("FinishedLevels", 0);
            PlayerPrefs.Save();
        }

        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        for(int i = 0; i < levelConfig.levels.Count; i++)
        {
            GameObject newButton = Instantiate(LevelButtonPrefab);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{i+1}";
            newButton.transform.SetParent(Panel.transform);
            int index = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(index));

            if (FinishedLevels < i)
            {
                newButton.GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>
    /// Change current level and go to battle scene
    /// </summary>
    /// <param name="levelIndex"></param>
    public void LoadLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("LevelToPlayIndex", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Close select level menu
    /// </summary>
    public void OnCloseButton()
    {
        gameObject.SetActive(false);
    }
}
