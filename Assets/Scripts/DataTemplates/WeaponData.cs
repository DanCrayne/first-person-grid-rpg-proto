using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public string description;
    public int weightInPounds;
    public int valueInGold;
    public int minDamage;
    public int maxDamage;
}
