using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] float _XRotationRate = 13.0f;
    [SerializeField] float _YRotationRate = -27.0f;
    Vector3 _positionOriginal;

    void Start()
    {
        _positionOriginal = GetComponent<Transform>().position;// transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation;
        newRotation.x = Time.deltaTime * _XRotationRate;//Mathf.Sin(Time.deltaTime);
        newRotation.y = Time.deltaTime * _YRotationRate;//Mathf.Cos(Time.deltaTime);
        newRotation.z = 0;
        transform.Rotate(newRotation);

        //newPosition.y = _positionOriginal.y + Mathf.Cos(Time.deltaTime) * 2.0f;
        //newPosition.z = _positionOriginal.z + Mathf.Cos(Time.deltaTime) * 1.0f;
        //transform.SetPositionAndRotation(newPosition, Quaternion.identity);

        /*addxyz.x = UnityEngine.Random.Range(1.0f, 10.2f);
        addxyz.y = UnityEngine.Random.Range(1.0f, 10.15f);
        addxyz.z = 0;
                
        Quaternion quaternion = transform.localRotation;
        Vector3 angles3d = quaternion.eulerAngles;
        angles3d += addxyz;
        quaternion.eulerAngles.Set(angles3d.x, angles3d.y, angles3d.z);
        transform.SetPositionAndRotation(_positionOriginal, quaternion);*/

    }
}
