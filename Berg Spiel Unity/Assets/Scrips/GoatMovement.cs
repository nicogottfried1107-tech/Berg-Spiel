
using System.Collections;
using UnityEngine;

public class GoatMovement : MonoBehaviour
{
    [Header("Laufpunkte")]
    public Transform pointA;
    public Transform pointB;

    [Header("Geschwindigkeit")]
    public float walkSpeed = 2.5f;
    public float minChargeSpeed = 4.5f;
    public float maxChargeSpeed = 7f;

    [Header("Pausen")]
    public float minPause = 0.3f;
    public float maxPause = 1.8f;
    [Range(0f, 1f)]
    public float randomPauseChance = 0.15f;

    [Header("Unberechenbarkeit")]
    [Range(0f, 1f)]
    public float turnAroundChance = 0.2f;
    public float minChargeTime = 0.5f;
    public float maxChargeTime = 1.3f;
    public float minWalkTime = 0.5f;
    public float maxWalkTime = 2f;

    private Rigidbody2D rb;
    private Transform target;
    private bool isWaiting;
    private bool isCharging;
    private float currentSpeed;
    private Coroutine behaviorRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (pointA == null || pointB == null)
        {
            Debug.LogError("Bitte Point A und Point B zuweisen!", this);
            enabled = false;
            return;
        }

        target = pointB;
        UpdateDirection();
        behaviorRoutine = StartCoroutine(BehaviorLoop());
    }

    void FixedUpdate()
    {
        if (isWaiting || target == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.MovePosition(Vector2.MoveTowards(
            rb.position,
            new Vector2(target.position.x, rb.position.y),
            currentSpeed * Time.fixedDeltaTime
        ));

        if (Mathf.Abs(rb.position.x - target.position.x) < 0.05f)
        {
            rb.MovePosition(new Vector2(target.position.x, rb.position.y));
            target = target == pointA ? pointB : pointA;
            UpdateDirection();
            StartPause();
        }
    }

    IEnumerator BehaviorLoop()
    {
        while (true)
        {
            isCharging = false;
            currentSpeed = walkSpeed * Random.Range(0.8f, 1.25f);

            yield return new WaitForSeconds(Random.Range(minWalkTime, maxWalkTime));

            if (Random.value < randomPauseChance)
            {
                StartPause();
                yield return new WaitUntil(() => !isWaiting);
            }

            if (Random.value < turnAroundChance)
            {
                target = target == pointA ? pointB : pointA;
                UpdateDirection();
            }

            isCharging = true;
            currentSpeed = Random.Range(minChargeSpeed, maxChargeSpeed);

            yield return new WaitForSeconds(Random.Range(minChargeTime, maxChargeTime));

            isCharging = false;
        }
    }

    void StartPause()
    {
        if (!isWaiting)
            StartCoroutine(PauseRoutine());
    }

    IEnumerator PauseRoutine()
    {
        isWaiting = true;
        currentSpeed = 0f;

        yield return new WaitForSeconds(Random.Range(minPause, maxPause));

        isWaiting = false;
    }

    void UpdateDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) *
                  (target.position.x > transform.position.x ? 1f : -1f);
        transform.localScale = scale;
    }
}
