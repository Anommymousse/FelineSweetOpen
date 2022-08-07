using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpVersionNumber : MonoBehaviour
{
  public SetupSingleTextBox setupMenuBox;
  public VersionNumber.VersionNumber versionNumber;

  void Start()
  {
    setupMenuBox._menuText = $" Build {versionNumber.versionNumber} ";
    setupMenuBox.UpdateText();
    
  }
}
