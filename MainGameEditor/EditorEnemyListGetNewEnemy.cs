using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorEnemyListGetNewEnemy : MonoBehaviour
{
    public List<Sprite> EnemyList;
    public Image _buttonImage;
    int _enemyIndex;

    public void GetPreviousEnemy()
    {
        _enemyIndex--;
        if (_enemyIndex < 0) _enemyIndex = EnemyList.Count - 1;
        _buttonImage.sprite = EnemyList[_enemyIndex];
    }

    public void GetNextEnemy()
    {
        _enemyIndex++;
        if (_enemyIndex > (EnemyList.Count-1)) _enemyIndex = 0;
        _buttonImage.sprite = EnemyList[_enemyIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemyIndex = 0;
        _buttonImage.sprite = EnemyList[_enemyIndex];
    }
}
