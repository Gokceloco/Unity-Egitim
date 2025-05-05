using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Player _player;

    public Transform doorLeaf;

    public bool isOpened;

    public bool isLocked;

    private void Update()
    {
        if (_player != null && (_player.transform.position - transform.position).magnitude > 5)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        doorLeaf.DOKill();
        doorLeaf.DOLocalRotate(new Vector3(0,-90,0), .3f);
        isOpened = true;
    }

    public void CloseDoor()
    {
        doorLeaf.DOKill();
        doorLeaf.DOLocalRotate(new Vector3(0, 0, 0), .3f);
        isOpened = false;
    }

   

    public void DoorIntereacted(Player player)
    {
        _player = player;
        if (isOpened)
        {
            CloseDoor();
        }
        else
        {
            if (!isLocked)
            {
                OpenDoor();
            }
            else
            {
                if (player.haveKey)
                {
                    OpenDoor();
                }
                else
                {
                    _player.gameDirector.messageUI.ShowMessage("DOOR IS LOCKED, FIND THE KEY");
                }
            }
        }
    }
}
