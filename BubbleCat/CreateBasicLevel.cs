using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateBasicLevel : MonoBehaviour
{
   public GameObject singleTunnel;
   public GameObject dualTunnel;
   public GameObject convergerTunnel;
   public GameObject divergerTunnel;

   public GameObject SpikeyBallPrefabRightObj;
   public GameObject SpikeyBallPrefabLeftObj;

   public GameObject GroundEffectPrefabObj;

   void LevelOne()
   {
      Vector3 currentPosition = Vector3.zero;
      var adjustedposition= singleTunnel.transform.position;
      Instantiate(singleTunnel, currentPosition, Quaternion.identity);
      AddSpikesSingleTunnel(currentPosition);

      currentPosition.z += 300.0f;
      Instantiate(singleTunnel, currentPosition, Quaternion.identity);
      AddSpikesSingleTunnel(currentPosition);
      
      currentPosition.z += 300.0f;
      Instantiate(divergerTunnel, currentPosition, Quaternion.identity);

      currentPosition.z += 300.0f;
      
      Instantiate(dualTunnel, currentPosition, Quaternion.identity);
      AddGroundEffectDualTunnelLeft(currentPosition);
      //AddSpikesDualTunnel(currentPosition);
      
      currentPosition.z += 300.0f;
      Instantiate(dualTunnel, currentPosition, Quaternion.identity);
      AddSpikesDualTunnel(currentPosition);
      
      currentPosition.z += 300.0f;
      Instantiate(convergerTunnel, currentPosition, Quaternion.identity);
      
      currentPosition.z += 300.0f;
      Instantiate(singleTunnel, currentPosition, Quaternion.identity);
      AddSpikesSingleTunnel(currentPosition);
      
   }

   void AddGroundEffectDualTunnelLeft(Vector3 currentPosition)
   {
      var tempposition = currentPosition;
      tempposition.y += 5.5f;
      var randomInt0and1 = Random.Range(0, 2);
      tempposition.x += 36.0f;
      Instantiate(randomInt0and1 == 0 ? GroundEffectPrefabObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      tempposition.x -= 36.0f;
      Instantiate(randomInt0and1 == 1 ? GroundEffectPrefabObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);

      tempposition = currentPosition;
      tempposition.y += 5.5f;
      tempposition.z -= 100.0f;
      randomInt0and1 = Random.Range(0, 2);
      tempposition.x += 36.0f;
      Instantiate(randomInt0and1 == 0 ? GroundEffectPrefabObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      tempposition.x -= 72.0f;
      Instantiate(randomInt0and1 == 1 ? GroundEffectPrefabObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      //Instantiate(SpikeyBallPrefabLeftObj, tempposition, Quaternion.identity);
      
      tempposition = currentPosition;
      tempposition.y += 5.5f;
      tempposition.z += 100.0f;
      randomInt0and1 = Random.Range(0, 2);
      tempposition.x += 36.0f;
      Instantiate(randomInt0and1 == 0 ? GroundEffectPrefabObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      tempposition.x -= 72.0f;
      Instantiate(randomInt0and1 == 1 ? GroundEffectPrefabObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      
   }
   
   void AddSpikesDualTunnel(Vector3 currentPosition)
   {
      var tempposition = currentPosition;
      tempposition.y += 5.5f;
      var randomInt0and1 = Random.Range(0, 2);
      tempposition.x += 36.0f;
      Instantiate(randomInt0and1 == 0 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      tempposition.x -= 36.0f;
      Instantiate(randomInt0and1 == 1 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);

      tempposition = currentPosition;
      tempposition.y += 5.5f;
      tempposition.z -= 100.0f;
      randomInt0and1 = Random.Range(0, 2);
      tempposition.x += 36.0f;
      Instantiate(randomInt0and1 == 0 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      tempposition.x -= 72.0f;
      Instantiate(randomInt0and1 == 1 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      //Instantiate(SpikeyBallPrefabLeftObj, tempposition, Quaternion.identity);
      
      tempposition = currentPosition;
      tempposition.y += 5.5f;
      tempposition.z += 100.0f;
      randomInt0and1 = Random.Range(0, 2);
      tempposition.x += 36.0f;
      Instantiate(randomInt0and1 == 0 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      tempposition.x -= 72.0f;
      Instantiate(randomInt0and1 == 1 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      
      //Instantiate(SpikeyBallPrefabLeftObj, tempposition, Quaternion.identity);

   }


   void AddSpikesSingleTunnel(Vector3 currentPosition)
   {
      var tempposition = currentPosition;
      tempposition.y += 5.5f;
      
      var randomInt0and1 = Random.Range(0, 2);
      Instantiate(randomInt0and1 == 0 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);

      tempposition = currentPosition;

      tempposition.y += 5.5f;
      tempposition.z -= 100.0f;
      randomInt0and1 = Random.Range(0, 2);
      Instantiate(randomInt0and1 == 0 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      //Instantiate(SpikeyBallPrefabLeftObj, tempposition, Quaternion.identity);
      
      tempposition = currentPosition;
      tempposition.y += 5.5f;
      tempposition.z += 100.0f;
      randomInt0and1 = Random.Range(0, 2);
      Instantiate(randomInt0and1 == 0 ? SpikeyBallPrefabRightObj : SpikeyBallPrefabLeftObj, tempposition,
         Quaternion.identity);
      
      //Instantiate(SpikeyBallPrefabLeftObj, tempposition, Quaternion.identity);

   }

   void Start()
   {
      LevelOne();
   }

   void OnEnable()
   {
      
   }
}
