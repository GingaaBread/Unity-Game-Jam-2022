namespace TimeManagement
{

    using System;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Events;

    /// <summary>
    /// This class owns...
    /// - The source of truth for current time, made available to other classes via TimeManager.time
    /// - Updating the time at the start of the computer phase
    /// - Firing events (OnStartPreTurn, OnStartPlayerTurn, OnEndPlayerTurn)
    /// - During computer phase, Syncronously calls various other systems during
    ///    computer phase to do their work (like a tile manager, time displayer, and probably a dozen others). 
    ///    To make this easy to update, TimeManager will have a public array of classes which extend ComputerPhaseStep
    ///    and therefore must implement ComputerPhaseStep.ProcessComputerPhase()
    /// </summary>
    /// <author>Ben</author>
    public class TimeManager : MonoBehaviour
    {

        public bool DebugMode;

        public PointInTime CurrentTime { get; private set; }
        public Phase CurrentPhase { get; private set; }

        [HideInInspector] public static UnityEvent OnStartPreTurn = new UnityEvent();
        [HideInInspector] public static UnityEvent OnStartPlayerTurn = new UnityEvent();

        public ComputerPhaseStep[] ComputerPhaseSteps;

        public enum Phase { Computer, PlayerTurn, PreTurn }

        private static TimeManager _instance = null;
        public static TimeManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("TimeManager singleton was called without TimeManager being set up (check that TimeManager is in the scene)");
                return _instance;
            }
            private set { _instance = value; }
        }

        private int IndexOfCurrentlyRunningStep;
        private bool IsFirstComputerPhase;

        private void Awake()
        {
            Assert.IsNull(_instance, "TimeManager singleton is already set. (check there is only one TimeManager in the scene)");
            Instance = this;

            if (ComputerPhaseSteps.Length == 0) Debug.Log($"TimeManager does not provided with any computer phase workers. maybe you meant to?");

            foreach (ComputerPhaseStep step in ComputerPhaseSteps)
                Assert.IsNotNull(step, "TimeManager must not be given steps which are null");

            IndexOfCurrentlyRunningStep = -1;
        }

        private void Start()
        {
            HandleOnInitGame();
        }

        public void HandleOnInitGame()
        {
            CurrentTime = PointInTime.GenerateFirstPointInTime();
            CurrentPhase = Phase.Computer;
            IsFirstComputerPhase = true;
            if (DebugMode) Debug.Log($"TimeManager HandleOnInitGame set time to {CurrentTime} and phase to {CurrentPhase}.");

            // call all computer phase workers, by calling the first and then waiting it to finish to call next, etc. 
            StartComputerPhaseSteps();
        }

        private void StartComputerPhaseSteps()
        {
            // call all computer phase workers 
            IndexOfCurrentlyRunningStep = 0;
            ComputerPhaseStep nextStep = ComputerPhaseSteps[IndexOfCurrentlyRunningStep];
            if (DebugMode) Debug.Log($"TimeManager calling {nextStep.name}.");
            nextStep.OnFinishProcessing.AddListener(HandleComputerPhaseStepCompleted);
            nextStep.StartProcessingForComputerPhase(IsFirstComputerPhase);
        }

        // called when each computer phase step finishes
        public void HandleComputerPhaseStepCompleted() {
            // stop listening for finished step to finish
            ComputerPhaseSteps[IndexOfCurrentlyRunningStep].OnFinishProcessing.RemoveListener(HandleComputerPhaseStepCompleted);
            IndexOfCurrentlyRunningStep += 1;
            if (IndexOfCurrentlyRunningStep < ComputerPhaseSteps.Length) { // if there's another step, start it and wait
                ComputerPhaseStep nextStep = ComputerPhaseSteps[IndexOfCurrentlyRunningStep];
                nextStep.OnFinishProcessing.AddListener(HandleComputerPhaseStepCompleted);
                if (DebugMode) Debug.Log($"TimeManager calling {nextStep.name}");
                nextStep.StartProcessingForComputerPhase(IsFirstComputerPhase);
            } else { // if there's no more steps, finish the computer phase
                IsFirstComputerPhase = false;
                FinishComputerPhase();
            }
        }

        private void FinishComputerPhase() {
            if(CurrentPhase != Phase.Computer) throw new Exception($"TimeManager.FinishComputerPhase called when in {CurrentPhase} phase");
            CurrentPhase = Phase.PreTurn; 
            if (DebugMode) Debug.Log($"TimeManager phase now {CurrentPhase}");
            OnStartPreTurn.Invoke();
        }

        public void FinishPreTurnPhase() {
            if (CurrentPhase != Phase.PreTurn) throw new Exception($"TimeManager.FinishPreTurnPhase called when in {CurrentPhase} phase");
            CurrentPhase = Phase.PlayerTurn; 
            if (DebugMode) Debug.Log($"TimeManager phase now {CurrentPhase}");
            OnStartPlayerTurn.Invoke();
        }

        public void FinishPlayerTurnPhase() {
            if (CurrentPhase != Phase.PlayerTurn) throw new Exception($"TimeManager.FinishPlayerTurnPhase called when in {CurrentPhase} phase");
            CurrentPhase = Phase.Computer;
            if (DebugMode) Debug.Log($"TimeManager phase now {CurrentPhase}");

            // start computer phase work
            CurrentTime = CurrentTime.GenerateNext();
            if (DebugMode) Debug.Log($"TimeManager time now {CurrentTime}");
            StartComputerPhaseSteps();
        }

    }
}
