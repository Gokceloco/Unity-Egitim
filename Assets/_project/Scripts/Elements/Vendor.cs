using System;
using UnityEngine;
using DG.Tweening;

public class Vendor : MonoBehaviour
{
    public GameObject dialogueBox;

    private Vector3 _playerDirection;

    private bool _isDialogueActive;

    public GameObject dialogue1;
    public GameObject dialogue2;

    private DialogueState _dialogueState;

    private void Start()
    {
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (_isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (_dialogueState == DialogueState.Dialogue1)
                {
                    dialogue1.SetActive(false);
                    dialogue2.SetActive(true);
                }                
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerDirection = transform.position - other.transform.position;
            ShowDialogueBox();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        HideDialogueBox();
    }

    private void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);

        dialogueBox.transform.DOKill();
        dialogueBox.transform.localScale = Vector3.zero;
        dialogueBox.transform.DOScale(.44f,  .2f).SetEase(Ease.OutBack);

        var lookPos = dialogueBox.transform.position + _playerDirection;
        dialogueBox.transform.LookAt(lookPos);

        _isDialogueActive = true;
    }

    private void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
        _isDialogueActive = false;
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        _dialogueState = DialogueState.Dialogue1;

        dialogue1.SetActive(true);
        dialogue2.SetActive(false);
    }
}
public enum DialogueState
{
    Dialogue1,
    Dialogue2a,
    Dialogue2b,
}