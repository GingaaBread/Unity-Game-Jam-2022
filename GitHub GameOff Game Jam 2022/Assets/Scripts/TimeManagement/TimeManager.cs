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

        [HideInInspector] public static UnityEvent OnInitGame = new UnityEvent();
        [HideInInspector] public static UnityEvent OnStartPreTurn = new UnityEvent();
        [HideInInspector] public static UnityEvent OnStartPlayerTurn = new UnityEvent();
        [HideInInspector] public static UnityEvent OnEndPlayerTurn = new UnityEvent();

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

        private bool IsInitialized;

        private void Awake()
        {
            Assert.IsNull(_instance, "TimeManager singleton is already set. (check there is only one TimeManager in the scene)");
            Instance = this;

            if (ComputerPhaseSteps.Length == 0) Debug.Log($"TimeManager does not provided with any computer phase workers. maybe you meant to?");

            IsInitialized = false;

            TimeManager.OnInitGame.AddListener(HandleOnInitGame);
            TimeManager.OnEndPlayerTurn.AddListener(HandleComputerPhase);
        }
        
        // TODO: May be invoked somewhere else?
        private void Start()
        {
            OnInitGame.Invoke();
        }

        public void HandleOnInitGame()
        {
            IsInitialized = true;
            CurrentTime = PointInTime.GenerateFirstPointInTime();
            CurrentPhase = Phase.Computer;
            if (DebugMode) Debug.Log($"TimeManager received OnInitGame event, so set time to {CurrentTime} and phase to {CurrentPhase}.");
            // call all computer phase workers 
            if (DebugMode) Debug.Log($"TimeManager calling DoProcessingForComputerPhaseDuringGameInit() on {ComputerPhaseSteps.Length} steps...");
            for (int i = 0; i < ComputerPhaseSteps.Length; i++)
            {
                if (ComputerPhaseSteps[i] != null)
                {
                    if (DebugMode) Debug.Log($"TimeManager calling DoProcessingForComputerPhaseDuringGameInit() on {ComputerPhaseSteps[i].name}.");
                    ComputerPhaseSteps[i].DoProcessingForComputerPhaseDuringGameInit();
                }
            }
            FinishCurrentPhase();
        }

        public void HandleComputerPhase()
        {
            // increment time
            CurrentTime = CurrentTime.GenerateNext();
            if (DebugMode) Debug.Log($"TimeManager incremented time to {CurrentTime}.");
            // call all computer phase workers 
            if (DebugMode) Debug.Log($"TimeManager calling DoProcessingForComputerPhaseDuringGameInit() on {ComputerPhaseSteps.Length} steps...");
            for (int i = 0; i < ComputerPhaseSteps.Length; i++)
            {
                if (ComputerPhaseSteps[i] != null)
                {
                    if (DebugMode) Debug.Log($"TimeManager calling DoProcessingForComputerPhase() on {ComputerPhaseSteps[i].name}.");
                    ComputerPhaseSteps[i].DoProcessingForComputerPhase();
                }
            }
            FinishCurrentPhase();
        }

        public void FinishCurrentPhase()
        {
            if (!IsInitialized)
                throw new Exception("TimeManager's FinishCurrentPhase() was called but TimeManager was never initialized first (something should invoke TimeManager.OnInitGame event first)");

            switch (CurrentPhase)
            {
                case Phase.Computer: CurrentPhase = Phase.PreTurn; OnStartPreTurn.Invoke(); break;
                case Phase.PreTurn: CurrentPhase = Phase.PlayerTurn; OnStartPlayerTurn.Invoke(); break;
                case Phase.PlayerTurn: CurrentPhase = Phase.Computer; OnEndPlayerTurn.Invoke(); break;
                default: throw new Exception($"TimeManager never expected a phase {CurrentPhase} and doesn't know how to handle it");
            }

            if (DebugMode) Debug.Log($"TimeManager FinishCurentPhase called, so phase is now {CurrentPhase} and corresponding event was fired.");
        }

    }
}
