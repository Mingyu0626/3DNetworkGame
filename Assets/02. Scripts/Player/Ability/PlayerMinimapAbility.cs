using UnityEngine;

public class PlayerMinimapAbility : PlayerAbility
{
    private MinimapCamera _minimapCamera;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _playerAvaterSprite;

    [SerializeField]
    private Sprite _enemyAvaterSprite;



    protected override void Awake()
    {
        base.Awake();
        _minimapCamera = FindFirstObjectByType<MinimapCamera>();
        if (_minimapCamera != null && PhotonView.IsMine)
        {
            _minimapCamera.SetTarget(transform);
        }
        SetAvater();
    }

    protected override void DoAbility()
    {
    }

    private void SetAvater()
    {
        if (PhotonView.IsMine)
        {
            _spriteRenderer.sprite = _playerAvaterSprite;
        }
        else
        {
            _spriteRenderer.sprite = _enemyAvaterSprite;
        }
    }
}
