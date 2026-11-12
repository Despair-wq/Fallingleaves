using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;          // سرعت حرکت بازیکن
    public float jumpForce = 10f;     // قدرت پرش

    [Header("Ground Check")]
    public Transform groundCheck;     // نقطه‌ای زیر پای کاراکتر برای تشخیص زمین
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;     // لایه‌ای که به تایل‌های زمین اختصاص دادی
    private bool isGrounded;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // پریفب گلوله
    public Transform firePoint;     // نقطه خروج گلوله
    public float fireRate = 0.5f;   // تاخیر بین هر شلیک
    private float nextFireTime = 0f;

    [Header("Death Settings")]
    public float fallYThreshold = -10f; // اگر مختصات Y از این کمتر شود، پلیر می‌افتد
    public float deathDelay = 1.5f;     // تاخیر برای مرگ عادی با انیمیشن
    private bool isDead = false;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // State
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        CheckGrounded();
        Move();
        Jump();
        HandleCombatInputs();
        CheckFallDeath();

        anim.SetBool("IsJumping", !isGrounded);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleCombatInputs()
    {
        // شلیک گلوله فقط با کلید K یا کلید راست ماوس (Mouse 1)
        if ((Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(1)) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            anim.SetTrigger("Shoot");
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        // حمله با شمشیر فقط با کلید J یا کلید چپ ماوس (Mouse 0)
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }

        // دکمه تستی آسیب دیدن
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("Hurt");
        }

        // دکمه تستی مرگ عادی با کلید T
        if (Input.GetKeyDown(KeyCode.T))
        {
            DieNormal();
        }
    }

    void CheckFallDeath()
    {
        if (transform.position.y < fallYThreshold)
        {
            FallDeath();
        }
    }

    // مرگ عادی (همراه با پخش انیمیشن مرگ)
    public void DieNormal()
    {
        if (isDead) return;
        isDead = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        StartCoroutine(LoadGameOverAfterDelay(deathDelay));
    }

    // مرگ ناشی از سقوط در دره یا DeathZone (بدون انیمیشن مرگ)
    public void FallDeath()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(LoadGameOverAfterDelay(0.5f));
    }

    IEnumerator LoadGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Game Over");
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone") && !isDead)
        {
            FallDeath();
        }
    }
}