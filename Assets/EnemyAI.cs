using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public float jumpHeight = 5f;
    public float jumpDistance = 5f;
    public float jumpDuration = 0.5f;
    public float obstacleCheckDistance = 2.2f;

    private bool isJumping = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null || isJumping) return;

        agent.SetDestination(player.position);

        CheckForJump();
    }

    void CheckForJump()
    {
        // kierunek ruchu
        Vector3 dir = agent.velocity.normalized;

        if (dir == Vector3.zero) return;

        RaycastHit hit;

        // wykryj przeszkodę przed enemy
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out hit, obstacleCheckDistance))
        {
            // sprawdź czy to coś jest wyżej niż my (czyli platforma)
            float heightDifference = hit.point.y - transform.position.y;

            if (heightDifference > 0.3f && heightDifference < jumpHeight + 1f)
            {
                StartCoroutine(Jump(hit.point + dir * jumpDistance));
            }
        }
    }

    IEnumerator Jump(Vector3 targetPos)
    {
        isJumping = true;
        agent.enabled = false; // wyłącz NavMesh na czas skoku

        Vector3 startPos = transform.position;
        float time = 0;

        while (time < jumpDuration)
        {
            float t = time / jumpDuration;

            float height = 4 * jumpHeight * t * (1 - t);

            transform.position = Vector3.Lerp(startPos, targetPos, t) + Vector3.up * height;

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        agent.enabled = true; // włącz z powrotem
        isJumping = false;
    }
}