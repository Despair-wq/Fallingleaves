using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [Range(0, 1)]
    public float smoothTime = 0.25f; // زمان نرم شدن حرکت (هرچه بیشتر، نرم‌تر)
    public Vector3 offset = new Vector3(0f, 2f, -10f); // تنظیم ارتفاع دوربین

    private Vector3 velocity = Vector3.zero;

    // استفاده از FixedUpdate برای هماهنگی کامل با فیزیک کاراکتر
    void FixedUpdate()
    {
        if (target == null) return;

        // محاسبه نقطه هدف
        Vector3 targetPosition = target.position + offset;

        // حرکت فوق‌العاده نرم با SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}