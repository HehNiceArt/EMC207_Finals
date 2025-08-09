using UnityEngine;

namespace Platformer
{
    public class AI_FSM : MonoBehaviour
    {
        private enum AiState
        {
            Idle,
            Chase,
            Patrol,
            Sprint,
            Attack,
            Cooldown,
        }
        [SerializeField] AiState currentState = AiState.Idle;
        [SerializeField] Transform[] patrolPoints;
        [SerializeField] Transform player;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float attackDistance = 2f;
        [SerializeField] int attackDamage = 5;
        float cooldownTimer = 3f;
        [SerializeField] Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * chaseDistance, Color.yellow);
            Debug.DrawRay(transform.position, transform.forward * attackDistance, Color.red);
            switch (currentState)
            {
                case AiState.Idle:
                    IdleUpdate();
                    break;
                case AiState.Chase:
                    ChaseUpdate();
                    break;
                case AiState.Attack:
                    AttackUpdate();
                    break;
                case AiState.Patrol:
                    PatrolUpdate();
                    break;
                case AiState.Cooldown:
                    CooldownUpdate();
                    break;
            }
        }
        void IdleUpdate()
        {
            animator.Play("Locomotion");
            if (Time.timeSinceLevelLoad > 2f)
                ChangeState(AiState.Patrol);
        }
        int patrolIndex = 0;
        void PatrolUpdate()
        {
            animator.Play("Sprint");
            if (patrolPoints.Length == 0) return;

            Transform targetPoint = patrolPoints[patrolIndex];
            Vector3 targetPos = targetPoint.position;
            targetPos.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, Time.deltaTime * 2f);
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            }

            //Targets player
            if (player && Vector3.Distance(transform.position, player.position) < chaseDistance)
                ChangeState(AiState.Chase);
        }

        void ChaseUpdate()
        {
            animator.Play("Sprint");
            if (player)
            {
                Vector3 targetPos = player.position;
                targetPos.y = transform.position.y;
                Vector3 direction = (player.position - transform.position).normalized;
                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(direction);
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 2f);
                if (distanceToPlayer < attackDistance)
                    ChangeState(AiState.Attack);
                else if (distanceToPlayer > chaseDistance)
                    ChangeState(AiState.Patrol);
            }
        }

        void AttackUpdate()
        {
            Vector3 attackPos = transform.position + transform.forward;
            Collider[] hitPlayers = Physics.OverlapSphere(attackPos, attackDistance);

            animator.Play("Attack");
            foreach (var player in hitPlayers)
            {
                if (player.CompareTag("Player") && Vector3.Distance(transform.position, player.transform.position) < attackDistance)
                {
                    player.GetComponent<Health>().TakeDamage(attackDamage);
                    ChangeState(AiState.Cooldown);
                }
            }
        }
        void CooldownUpdate()
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 2f)
            {
                animator.Play("Locomotion");
            }

            Debug.Log($"Cooldown: {cooldownTimer}");
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 3f;
                ChangeState(AiState.Idle);
            }
        }

        void ChangeState(AiState newState)
        {
            currentState = newState;
        }
    }
}
