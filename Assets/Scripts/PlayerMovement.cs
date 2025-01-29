using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (moveInput.x < 0)
        {
            // Rotate to face left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveInput.x > 0)
        {
            // Rotate to face right
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        Debug.Log("Magnitude " + moveInput.magnitude);
        animator.SetFloat("Speed", moveInput.magnitude);
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}