using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public static Player Instance { get; private set; }


    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private float moveSpeed = 3f;
    private bool isWalking = false;

    private Vector2 lastInteractDirection;
    private BaseCounter selectedCounter;
    private SpriteRenderer spriteRenderer;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        Instance = this;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>(false);
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnAlternateInteractAction += GameInput_OnAlternateInteractAction;
    }

    private void GameInput_OnAlternateInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
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
            if (raycastHit.collider.TryGetComponent(out BaseCounter baseCounter))
            {
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
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

    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = counter });
    }

    public bool IsWalking()
    {
        return isWalking;
    }




    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
