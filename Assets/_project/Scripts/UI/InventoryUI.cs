using DG.Tweening;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameDirector gameDirector;

    public CanvasGroup canvasGroup;

    public GameObject shotgunButton;

    public bool isShotgunAvailable;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, .2f);
        gameDirector.gameState = GameState.InventoryUI;
        if (!isShotgunAvailable)
        {
            shotgunButton.SetActive(false);
        }
        else
        {
            shotgunButton.SetActive(true);
        }
    }
    public void Hide()
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, .2f).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            gameDirector.gameState = GameState.GamePlay;
            });
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MachineGunButtonPressed()
    {
        gameDirector.player.weapon.ActivateMachineGun();
        Hide();
        gameDirector.inventoryUIVisible = false;
    }
    public void ShotGunButtonPressed()
    {
        gameDirector.player.weapon.ActivateShotgun();
        Hide();
        gameDirector.inventoryUIVisible = false;
    }
}
