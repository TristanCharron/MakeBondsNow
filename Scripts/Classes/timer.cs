using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace tools
{

    // Simple timer class I made that allow to make itween timers simply without using coroutines.
    public class timer
    {

        private static float length = 0;
        private static timer _instance;
        public static timer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new timer();
                    return _instance;
                }
                else
                    return null;
            }

        }




        public static void setTimer(GameObject gObject, float timeInSeconds, string functionAfterCompletion)
        {
            iTween.ValueTo(gObject, iTween.Hash("from", 0, "to", 0.1, "time", timeInSeconds, "onupdate", "changeTimer", "oncomplete", functionAfterCompletion, "loop", "none", "easeType", "easeOutQuint"));
        }

        static void changeTimer(float newValue)
        {

            length = newValue;

        }


    }

    public static class QuickSort
    {
        public static void Sort(ref List<Answer> list, int left, int right)
        {
            int i = left, j = right;
            Answer pivot = list[(left + right / 2)];

            while (i <= j)
            {
                while ((list[i].getScore() < pivot.getScore()))
                    i++;

                while ((list[j].getScore() > pivot.getScore()))
                    j--;


                if (i <= j)
                {
                    // Swap
                    Answer ans = list[i];
                    list[i] = list[j];
                    list[j] = ans;

                    i++;
                    j--;
                }
            }
            if (left < j)
                Sort(ref list, left, j);

            if (i < right)
                Sort(ref list, i, right);


        }


    }
}









