using UnityEngine;
using Project.Scripts.Dennis.Game;
using Project.Scripts.Philipp.Hacking;

public class HackNodesUI : MonoBehaviour
{
    [Header("UI Slots")]
    [SerializeField] private HackNodeUI hackSlot1;
    [SerializeField] private HackNodeUI hackSlot2;
    [SerializeField] private HackNodeUI hackSlot3;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    private int lastHackedNodeCount = 0;

    private void Awake()
    {
        if (!gameManager)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    private void Update()
    {
        if (!gameManager)
            return;

        UpdateHackProgress();
        UpdateCompletedNodes();
    }

    private void UpdateHackProgress()
    {
        if (!gameManager.ActiveHack)
            return;

        HackNode activeHack = gameManager.ActiveHack as HackNode;

        if (!activeHack)
            return;

        HackNodeUI currentSlot = GetCurrentSlot();

        if (currentSlot)
        {
            currentSlot.SetHackProgress(activeHack.HackProgress);
        }
    }

    private void UpdateCompletedNodes()
    {
        while (lastHackedNodeCount < gameManager.HackedNodes)
        {
            HackNodeUI completedSlot = GetSlot(lastHackedNodeCount);

            if (completedSlot)
            {
                completedSlot.CompleteHack();
            }

            lastHackedNodeCount++;
        }
    }

    private HackNodeUI GetCurrentSlot()
    {
        return GetSlot(gameManager.HackedNodes);
    }

    private HackNodeUI GetSlot(int index)
    {
        switch (index)
        {
            case 0: return hackSlot1;
            case 1: return hackSlot2;
            case 2: return hackSlot3;
            default: return null;
        }
    }
}