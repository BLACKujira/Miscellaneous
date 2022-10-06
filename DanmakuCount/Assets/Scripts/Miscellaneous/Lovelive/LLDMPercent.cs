using CitrusDammakuCount.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitrusDammakuCount.Miscellaneous.Lovelive
{
    public class LLDMPercent : MonoBehaviour
    {
        public List<Line> lines;
        public float waitTime;
        
        [System.Serializable]
        public class Line
        {
            public List<MoveAndFade> objects;
        }

        private void Start()
        {
            StartCoroutine(Process());
        }

        IEnumerator Process()
        {
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return 1;
            }

            foreach (var line in lines)
            {
                foreach (var moveAndFade in line.objects)
                {
                    moveAndFade.TurnOn();
                }
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}