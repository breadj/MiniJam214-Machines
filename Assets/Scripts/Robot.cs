using System.Collections;
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

        private bool isWandering = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            CheckSpriteFlip();
        }

        public void PickUp(Transform pickerUpper)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.SetParent(pickerUpper);
        }

        public void PutDown()
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            transform.SetParent(null);
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

        private IEnumerator Wander()
        {
            float timeToWait = GetRandomWaitTime();
            yield return new WaitForSeconds(timeToWait);

            Vector2 wanderDirection = GetRandomDirection();

            isWandering = true;
        }
    }
}
