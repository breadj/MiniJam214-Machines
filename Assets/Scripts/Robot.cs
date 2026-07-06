using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    public enum RobotColour
    {
        None = 0,
        Blue = 1,
        Red = 2
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Robot : MonoBehaviour
    {
        [SerializeField] private RobotSettings settings;
        [SerializeField] private RobotColour robotColour;
        public RobotColour Colour => robotColour;

        private static readonly int animatorInAirHash = Animator.StringToHash("IsInAir");
        private static readonly int animatorWalkingHash = Animator.StringToHash("IsWalking");

        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Animator animator;

        private Vector2 wanderDirection;
        private CancellationTokenSource wanderCTS;

        private float timeAlive = 0f;
        private bool timerPaused = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            timerPaused = false;
            //StartWandering();
        }

        // Update is called once per frame
        void Update()
        {
            CheckSpriteFlip();

            if (!timerPaused)
            {
                timeAlive += Time.deltaTime;
                if (timeAlive >= settings.Lifetime)
                {
                    GameplayManager.Instance.ReportRobotDeath(this);
                    Explode();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 normal = collision.GetContact(0).normal;
            wanderDirection = Vector2.Reflect(wanderDirection.normalized, normal).normalized;

            rb.linearVelocity = wanderDirection * settings.MoveSpeed;
        }

        public void PickUp(Transform pickerUpper)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
            transform.SetParent(pickerUpper);

            animator.SetBool(animatorInAirHash, true);

            StopWandering();
        }

        public void PutDown()
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
            transform.SetParent(null);

            animator.SetBool(animatorInAirHash, false);

            StartWandering();
        }

        private void CheckSpriteFlip()
        {
            if (rb.linearVelocityX < 0)
            {
                sr.flipX = true;
            }
            else if (rb.linearVelocityX > 0)
            {
                sr.flipX = false;
            }

            // if linear velocity X == 0, then just keep the current orientation
        }

        #region Random Funcs
        private float GetRandomWaitTime()
        {
            return Random.Range(settings.WanderWaitTime.Min, settings.WanderWaitTime.Max);
        }

        private float GetRandomWalkDuration()
        {
            return Random.Range(settings.WanderWalkDuration.Min, settings.WanderWalkDuration.Max);
        }

        private Vector2 GetRandomDirection()
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        #endregion Random Funcs

        #region Wander Logic
        private void StartWandering()
        {
            wanderCTS?.Cancel();
            wanderCTS?.Dispose();

            wanderCTS = new CancellationTokenSource();
            Wander(wanderCTS.Token).Forget();
        }

        private void StopWandering()
        {
            wanderCTS?.Cancel();
            wanderCTS.Dispose();
            wanderCTS = null;
        }

        private async UniTaskVoid Wander(CancellationToken cancelToken)
        {
            try
            {
                while (true)
                {
                    float timeToWait = GetRandomWaitTime();
                    await UniTask.WaitForSeconds(timeToWait, cancellationToken: cancelToken);

                    wanderDirection = GetRandomDirection();
                    rb.linearVelocity = wanderDirection * settings.MoveSpeed;

                    animator.SetBool(animatorWalkingHash, true);

                    float walkDuration = GetRandomWalkDuration();
                    await UniTask.WaitForSeconds(walkDuration, cancellationToken: cancelToken);

                    rb.linearVelocity = Vector2.zero;
                    animator.SetBool(animatorWalkingHash, false);
                }
            }
            catch (System.OperationCanceledException)
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool(animatorWalkingHash, false);
            }
        }
        #endregion Wander Logic

        public void Explode()
        {
            StopWandering();
            timerPaused = true;
            rb.simulated = false;

            GameObject deathFXObject = Instantiate(settings.ExplosionFXPrefab, transform.position, Quaternion.identity);
            if (deathFXObject.TryGetComponent(out DestroyAfterAnimation deathFXScript))
            {
                deathFXScript.OnAnimationEnd += DestroySelf;
            }
        }

        public void Teleport()
        {
            StopWandering();
            timerPaused = true;
            rb.simulated = false;

            GameObject teleportFXObject = Instantiate(settings.TeleportFXPrefab, transform.position, Quaternion.identity);
            if (teleportFXObject.TryGetComponent(out DestroyAfterAnimation teleportFXScript))
            {
                teleportFXScript.OnAnimationEnd += DestroySelf;
            }
        }

        public void SpawnIn()
        {
            GameObject spawnFXObject = Instantiate(settings.SpawnFXPrefab, transform.position, Quaternion.identity);
            if (spawnFXObject.TryGetComponent(out DestroyAfterAnimation spawnFXScript))
            {
                spawnFXScript.OnAnimationEnd += StartWandering;
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
