using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int keys = 0;

    // weapen & armour lists
    public List<string> weapons = new List<string>();
    public List<string> armors = new List<string>();

    public int currentWeaponIndex = 0;
    public int currentArmorIndex = 0;

    // keys
    public void AddKey(int count = 1)
    {
        keys += count;
        Debug.Log("Keys collected: " + keys);
    }

    public bool UseKey()
    {
        if (keys > 0)
        {
            keys--;
            Debug.Log("Key used, remaining: " + keys);
            return true;
        }
        else
        {
            Debug.Log("No keys available");
            return false;
        }
    }

    public void AddWeapon(string weaponName)
    {
        weapons.Add(weaponName);
        Debug.Log("Added weapon: " + weaponName);
    }

    public void AddArmor(string armorName)
    {
        armors.Add(armorName);
        Debug.Log("Added armor: " + armorName);
    }

    public string GetCurrentWeapon()
    {
        if (weapons.Count == 0) return null;
        return weapons[currentWeaponIndex];
    }

    public string GetCurrentArmor()
    {
        if (armors.Count == 0) return null;
        return armors[currentArmorIndex];
    }

    public void SwitchWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            currentWeaponIndex = index;
            Debug.Log("Switched to weapon: " + weapons[index]);
        }
    }

    public void SwitchArmor(int index)
    {
        if (index >= 0 && index < armors.Count)
        {
            currentArmorIndex = index;
            Debug.Log("Switched to armor: " + armors[index]);
        }
    }
}
