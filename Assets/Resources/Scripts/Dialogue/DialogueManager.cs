﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Manages dialogue. Dialogue is stored as a queue.
// If there is any dialogue in the queue, the one at the top is displayed.
// External scripts can add dialogue to the queue and iterate to the next dialogue in the queue.
// Dialogue is designed to display to a 2D canvas, which has a TextmeshProUGUI object

public class DialogueManager : MonoBehaviour
{
    public delegate void DoAction();
    // Queue of all currently-queued dialogue
    private Queue<Tuple<string, DoAction>> dialogQueue = new Queue<Tuple<string, DoAction>>();

    // Canvas the display text should be contained within
    public GameObject dialogueCanvas;

    // Whether to run the action  associated with the dialogue
    private bool InvokeAction = true;

    // Start is called before the first frame update
    void Start()
    {
        HideDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogQueued())
        {
            DisplayDialogue(dialogQueue.Peek());
        }
        else
        {
            HideDialogue();
        }
    }

    // Display the latest dialogue
    private void DisplayDialogue(Tuple<string, DoAction> dialogue)
    {
        dialogueCanvas.SetActive(true);
        dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.Item1;

        if (InvokeAction)
        {
            dialogue.Item2?.Invoke();
            InvokeAction = false;
        }

    }

    // Hide any dialogue
    private void HideDialogue()
    {
        dialogueCanvas.SetActive(false);
    }

    // Iterate to the next dialogue in the queue if there is one
    public void IterateDialogue()
    {
        if (DialogQueued())
        {
            dialogQueue.Dequeue();
            InvokeAction = true;
        }
    }

    // Add dialogue to the queue
    public void Enqueue(string dialogue, DoAction action = null)
    {
        dialogQueue.Enqueue(new Tuple<string, DoAction>(dialogue, action));
    }

    // Return if any dialogue should be dislayed
    public bool DialogQueued()
    {
        return dialogQueue.Count > 0;
    }

    // Completely clear all current dialogue
    public void resetQueue()
    {
        dialogQueue.Clear();
    }
}
