using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _baseDamage = 20;
    [SerializeField] private float _damageMultiplayer = 1f;
    private int _damageBonus = 0;
    private int _healtheBonus = 0;

    public int CurrentDamage => Mathf.RoundToInt((_baseDamage + _damageBonus) * _damageMultiplayer);

    public void ApplyUpgrade(string branchName, UpgradeLevel upgrade)
    {
        switch (branchName)
        {
            case "Damage":
                _damageBonus += (int)upgrade.value;
                break;

            case "Health":
                _healtheBonus += (int)upgrade.value;
                break;
        }
    }
}
