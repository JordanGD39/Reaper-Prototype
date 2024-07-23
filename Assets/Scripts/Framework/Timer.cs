using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private bool canCountOnAwake;
        [SerializeField] private bool canCount;
        [SerializeField, Tooltip("Time in seconds.")] private float startingTime;

        private float _currentTimer;
        private float _totalTimer;
        private bool _isStarting;
        private bool _isTimerLengthSmall;

        public bool IsCounting { get; private set; }

        #region Events
        
        [SerializeField] private UnityEvent onTimerDone = new();
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onReset = new();
        [SerializeField] private UnityEvent onAddedSomeTime = new();

        #endregion

        private void Awake()
        {
            SetCanCount(canCountOnAwake);
            
            if (canCountOnAwake)
                StartCounting();
        }

        private void Update() => Counting();

        /// <summary>
        /// Reset the timer to startingTime and calls the onReset event.
        /// </summary>
        public void ResetTimer()
        {
            onReset?.Invoke();
            _currentTimer = startingTime;
            _totalTimer = startingTime;
            _isStarting = true;
        }

        /// <summary>
        /// Set the canCount property, when setting it to true it will start counting, otherwise is stops.
        /// </summary>
        /// <param name="target">The target for the property</param>
        public void SetCanCount(bool target)
        {
            canCount = target;
            
            if (!target)
                IsCounting = false;
        }

        /// <summary>
        /// Set the timer to count.
        /// </summary>
        public void StartCounting()
        {
            canCount = true;
            _isStarting = true;
            _totalTimer = startingTime;
            _currentTimer = startingTime;
        }

        /// <summary>
        /// Add the given amount to the timer if it's counting. Will invoke the onAddedSomeTime event.
        /// </summary>
        public void AddSomeTime(float addAmount)
        {
            _currentTimer += addAmount;
            onAddedSomeTime?.Invoke();
        }
        
        /// <summary>
        /// Set the timer lenght, when resetting it will use this number.
        /// </summary>
        /// <param name="target">The target amount for the timer</param>
        public void SetCurrentTimerLength(float target) => _currentTimer = target;
        
        /// <summary>
        /// Set the timer lenght, when resetting it will use this number.
        /// </summary>
        /// <param name="target">The target amount for the timer</param>
        public void SetTimerLength(float target) => startingTime = target;

        /// <summary>
        /// Get the timer it's current lenght.
        /// </summary>
        /// <returns>The current timer lenght</returns>
        public float GetCurrentTimerLength() => _currentTimer;

        /// <summary>
        /// Calculates the percentage of the current timer relative to the progress.
        /// </summary>
        /// <returns>Return a number between 0-1</returns>
        public float GetCurrentTimerPercentage()
        {
            const float HUNDRED_PERCENT = 100;
            float currentPercent = _currentTimer / _totalTimer * HUNDRED_PERCENT;
            return currentPercent;
        }
        
        private void Counting()
        {
            if (!canCount)
                return;
            
            if (_isStarting)
            {
                _isStarting = false;
                onStart?.Invoke();
            }

            _currentTimer -= Time.deltaTime;
            IsCounting = true;

            if (_currentTimer > 0)
                return;
            
            onTimerDone?.Invoke();
            SetCanCount(false);
        }
    }
}