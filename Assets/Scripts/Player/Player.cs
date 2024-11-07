using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    public static Player Instance { get; private set; }

    private float moveSpeed = 3f;
    private bool isWalking = false;
    private Vector2 lastInteractDirection;
    private ClearCounter selectedCounter;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>(false);
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInterract();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        if (inputVector.x == -1)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputVector.x == 1)
        {
            spriteRenderer.flipX = true;
        }

        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0);

        isWalking = moveDir != Vector3.zero;

        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void HandleInterract()
    {
        Vector2 forwardDir = transform.forward;

        if (forwardDir != Vector2.zero)
        {
            lastInteractDirection = forwardDir;
        }

        float interactDistance = 1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInteractDirection, interactDistance, countersLayerMask);


        if (raycastHit.collider != null)
        {
            if (raycastHit.collider.TryGetComponent(out ClearCounter clearCounter))
            {
                if (selectedCounter != clearCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(ClearCounter counter)
    {
        selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = counter });
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
