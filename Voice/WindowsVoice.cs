using System.Diagnostics;
using UnityEngine;

namespace Voice
{
    public class WindowsVoice : MonoBehaviour
    {
        public static void Say(string text) {
            text = text.Replace("'", "").Replace("\"", "");
            var cmd = $"/c PowerShell -Command \"Add-Type –AssemblyName System.Speech; " + "$speak = New-Object System.Speech.Synthesis.SpeechSynthesizer; " + "$speak.Volume = 60;" + "$speak.SelectVoice('Microsoft Zira Desktop');" +
                      $"$speak.Speak('{text}');\"";
            var psi = new ProcessStartInfo("cmd.exe", cmd);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.ErrorDialog = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            var proc = Process.Start(psi);
            proc.WaitForExit();
            proc.Close();
        }
    }
}
