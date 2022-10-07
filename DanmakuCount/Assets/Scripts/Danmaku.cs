using System;
using UnityEngine;

namespace CitrusDammakuCount
{
    public class Danmaku
    {
        public string content;

        public float time;
        public int type;
        public int size;
        public Color32 color;
        public DateTime sendTime;
        public int poolType;
        public string midHash;
        public long dmid;
        public int weight;

        readonly string getHashStr;

        public override bool Equals(object obj)
        {
            if (obj is Danmaku)
            {
                Danmaku danmaku = (Danmaku)obj;
                return danmaku.getHashStr.Equals(getHashStr) && danmaku.content.Equals(content);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return getHashStr.GetHashCode();
        }

        public Danmaku(string content, string parameters)
        {
            this.content = content;
            int lastToken = parameters.LastIndexOf(',');
            getHashStr = parameters.Substring(0,lastToken)+content;

            string[] paramArray = parameters.Split(',');
            time = float.Parse(paramArray[0]);
            type = int.Parse(paramArray[1]);
            size = int.Parse(paramArray[2]);

            string colorHex = int.Parse(paramArray[3]).ToString("x6");
            if (colorHex.Length != 6)
                Debug.Log(1);
            byte color_r = Convert.ToByte(colorHex.Substring(0, 2), 16);
            byte color_g = Convert.ToByte(colorHex.Substring(2, 2), 16);
            byte color_b = Convert.ToByte(colorHex.Substring(4, 2), 16);
            color = new Color32(color_r, color_g, color_b, 255);

            sendTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(paramArray[4])).DateTime.ToLocalTime();
            poolType = int.Parse(paramArray[5]);
            midHash = paramArray[6];
            dmid = long.Parse(paramArray[7]);
            weight = int.Parse(paramArray[8]);
        }
    }
}