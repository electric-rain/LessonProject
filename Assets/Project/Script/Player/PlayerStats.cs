using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _damageMultiplayer = 1f;
    private int _damageBonus = 0;
    private int _healtheBonus = 0;

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

    public int ModifyDamage(int baseWeaponDamage)
    {
        return Mathf.RoundToInt((baseWeaponDamage + _damageBonus) * _damageMultiplayer);
    }
}
