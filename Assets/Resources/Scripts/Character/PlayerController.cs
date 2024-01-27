using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject cameraObject;
    [SerializeField]
    private GameObject gunObject;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private NameTag nameTag;
    public Vector3 moveInput;
    private Vector3 position;
    private Quaternion rotation;
    private bool running;
    private float smoothing = 10.0f;




    void Awake()
    {
        if (photonView.IsMine)
        {
            //cameraObject.SetActive(true);
        }
    }


    void Start()
    {
/*        if (photonView.IsMine)
        {
            // MoveToLayer(gunObject, LayerMask.NameToLayer("Hidden"));
            //MoveToLayer(playerObject, LayerMask.NameToLayer("Hidden"));

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponentInChildren<NameTag>().target = nameTag.transform;
            }
        }
        else
        {
            position = transform.position;
            rotation = transform.rotation;

            // Set nametag
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (player != gameObject)
                {
                    nameTag.target = player.GetComponentInChildren<NameTag>().target;
                    break;
                }
            }
        }*/
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
        }
    }


    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (moveInput.magnitude != 0.0) running = true; else running = false;
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.z = Input.GetAxisRaw("Vertical");

            transform.position += (6 * Time.deltaTime * moveInput) / 3;
            animator.SetBool("Running", running);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(animator.GetBool("Running"));
        } 
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            animator.SetBool("Running", (bool)stream.ReceiveNext());
        }
    }
}
