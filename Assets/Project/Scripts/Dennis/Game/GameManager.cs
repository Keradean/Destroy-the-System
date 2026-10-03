using System;
using Project.Scripts.Dennis.Nodes;
using UnityEngine;

namespace Project.Scripts.Dennis.Game
{
    // Hält den Spielzustand: ALARM, gehackte Nodes, Core, Boss, Sieg und Niederlage.
    // Andere Skripte melden sich hier, UI und Spawner hören auf die Events.
    public class GameManager : MonoBehaviour
    {
        public enum GameState { Playing, Won, Lost }

        [Header("ALARM")]
        [SerializeField] private float _alarmPerSecond = 1f;    // Anstieg pro Sekunde Hacken
        [SerializeField] private float _maxAlarm = 15f;         // 3 Hacks x 5 s HackingTime, später abstimmen

        [Header("Ablauf")]
        [SerializeField] private int _hacksForCore = 3;
        [SerializeField] private EndNode _endNode;               // öffnet sich, wenn der Boss besiegt ist

        private float _alarm;
        private int _hackedNodes;
        private bool _coreReached;

        public GameState State { get; private set; } = GameState.Playing;
        public float Alarm => _alarm;
        public float MaxAlarm => _maxAlarm;
        public int HackedNodes => _hackedNodes;
        public bool IsCoreUnlocked => _hackedNodes >= _hacksForCore;

        public event Action<float, float> OnAlarmChanged;   // aktuell, max
        public event Action<int, int> OnHacksChanged;       // gehackt, benötigt
        public event Action OnCoreUnlocked;
        public event Action OnCoreReached;                  // hier spawnt später der Boss
        public event Action OnWin;
        public event Action OnLose;

        private void Awake()
        {
            // Falls die Szene nach Sieg oder Niederlage neu geladen wird
            Time.timeScale = 1f;
        }

        // Wird von HackNode jeden Frame beim Hacken aufgerufen, mit Time.deltaTime
        public void AddAlarm(float hackSeconds)
        {
            if (State != GameState.Playing) return;
            if (_alarm >= _maxAlarm) return;

            _alarm = Mathf.Min(_alarm + hackSeconds * _alarmPerSecond, _maxAlarm);
            OnAlarmChanged?.Invoke(_alarm, _maxAlarm);
        }

        // Wird von HackNode aufgerufen, wenn ein Node fertig gehackt ist
        public void NodeHacked()
        {
            if (State != GameState.Playing) return;

            bool wasUnlocked = IsCoreUnlocked;
            _hackedNodes++;
            Debug.Log("Nodes gehackt: " + _hackedNodes + "/" + _hacksForCore);
            OnHacksChanged?.Invoke(_hackedNodes, _hacksForCore);

            if (wasUnlocked || !IsCoreUnlocked) return;
            Debug.Log("CORE ist jetzt aktiv");
            OnCoreUnlocked?.Invoke();
        }

        // Wird vom CoreNode aufgerufen, nur beim ersten Betreten
        public void CoreReached()
        {
            if (State != GameState.Playing) return;
            if (_coreReached) return;

            _coreReached = true;
            Debug.Log("CORE betreten, Boss kommt");   // TODO: SpawnManager hört auf OnCoreReached
            OnCoreReached?.Invoke();
        }

        // Wird aufgerufen, wenn der Boss tot ist
        public void BossDefeated()
        {
            if (State != GameState.Playing) return;

            Debug.Log("Boss besiegt, Ausgang offen");
            if (_endNode != null) _endNode.Open();
        }

        public void Win()
        {
            if (State != GameState.Playing) return;

            State = GameState.Won;
            Debug.Log("SIEG");
            Time.timeScale = 0f;
            OnWin?.Invoke();
        }

        public void Lose()
        {
            if (State != GameState.Playing) return;

            State = GameState.Lost;
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
            OnLose?.Invoke();
        }

        // Testmenü, solange HackNode und Boss noch nicht angebunden sind
        [ContextMenu("Test: Node gehackt")]
        private void TestNodeHacked() => NodeHacked();

        [ContextMenu("Test: ALARM +5")]
        private void TestAddAlarm() => AddAlarm(5f / _alarmPerSecond);

        [ContextMenu("Test: Boss besiegt")]
        private void TestBossDefeated() => BossDefeated();
    }
}
