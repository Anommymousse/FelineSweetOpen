using System.Collections;
using System.Collections.Generic;
using DarkTonic.PoolBoss;
using UnityEngine;

public class EnemyStaticFirePrefab : MonoBehaviour
{
    public Sprite nonFireModeSprite;
    public Sprite fireModeSprite;
    public string possBossProjectileName;
    public float fireInitialDelay = 0.0f;
    public float howOftenToFire = 4.0f;
    public float howLongChangeInSpriteLasts = 0.25f;
    public Vector2 shootDirection = Vector2.left;
    float _timeCounterSinceLastFired = 0.0f;
    
    SpriteRenderer _displayedSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        InitialEnemySetup();
    }

    void InitialEnemySetup()
    {
        _displayedSpriteRenderer = GetComponent<SpriteRenderer>();
        _timeCounterSinceLastFired -= fireInitialDelay;
        _displayedSpriteRenderer.sprite = nonFireModeSprite;
    }

    // Update is called once per frame
    void Update()
    {
        _timeCounterSinceLastFired += Time.deltaTime;
        if (_timeCounterSinceLastFired > howOftenToFire)
        {
            _timeCounterSinceLastFired = 0.0f;
            StartCoroutine(FireTheProjectile());
        }
    }

    IEnumerator FireTheProjectile()
    {
        yield return null;
        _displayedSpriteRenderer.sprite = fireModeSprite;
        Transform projectileRef = PoolBoss.SpawnInPool(possBossProjectileName,transform.position,Quaternion.identity);
        var projectileComponent = projectileRef.GetComponent<MoveConstantSpeed>();
        projectileComponent.SetDirection(shootDirection);
        StartCoroutine(ResetSpriteTimer());
    }

    IEnumerator ResetSpriteTimer()
    {
        float timer = 0.0f;
        while (timer < howLongChangeInSpriteLasts)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        _displayedSpriteRenderer.sprite = nonFireModeSprite;
    }
}
