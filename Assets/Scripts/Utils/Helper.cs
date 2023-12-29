using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Key và Analytic strings để hết ở đây
    /// </summary>
    public static class Helper
    {
        public const string DataLevel = "DataLevel";
        public const string DataMaxLevelReached = "DataMaxLevelReached";
        public const string DataTypeSkin = "DataTypeSkin";
        public const string DataTypePet = "DataTypePet";
        public const string DataTypeHat = "DataTypeHat";
        public const string DataEquipHat = "DataEquipHat";
        public const string DataEquipSkin = "DataEquipSkin";
        public const string DataEquipPet = "DataEquipPet";
        public const string DataNumberWatchVideo = "DataNumberWatchVideo";
        public const string GOLD = "GOLD";

        public const string KEY = "KEY";
        public const string CurrentRewardEndGame = "CurrentRewardEndGame";
        public const string ProcessReceiveEndGame = "ProcessReceiveEndGame";

        public const string inter_end_game_lose = "inter_end_game_lose";
        public const string inter_end_game_win = "inter_end_game_win";

        public const string video_shop_general = "video_shop_{0}";
        public const string video_reward_end_game = "video_reward_end_game";
        public const string video_reward_chest_key = "video_reward_chest_key";
        public const string video_reward_lucky_wheel = "video_reward_lucky_wheel";
        public const string video_reward_revive = "video_reward_revive";
        public const string video_reward_x3_gold_end_game = "video_x3_gold_end_game";
        public const string video_reward_gift_box = "video_reward_gift_box";
        public const string PathLevelResources = "Wave/Wave";


        public const string SoundSetting = "SoundSetting";
        public const string MusicSetting = "MusicSetting";

        public static string FormatTime(int minute, int second, bool isSpaceSpecial = false)
        {
            StringBuilder sb = new StringBuilder();
            if (minute < 10)
            {
                sb.Append("0");
            }

            sb.Append(minute);
            if (isSpaceSpecial)
            {
                sb.Append("M");
                sb.Append(" ");
            }
            else
            {
                sb.Append(":");
            }

            if (second < 10)
            {
                sb.Append("0");
            }

            sb.Append(second);
            if (isSpaceSpecial)
            {
                sb.Append("S");
            }

            return sb.ToString();
        }

        public static int GetRandomGoldReward()
        {
            return Random.Range(100, 250);
        }

        public static bool CheckNewDay(string stringTimeCheck, bool isUnbiasedTime)
        {
            if (string.IsNullOrEmpty(stringTimeCheck))
            {
                return true;
            }

            try
            {
                DateTime timeNow = DateTime.Now;
                DateTime timeOld = DateTime.Parse(stringTimeCheck);
                DateTime timeOldCheck = new DateTime(timeOld.Year, timeOld.Month, timeOld.Day, 0, 0, 0);
                long tickTimeNow = timeNow.Ticks;
                long tickTimeOld = timeOldCheck.Ticks;

                long elapsedTicks = tickTimeNow - tickTimeOld;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                double totalDay = elapsedSpan.TotalDays;

                if (totalDay >= 1)
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }
    }

}