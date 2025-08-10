using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Heart, Weapon, Armor, Key }

public class ItemPickup : MonoBehaviour
{
    public ItemType itemType;
    public int amount = 1; // 心是回血量，钥匙是数量，武器/护甲通常为1件
    public string itemName; // 武器或护甲的名字

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            PlayerInventory pi = other.GetComponent<PlayerInventory>();

            if (pi == null) return;

            switch (itemType)
            {
                case ItemType.Heart:
                    if (ph != null) ph.Heal(amount);
                    break;
                case ItemType.Weapon:
                    pi.AddWeapon(itemName);
                    break;
                case ItemType.Armor:
                    pi.AddArmor(itemName);
                    break;
                case ItemType.Key:
                    pi.AddKey(amount);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
