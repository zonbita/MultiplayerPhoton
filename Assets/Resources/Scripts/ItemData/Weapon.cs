using UnityEngine;


public abstract class Weapon : Item
{
    public int damagePerShot = 20;
    public float fireRate = 0.2f;
    public float weaponRange = 100.0f;



    public abstract override void Use();
}
