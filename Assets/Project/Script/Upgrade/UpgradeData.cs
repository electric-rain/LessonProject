using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeLevel
{
    public int level;
    public int cost;
    public float value;
    public string description;
}

[System.Serializable]
public class UpgradeBranch
{
    public string branchName;
    public List<UpgradeLevel> levels;
}

[System.Serializable]
public class GameUpgradeConfig
{
    public List<UpgradeBranch> branches;
}
