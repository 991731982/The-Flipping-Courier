using UnityEngine;

public class AshleyCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField, Tooltip("Me")] private float rotateSpeed = 180.0f;
    private readonly int speedParamHash = Animator.StringToHash("Speed");
    private readonly int jumpParamHash = Animator.StringToHash("Jump");

    float verticalInput;
    float horizontalInput;

    void OnValidate()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Rotate the character based on horizontal input
        transform.Rotate(0, rotateSpeed * horizontalInput * Time.deltaTime, 0);

        // Set speed parameter in the Animator
        animator.SetFloat(speedParamHash, verticalInput);

        // Trigger jump if the Jump button is pressed
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger(jumpParamHash);
        }
    }
}
