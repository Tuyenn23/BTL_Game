using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Unicorn
{
    /// <summary>
    /// Giảm GarbageCollector khi dùng Coroutine
    /// </summary>
    public static class Yielders
    {

        #region Wait for second

        static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(32);

        public static WaitForSeconds Get(float seconds)
        {
            seconds = (float) Math.Round(seconds, 2);
            if (!_timeInterval.ContainsKey(seconds))
            {
                _timeInterval.Add(seconds, new WaitForSeconds(seconds));
            }

            return _timeInterval[seconds];
        }

        #endregion

        #region wait for end of frame

        static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

        public static WaitForEndOfFrame EndOfFrame
        {
            get => endOfFrame;
        }

        #endregion
    }
}