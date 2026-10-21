using UnityEngine;

public class Frog : MonoBehaviour
{
    public float lifeTime = 1.5f;   // seconds before the frog escapes
    bool isDead;
    Animator anim;
    Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Give the frog a random push so it drifts (physics)
        rb.AddForce(Random.insideUnitCircle * 2f, ForceMode2D.Impulse);

        // If not killed in time, it escapes
        Invoke(nameof(Escape), lifeTime);
    }

    void Escape()
    {
        if (isDead) return;
        GameManager.Instance.FrogEscaped();
        Destroy(gameObject);
    }

    public void Kill()
    {
        if (isDead) return;
        isDead = true;
        CancelInvoke();                              // stop the escape timer
        GetComponent<Collider2D>().enabled = false;  // can't be clicked twice
        rb.simulated = false;                        // stop moving
        anim.SetTrigger("Die");                      // play squash animation
        Destroy(gameObject, 0.4f);                   // remove after animation
    }
}