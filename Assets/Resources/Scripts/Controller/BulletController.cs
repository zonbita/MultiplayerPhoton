using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class BulletController : MonoBehaviourPun
{
    Rigidbody rb;

    public float bulletSpeed = 15f;
    public AudioClip BulletHitAudio;
    public float bulletLifeTime = 10f;
    public GameObject bulletImpactEffect;
    public float damage = 10;

    public Player Owner { get; private set; }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(Vector3 originalDirection, Player owner)
    {
        Owner = owner;
        transform.forward = originalDirection;
        rb.velocity = transform.forward * bulletSpeed;

        Destroy(gameObject, bulletLifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PhotonView p = collision.gameObject.GetComponent<PhotonView>();
        if (p && p.Owner == Owner) return;

        if (collision.gameObject.CompareTag("Player"))
             p.RPC("RPC_TakeDamage", RpcTarget.All, damage);
            
        Destroy(gameObject);

        //AudioManager.Instance.Play3D(BulletHitAudio, transform.position);

        //VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);

    }
}
