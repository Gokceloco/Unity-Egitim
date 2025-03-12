using DG.Tweening;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Player player;

    private InteractableBox _interactableBoxInRange;

    private InteractableBox _carryingBox;

    private Vector3 _offset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractableBox"))
        {
            _interactableBoxInRange = other.GetComponent<InteractableBox>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableBox"))
        {
            _interactableBoxInRange = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_interactableBoxInRange != null)
            {
                _carryingBox = _interactableBoxInRange;
                _offset = _carryingBox.transform.position - player.transform.position;
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            _carryingBox = null;            
        }
        if (_carryingBox)
        {
            _carryingBox.transform.position = player.transform.position + _offset;
        }
    }
}
