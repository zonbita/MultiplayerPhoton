using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject gunObject;
    [SerializeField] private Weapon_Gun[] Item;
    [SerializeField] private Image HealthbarGO;
    [SerializeField] public PlayerManager playerManager;
    PhotonView PV;


    public float Health = 100f;
    [SerializeField]
    public Rigidbody rb;

    [SerializeField] private NameTag nameTag;
    public Vector3 moveInput;
    private Vector3 position;
    public DynamicJoystick dynamicJoystick;

    private Vector3 direction;
    private Quaternion rotation, GunRotation;
    private bool running;
    private float smoothing = 10.0f;



    void Awake()
    {
        dynamicJoystick = FindObjectOfType<DynamicJoystick>();
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            rb = GetComponentInChildren<Rigidbody>();
            cameraObject.SetActive(true);
            GetComponentInChildren<PhotonTransformView>();
        }
        else {
            rb = GetComponentInChildren<Rigidbody>();
            Destroy(rb);
            cameraObject.SetActive(false);
        } 
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
            //gunObject.transform.rotation = Quaternion.Lerp(gunObject.transform.rotation, GunRotation, Time.deltaTime * smoothing);
        }
    }


    void FixedUpdate()
    {
        if (!PV.IsMine) return;

        Movement(); 
        Rotate();

        if (Input.GetKey(KeyCode.Mouse0))
            Shoot();
    }

    void Shoot()
    {
        PV.RPC("RPC_Shoot", RpcTarget.AllViaServer);
    }



    [PunRPC]
    public void RPC_Shoot()
    {

        if (Time.time > Item[0].nextFire)
        {
            //animator.CrossFade("Shoot", 0.3f);
            Item[0].nextFire = Time.time + Item[0].fireRate;

            GameObject bullet = Instantiate(Item[0].bulletPrefab, Item[0].bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<BulletController>().InitializeBullet(gunObject.transform.rotation * Vector3.forward, PV.Owner);
            animator.Play("Shoot", 0, 0);
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Range);
            animator.Play("Idle", 0, 0.3f);
        }

    }

    IEnumerator ResetAni()
    {
        yield return new WaitForSeconds(1.0f);
        animator.CrossFade("", 0.0f);
    }

    void Movement()
    { 
        if (moveInput.magnitude != 0.0) running = true; else running = false;
        moveInput.x = InputSystem.Instance.moveVector.x;
        moveInput.z = InputSystem.Instance.moveVector.y;

        direction = Vector3.forward * dynamicJoystick.Vertical + Vector3.right * dynamicJoystick.Horizontal;
        rb.MovePosition(transform.position + direction * 6 * Time.fixedDeltaTime);

        rb.MovePosition(transform.position + moveInput * 6 * Time.fixedDeltaTime);

        animator.SetBool("Running", running);
    }

    Vector3 lookPos, lookDir;
    Ray ray;
    RaycastHit hit;
    void Rotate()
    {
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        lookDir = lookPos - gunObject.transform.position;
        lookDir.y = 0;

        gunObject.transform.LookAt(gunObject.transform.position + lookDir, Vector3.up);
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        //if (!PV.IsMine) return; 

        Health = Mathf.Clamp(Health - damage, 0, 100);

        if (Health == 0) 
        { 
            Die();
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
            
        } else
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }

        HealthbarGO.fillAmount = (float)Health / 100f;
    }

    public void Die()
    { 
        if(playerManager) playerManager.Die();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(gunObject.transform.rotation);
            stream.SendNext(animator.GetBool("Running"));
        } 
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            //GunRotation = (Quaternion)stream.ReceiveNext();
            animator.SetBool("Running", (bool)stream.ReceiveNext());
        }
    }
}
