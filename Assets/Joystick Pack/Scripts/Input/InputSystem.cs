using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;


public partial class InputSystem : Singleton<InputSystem>
{
    private Controls controls;
    public Vector2 moveVector;
    public Vector2 mousePosition;
    public bool isPressingLMB=false;
    private void Start()
    {

        controls = new Controls();
        controls.Enable();
    }

    protected  void FixedUpdate()
    {
        moveVector = controls.ActionMap.Movement.ReadValue<Vector2>();
        mousePosition = controls.ActionMap.MousePosition.ReadValue<Vector2>();
        isPressingLMB = controls.ActionMap.Shoot.ReadValue<float>() == 1 ? true : false;
    }
}
