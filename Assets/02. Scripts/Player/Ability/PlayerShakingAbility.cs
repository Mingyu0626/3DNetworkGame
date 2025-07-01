using System.Collections;
using UnityEngine;

public class PlayerShakingAbility : PlayerAbility
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _strength;
    [SerializeField]
    private float _duration;

    public void Shake()
    {
        StartCoroutine(Shake_Coroutine());
    }

    protected override void DoAbility()
    {
    }

    private IEnumerator Shake_Coroutine()
    {
        float elapsedTime = 0f;

        Vector3 startPosition = _target.localPosition;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 randomPosition = Random.insideUnitSphere * _strength;
            randomPosition.y = startPosition.y;
            _target.localPosition = randomPosition;
            yield return null;
        }

        _target.localPosition = startPosition;
    }
}
