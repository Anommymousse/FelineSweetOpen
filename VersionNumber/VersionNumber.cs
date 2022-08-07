using System.Collections.Generic;
using UnityEngine;

namespace VersionNumber
{
    [CreateAssetMenu(fileName = "VersionNumber", menuName = "ScriptableObjects/VersionNum", order = 1)]
    public class VersionNumber : ScriptableObject
    {
        public string versionNumber;
        public List<string> mainNotes;
        public List<string> bugfixesThisVersion;
    }
}
