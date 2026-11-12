using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 2; // میزان جان دشمن (مثلاً با ۲ ضربه نابود شود یا ۱ تیر)

    // اگر از Collider های معمولی (Collision) استفاده می‌کنید:
    void OnCollisionEnter2D(Collision2D collision)
    {
        // بررسی برخورد با تیر
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1); // یک واحد دمیج با تیر
            Destroy(collision.gameObject); // نابود شدن تیر پس از برخورد
        }
    }

    // اگر از Trigger (مثل هوازون شمشیر یا تیرِ Is Trigger شده) استفاده می‌کنید:
    void OnTriggerEnter2D(Collider2D collider)
    {
        // اگر تیر به صورت تریگر بود
        if (collider.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collider.gameObject);
        }

        // اگر محدوده حمله شمشیر (AttackPoint) به دشمن برخورد کرد
        else if (collider.CompareTag("PlayerAttack"))
        {
            TakeDamage(1); // یک واحد دمیج با شمشیر
        }
    }

    void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        // اینجا می‌توانید انیمیشن Hit یا فلش زدن دشمن را هم اضافه کنید

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // کارهای مربوط به مرگ دشمن (مثل پخش افکت صوتی یا پارتیکل)
        Destroy(gameObject);
    }
}