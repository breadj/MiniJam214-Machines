using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace BreadJ.MiniJam214
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Robot : MonoBehaviour
    {
        [SerializeField] private RobotSettings settings;

        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private bool isMoving = false;
        private CancellationTokenSource wanderCTS;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartWandering();
        }

        // Update is called once per frame
        void Update()
        {
            CheckSpriteFlip();
        }

        private void FixedUpdate()
        {
            if (isMoving)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * settings.MoveSpeed;
            }
        }

        public void PickUp(Transform pickerUpper)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
            transform.SetParent(pickerUpper);

            StopWandering();
        }

        public void PutDown()
        {
            rb.simulated = true;
            transform.SetParent(null);

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
            return Random.insideUnitCircle;
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

                    Vector2 wanderDirection = GetRandomDirection();
                    rb.linearVelocity = wanderDirection * settings.MoveSpeed;
                    isMoving = true;

                    float walkDuration = GetRandomWalkDuration();
                    await UniTask.WaitForSeconds(walkDuration, cancellationToken: cancelToken);

                    rb.linearVelocity = Vector2.zero;
                    isMoving = false;
                }
            }
            catch (System.OperationCanceledException)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
            }
        }
        #endregion Wander Logic
    }
}
