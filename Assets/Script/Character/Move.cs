using UnityEngine;
using UnityEngine.InputSystem;


public class Move : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed = 5.0f;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private FollowCamera followCamera;
    private Rigidbody      playerRigidbody;
    private PlayerCharacter character;
    private Vector2        moveInput;
    private Animator       animator;

    private bool isMove;
    private bool canMove = true;
    private RaycastHit hit;
    private int stepLayer;
    private float maxSlopeAngle = 45.0f;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public bool CanMove
    {
        get => canMove;
        set 
        { 
            canMove = value;
            if (!canMove)
            {
                playerRigidbody.velocity = Vector3.zero;
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                moveInput = Vector2.zero;
            }
        }

    }

    public bool IsMove { get => isMove; set => isMove = value; }

    void Start()
    {
        character = GetComponent<PlayerCharacter>();
        stepLayer = 1 << LayerMask.NameToLayer("Step");
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(character.IsDead) return;


        IsMove = moveInput.magnitude != 0;
        animator.SetBool("Walk", IsMove);
        if (CanMove)
        {
            Vector3 lookForward = new Vector3(followCamera.CamPos.forward.x, 0.0f, followCamera.CamPos.forward.z).normalized;
            Vector3 lookRight = new Vector3(followCamera.CamPos.right.x, 0.0f, followCamera.CamPos.right.z).normalized;
            Vector3 moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;
            if (IsMove && !character.IsRoll)
            {
                var look = Quaternion.LookRotation(moveDirection);
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, look, Time.fixedDeltaTime * rotSpeed);

            }
            //경사로 판별
            bool onslope = false;
            Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.black);
            if (Physics.Raycast(ray, out hit, 1.5f, stepLayer))
            {
                //upVector와 경사로의 normal값을 이용하여 각도판별
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                onslope = angle < maxSlopeAngle;
            }

            Vector3 gravity = onslope ? Vector3.zero : Vector3.down * Mathf.Abs(playerRigidbody.velocity.y);

            if (onslope)
            {
                Vector3 dir = Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;

                playerRigidbody.velocity = dir * MoveSpeed + gravity;
            }
            else
            {
                playerRigidbody.velocity = moveDirection * MoveSpeed + gravity;
            }

            if (character.IsRoll) 
            {
                if (onslope)
                {
                    Vector3 dir = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
                    Debug.Log(gravity);
                    playerRigidbody.velocity = dir * 6.0f + gravity;
                }
                else
                {
                    playerRigidbody.velocity = transform.forward * 6.0f + gravity;
                }
            }
        }

    }

    public void OnMove(InputAction.CallbackContext value)
    {

       Vector2 input = value.ReadValue<Vector2>();
       if (input != null)
       {
           moveInput = new Vector2(input.x, input.y);
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
        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;

    }

}
