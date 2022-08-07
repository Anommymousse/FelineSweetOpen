using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ConvertLanguage : MonoBehaviour
{
    public TMP_FontAsset ChineseFontAsset;
    public TMP_FontAsset EnglishFontAsset;
    int _currentWidth=50;
    
    public enum Languages
    {
        English,
        Chinese,
        French,
        Icelandic,
        Martian
    };

    public Dictionary<string, string> LanguageConversion;
    Languages _currentLanguage;
    bool _dictionaryReady = false;

    void Start()
    { 
      //  ChangeLanguage(Languages.Chinese);
      SetUpDictionary(Languages.English);
    }

    public int GetCurrentWidth() => _currentWidth;

    public Languages GetCurrentLanguage()
    {
        return _currentLanguage;
    }

    public TMP_FontAsset GetCurrentFont()
    {
        if (_currentLanguage == Languages.Chinese)
            return ChineseFontAsset;
        if (_currentLanguage == Languages.English)
            return EnglishFontAsset;
        return EnglishFontAsset;
    }

    public void ChangeLanguage(Languages newLanguage)
    {
        SetUpDictionary(newLanguage);
        _currentLanguage = newLanguage;
        
    }

    //This setup is sub-optimal given that each language requires conversion from the others leading to 2n lists.
    void SetUpDictionary(Languages newLanguage)
    {
        LanguageConversion = new Dictionary<string, string>();

        if (newLanguage == Languages.English)
            _currentWidth = 40;
        
        if (newLanguage == Languages.Chinese)
        {
            _currentLanguage = newLanguage;
            LanguageConversion.Add("feline sweet", "甜美可爱的猫咪");
            LanguageConversion.Add("start", "开始");
            LanguageConversion.Add("continue", "继续");
            LanguageConversion.Add("credits", "鸣谢");
            LanguageConversion.Add("options", "选项");
            LanguageConversion.Add("quit", "退出");
            LanguageConversion.Add("exit", "退出");
            LanguageConversion.Add("quit game", "退出");
            LanguageConversion.Add("master", "主音量");
            LanguageConversion.Add("sfx", "声音");
            LanguageConversion.Add("music", "音乐");
            LanguageConversion.Add("audio", "音频");
            LanguageConversion.Add("controls", "控制");
            LanguageConversion.Add("video", "视频");
            LanguageConversion.Add("fullscreen", "全屏");
            LanguageConversion.Add("low", "低");
            LanguageConversion.Add("medium", "中");
            LanguageConversion.Add("high", "高");
            LanguageConversion.Add("editor", "编辑");
            LanguageConversion.Add("keyboard", "键盘");
            LanguageConversion.Add("gamepad", "游戏盘");
            LanguageConversion.Add("jump", "跳跃");
            LanguageConversion.Add("magic", "魔法");
            LanguageConversion.Add("build", "构建");
            LanguageConversion.Add("left", "左");
            LanguageConversion.Add("right", "右");
            LanguageConversion.Add("Last but not least you for playing the game", "最后，感谢您玩游戏");
            LanguageConversion.Add("easy", "简单");
            LanguageConversion.Add("mediumdifficulty", "中等");
            LanguageConversion.Add("hard", "困难");
            LanguageConversion.Add("masterdiff", "大师");
            LanguageConversion.Add("custom", "自定义");
            LanguageConversion.Add("difficulty", "难度");
            LanguageConversion.Add("infinite lives", "无限生命");
            LanguageConversion.Add("suggested age", "建议年龄");
            LanguageConversion.Add("kitten", "小猫");
            LanguageConversion.Add("feline", "猫");
            LanguageConversion.Add("catlike", "像猫的");
            LanguageConversion.Add("nine lives", "九条命");
            LanguageConversion.Add("pick a difficulty", "选择一个难度");
            LanguageConversion.Add("pick a cat", "选择一只猫");
            LanguageConversion.Add(
                "help -: Currency, you collect stars as you play, each star can only counts once, each unlockable cat costs 9",
                "帮助 -：货币，你可以在游戏的过程中收集星星，每颗星星只能算一次，每颗可解锁成本为9的猫");
            LanguageConversion.Add("close", "关闭");
            LanguageConversion.Add("world", "世界");
            LanguageConversion.Add("level", "等级");
            LanguageConversion.Add("level editor","等级编辑");
            LanguageConversion.Add("start editor","开始编辑");
            LanguageConversion.Add("play!", "玩");
            LanguageConversion.Add("level target", "等级目标");
            LanguageConversion.Add("score", "分数");
            LanguageConversion.Add("highscore", "高分");
            LanguageConversion.Add("stars", "星星");
            LanguageConversion.Add("cakes", "蛋糕");
            LanguageConversion.Add("cake", "蛋糕");
            LanguageConversion.Add("auto", "自动");
            LanguageConversion.Add("save", "保存");
            LanguageConversion.Add("load", "加载");
            LanguageConversion.Add("player start", "玩家开始");
            LanguageConversion.Add("angel", "天使");
            LanguageConversion.Add("book", "书籍");
            LanguageConversion.Add("key", "钥匙");
            LanguageConversion.Add("door", "门");
            LanguageConversion.Add("random", "随机的");
            LanguageConversion.Add("clear", "清除");
            LanguageConversion.Add("test", "测试");
            LanguageConversion.Add("paws menu", "<wiggle>暂停菜单</wiggle>");
            LanguageConversion.Add("resume", "恢复");
            _currentWidth = 70;
        }
        _dictionaryReady = true;
    }

    
    public string ConvertString(string keyString)
    {
        string rv = " ";
        string value = keyString;

        
        if (keyString == "resume")
        {
            Debug.Log($"<color=green> Keystring = {keyString} </color>");
            Debug.Log("STMP");
        }
        
        if (_dictionaryReady)
        {
            if (LanguageConversion == null)
            {
                Debug.Log($"<color=red> NULL! for language {_dictionaryReady} </color>");
            }
            else if (LanguageConversion.TryGetValue(keyString, out value))
            {
                rv = value;
                if (keyString == "resume")
                {
                    Debug.Log($" {keyString} returned {value}");
                }
            }
            else
            {
                Debug.Log($" >{keyString}< not found in list");
            }
        }
        else
        {
            Debug.Log($" ready ? {LanguageConversion?.Count}");
            Debug.Log($"<color=red> directory not ready!  {_dictionaryReady} </color>");
        }

        if (_currentLanguage == Languages.English) rv = keyString;

        return rv;
    }
    
    

}
