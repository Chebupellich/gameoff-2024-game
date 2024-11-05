using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    private float moveSpeed = 3f;
    private bool isWalking = false;

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Debug.Log("Logic of counter interaction");
    }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        if (inputVector.x == -1)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (inputVector.x == 1)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0);

        isWalking = moveDir != Vector3.zero;

        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
