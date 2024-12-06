using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Move : MonoBehaviour
{
    private readonly float rotSpeed = 5.0f;
    private readonly float moveSpeed = 5.0f;
    private Rigidbody      playerRigidbody;
    private Vector2        moveInput;
    private Animator       animator;
    [SerializeField]
    private FollowCamera followCamera;
    private bool isMove;
    private bool canMove = true;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        followCamera.CamTarget.position = playerRigidbody.position + followCamera.OffSet;
        if(canMove)
        {
            isMove = moveInput.magnitude != 0;
            animator.SetBool("Walk", isMove);
            if (isMove)
            {
                Vector3 lookForward = new Vector3(followCamera.CamTarget.forward.x, 0.0f, followCamera.CamTarget.forward.z).normalized;
                Vector3 lookRight = new Vector3(followCamera.CamTarget.right.x, 0.0f, followCamera.CamTarget.right.z).normalized;
                Vector3 moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;

                var look = Quaternion.LookRotation(moveDirection);
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, look, Time.fixedDeltaTime * rotSpeed);
                playerRigidbody.MovePosition(playerRigidbody.position + moveDirection * Time.fixedDeltaTime * moveSpeed);

            }
        }

    }


    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        if (input != null)
        {
            moveInput = new Vector2(input.x,input.y);
        }

    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 mousePosition = value.ReadValue<Vector2>();
        followCamera.CamLook = mousePosition;
    }

    public void StopMovement()
    {
        canMove = false;
    }
    public void AllowMovement()
    {
        canMove = true;
    }
    public void RotateToTarget(Vector3 target)
    {

        playerRigidbody.rotation = Quaternion.LookRotation(target - playerRigidbody.position);
    }
}
