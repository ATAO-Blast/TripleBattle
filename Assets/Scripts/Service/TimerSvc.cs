using System;
using UnityEngine;
namespace TripleBattle
{
    public class TimerSvc : MonoBehaviour
    {
        private static TimerSvc instance;
        private PETimer pt;
        public static TimerSvc Instance { get { return instance; } }
        public void InitSvc()
        {
            pt = new PETimer();
            pt.SetLog(info =>
            {
                Debug.Log(info);
            });
            instance = this;
        }
        private void Update()
        {
            pt.Update();
        }
        public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
        {
            return pt.AddTimeTask(callback, delay, timeUnit, count);
        }
        public void DeleteTimeTask(int tid)
        {
            pt.DeleteTimeTask(tid);
        }
        public bool ReplaceTimeTask(int tid, Action<int> callback, float delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
        {
            return pt.ReplaceTimeTask(tid, callback, delay, timeUnit, count);
        }

        public int AddFrameTask(Action<int> callback, int delay, int count = 1)
        {
            return pt.AddFrameTask(callback, delay, count);
        }
        public void DeleteFrameTask(int tid)
        {
            pt.DeleteFrameTask(tid);
        }
        public bool ReplaceFrameTask(int tid, Action<int> callback, int delay, int count = 1)
        {
            return pt.ReplaceFrameTask(tid, callback, delay, count);
        }
        public void ResetTask()
        {
            pt.Reset();
        }
        public int GetYear()
        {
            return pt.GetYear();
        }
        public int GetMonth()
        {
            return pt.GetMonth();
        }
        public int GetDay()
        {
            return pt.GetDay();
        }
        public int GetWeek()
        {
            return pt.GetWeek();
        }
        public DateTime GetLocalDateTime()
        {
            return pt.GetLocalDateTime();
        }
        public double GetMillisecondsTime()
        {
            return pt.GetMillisecondsTime();
        }
        public string GetLocalTimeStr()
        {
            return pt.GetLocalTimeStr();
        }
    }
}