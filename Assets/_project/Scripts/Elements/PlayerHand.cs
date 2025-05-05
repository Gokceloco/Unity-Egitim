using DG.Tweening;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Player player;

    private InteractableBox _interactableBoxInRange;

    private InteractableBox _carryingBox;

    private Vector3 _offset;

    public Door _doorInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractableBox"))
        {
            _interactableBoxInRange = other.GetComponent<InteractableBox>();
        }
        if (other.CompareTag("Door"))
        {
            _doorInRange = other.GetComponentInParent<Door>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableBox"))
        {
            _interactableBoxInRange = null;
        }
        if (other.CompareTag("Door"))
        {
            _doorInRange = null;
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
        if (Input.GetKeyUp(KeyCode.E) && _doorInRange != null)
        {
            _doorInRange.DoorIntereacted(player);
        }
    }
}
