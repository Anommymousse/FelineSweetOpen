using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorClearTileset : MonoBehaviour
{
   public Tilemap nonhidden;
   public Tilemap hidden;

   public void ClearAllTiles()
   {
      for (int x = -11; x < 5; x++)
      {
         for (int y = -10; y < 3; y++)
         {
            Vector3Int postion = Vector3Int.zero;
            postion.x = x;
            postion.y = y;
            nonhidden.SetTile(postion, null);
            hidden.SetTile(postion,null);
         }
      }
      
      nonhidden.RefreshAllTiles();
      hidden.RefreshAllTiles();
   }


   void Update()
   {
      if (LevelLoader.runningTestMode == false)
      {
         gameObject.GetComponent<Button>().interactable = true;
      }
      else
      {
         gameObject.GetComponent<Button>().interactable = false;
      }
   }
}
