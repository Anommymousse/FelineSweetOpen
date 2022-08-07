using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerListForCharacterSelection : MonoBehaviour
{
    [SerializeField] List<Sprite> _sprites;

    public List<Sprite> GetList() => _sprites;

}
