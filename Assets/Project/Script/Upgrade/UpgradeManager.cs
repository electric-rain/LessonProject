using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameUpgradeConfig config;

    private void Awake()
    {
        LoadUpgrade();
    }

    private void LoadUpgrade()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("upgrades");

        if (jsonFile != null)
        {
            config = JsonUtility.FromJson<GameUpgradeConfig>(jsonFile.text);

            Debug.Log("Данные прокачки загружены");

            if (config.branches.Count > 0)
                Debug.Log("Первая ветка: " + config.branches[0].branchName);
        }
        else
            Debug.Log("Файл upgrades.json не найден в папке Resources!");
    }

    public UpgradeLevel GetUpgrade(string branchName, int levelIndex)
    {
        var branch = config.branches.Find(b => b.branchName == branchName);

        if (branch != null && levelIndex < branch.levels.Count)
        {
            return branch.levels[levelIndex];
        }

        return null;
    }
}
