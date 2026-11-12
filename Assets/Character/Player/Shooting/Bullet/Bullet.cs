using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // پیدا کردن بازیکن از طریق تگ
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float direction = 1f;

        if (player != null && player.transform.localScale.x < 0)
        {
            direction = -1f;
        }

        // اعمال حرکت
        rb.linearVelocity = new Vector2(direction * speed, 0f);

        // چرخش ظاهری تصویر گلوله (رفع حالت دنده عقب)
        if (direction < 0)
        {
            // روش اول: برعکس کردن تصویر (Flip X)
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = true;
            }

            // اگر گلوله شما پارتیکل یا جزئیات دیگری دارد، این خط را هم فعال کنید تا کل آبجکت بچرخد:
            // transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // نابودی خودکار پس از 3 ثانیه
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player") || hitInfo.name == "Player") return;
        Destroy(gameObject);
    }
}