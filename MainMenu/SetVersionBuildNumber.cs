using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class SetVersionBuildNumber : MonoBehaviour
    {
        public TMP_Text TMPText;
        public VersionNumber.VersionNumber versionObject;

        void Start()
        {
            TMPText.SetText($" {versionObject.mainNotes[0]} {versionObject.versionNumber} ");
        }
    }
}
