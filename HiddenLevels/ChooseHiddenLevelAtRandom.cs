using UnityEngine;
using static MyExtensions.MyExtensions;
namespace HiddenLevels
{
    public class ChooseHiddenLevelAtRandom : MonoBehaviour
    {
        static int EasyHiddenLevelMax=5;
        static int NormalHiddenLevelMax=1;
        static int HardHiddenLevelMax=1;
        
        static string _currentHiddenLevel;
        static string _nextLevel;
        static string _difficulty;
        static int _randomlevelchoosen;
        static int _storedLevelIn;

        public static string GetCurrentHiddenLevel() => _currentHiddenLevel;
        public static string GetNextLevel() => _nextLevel;
        public static string GetDifficulty() => "/Hidden/Hidden" + _difficulty;
        public static int GetHiddenLevelLevel() => _randomlevelchoosen;
        public static int GetNextLevelnum() => _storedLevelIn;

        public static string HiddenLevelCreation(int level,string difficulty)
        {
            StoreNextNonHiddenLevel(level,difficulty);
            return ChooseAHiddenLevel(level, difficulty);
        }

        //Privates....    
        static string MakeHiddenFilePath()
        {
            return Application.streamingAssetsPath + "/hidden/";
        }

        static void StoreNextNonHiddenLevel(int levelIn,string difficulty)
        {
            string nextLevelToGoto;
            _storedLevelIn = levelIn;
            
            if (difficulty.ToLower().Contains("custom"))
            {
                nextLevelToGoto = "Custom" + levelIn + ".txt";
                _nextLevel = nextLevelToGoto;
                return;
            }

            //Make level filename
            nextLevelToGoto = "Easy";
            if (levelIn > 29) nextLevelToGoto = "Normal";
            if (levelIn > 60) nextLevelToGoto = "Hard";
            _difficulty = nextLevelToGoto;
            
            nextLevelToGoto += levelIn + ".txt";
        
        
            //Where are the hidden levels. 
            var fullpathHiddenLevelChoosen = Application.streamingAssetsPath +"/"+ nextLevelToGoto;
        
            _nextLevel = fullpathHiddenLevelChoosen;
            
            Log($"<color=red> NEXT LEVEL = {_nextLevel} </color>");
        }
    
        public static string ChooseAHiddenLevel(int levelIn,string difficulty)
        {
            int randomlevel = 1;
            //Int random range is max exclusive
            randomlevel= Random.Range(0, EasyHiddenLevelMax) + 1;
            if(levelIn>30)
                randomlevel= Random.Range(0, NormalHiddenLevelMax) + 1;
            if(levelIn>60)
                randomlevel= Random.Range(0, HardHiddenLevelMax) + 1;

            _randomlevelchoosen = randomlevel;
            
            string rv = MakeHiddenFilePath() + "Hidden" + randomlevel + ".txt";
            
            Log($"<color=red> hidden level choosen = {rv} </color>");

            return rv;
        }
        
    }
}
