using UnityEngine;
using System.Collections.Generic;

public class HackNodesManager : MonoBehaviour
{
    [Header("UI Slots")]
    [SerializeField] private HackNodeUI hackSlot1;
    [SerializeField] private HackNodeUI hackSlot2;
    [SerializeField] private HackNodeUI hackSlot3;

    [Header("Other Systems")]
    [SerializeField] private DangerLevelUI dangerLevelUI;

    private int hackedNodeCount = 0;
    private int activeWorldNodeID = -1;
    private float activeNodeProgress = 0f;

    private HashSet<int> hackedWorldNodes = new HashSet<int>();

    // Beispiel: UpdateHackProgress(2, 0.45f) = Welt-Node 2 ist bei 45 %.
    public void UpdateHackProgress(int worldNodeID, float progress)
    {
        if (hackedNodeCount >= 3)
            return;

        if (hackedWorldNodes.Contains(worldNodeID))
            return;

        // Wenn noch kein Node aktiv ist, wird dieser Node zum aktiven Node.
        if (activeWorldNodeID == -1)
        {
            activeWorldNodeID = worldNodeID;
        }

        // Ein anderer Node kann nicht gehackt werden, solange der aktuelle Node noch nicht abgeschlossen ist.
        if (worldNodeID != activeWorldNodeID)
            return;

        activeNodeProgress = Mathf.Clamp01(progress);
        GetCurrentSlot().SetHackProgress(activeNodeProgress);

        if (activeNodeProgress >= 1f)
        {
            CompleteActiveNode();
        }
    }

    private void CompleteActiveNode()
    {
        HackNodeUI currentSlot = GetCurrentSlot();

        if (!currentSlot)
            return;

        currentSlot.CompleteHack();

        hackedWorldNodes.Add(activeWorldNodeID);
        hackedNodeCount++;

        if (dangerLevelUI)
        {
            dangerLevelUI.NodeHacked();
        }

        activeWorldNodeID = -1;
        activeNodeProgress = 0f;
    }

    private HackNodeUI GetCurrentSlot()
    {
        switch (hackedNodeCount)
        {
            case 0: return hackSlot1;
            case 1: return hackSlot2;
            case 2: return hackSlot3;
            default: return null;
        }
    }

    // Kann vom Gameplay-Code abgefragt werden, bevor ein Welt-Node Hack-Fortschritt erzeugt.
    public bool CanHackNode(int worldNodeID)
    {
        if (hackedNodeCount >= 3)
            return false;

        if (hackedWorldNodes.Contains(worldNodeID))
            return false;

        return activeWorldNodeID == -1 || activeWorldNodeID == worldNodeID;
    }

    // Kann vom Gameplay-Code abgefragt werden.
    // Ab 3 gehackten Nodes kann dort der Boss-Spawn ausgelöst werden.
    public int GetHackedNodeCount()
    {
        return hackedNodeCount;
    }
}


/*
HackNode-System:

- Insgesamt gibt es 5 Welt-Nodes.
- Sobald ein Node begonnen wird, wird dieser zum aktuell aktiven Node.
- Sein Hack-Fortschritt wird im aktuellen UI-Slot angezeigt.

Beispiel:
Node A wird begonnen.
→ UI Slot 1 füllt sich.

Spieler verlässt Node A bei z. B. 60 %.
→ Der Fortschritt bleibt bei 60 % gespeichert.

Spieler betritt Node B.
→ Node B bekommt keinen Fortschritt,
  solange Node A noch nicht abgeschlossen ist.

Spieler kehrt zu Node A zurück.
→ Hack-Fortschritt läuft von 60 % weiter bis 100 %.

Bei 100 %:
→ UI Slot 1 wird abgeschlossen.
→ Danger Level erhält +30.
→ Node A wird als abgeschlossen gespeichert
  und kann nicht erneut gezählt werden.

Danach darf einer der übrigen Nodes
(B, C, D oder E) begonnen werden.
→ Dieser verwendet UI Slot 2.

Nach insgesamt 3 verschiedenen,
erfolgreich gehackten Nodes:
→ Alle 3 UI-Slots sind abgeschlossen.
→ GetHackedNodeCount() gibt 3 zurück.
→ Der Gameplay-Code kann daraufhin den Boss spawnen.
*/