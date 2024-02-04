using Photon.Pun;
using System;
using UnityEngine;

public class Weapon_Gun : Weapon
{
    [Header("Options")]
    [SerializeField] public ParticleSystem gunParticles;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform bulletPosition;

    public float nextFire;


    void Awake()
    {
        if (GunModel != null)
            GunModel = Instantiate(GunModel.gameObject,this.transform);
    }

    public override void Use() 
    {

    }


}
