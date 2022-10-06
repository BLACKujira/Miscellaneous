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
        public Color32 Color => GetColor(color.ToString());
        public DateTime SendTime => CitrusTime.UnixTimeToDateTime(sendTime);
        public int poolType;
        public string midHash;
        public string dmid;
        public int weight;

        public uint color;
        public long sendTime;

        public override bool Equals(object obj)
        {
            if (obj is Danmaku)
            {
                Danmaku danmaku = (Danmaku)obj;
                return
                    time == danmaku.time &&
                    type == danmaku.type &&
                    size == danmaku.size &&
                    Color.r == danmaku.Color.r &&
                    Color.g == danmaku.Color.g &&
                    Color.b == danmaku.Color.b &&
                    SendTime == danmaku.SendTime &&
                    poolType == danmaku.poolType &&
                    midHash.Equals(danmaku.midHash) &&
                    dmid.Equals(danmaku.dmid) &&
                    content.Equals(danmaku.content);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return dmid.GetHashCode();
        }

        public Danmaku(string content, string parameters)
        {
            this.content = content;

            string[] paramArray = parameters.Split(',');
            time = float.Parse(paramArray[0]);
            type = int.Parse(paramArray[1]);
            size = int.Parse(paramArray[2]);
            color = uint.Parse(paramArray[3]);

            sendTime = long.Parse(paramArray[4]);
            poolType = int.Parse(paramArray[5]);
            midHash = paramArray[6];
            dmid = paramArray[7];
            weight = int.Parse(paramArray[8]);
        }

        private static Color32 GetColor(string color)
        {
            string colorHex = uint.Parse(color).ToString("x6");
            if (colorHex.Length != 6)
                throw new ColorFormatException();
            byte color_r = Convert.ToByte(colorHex.Substring(0, 2), 16);
            byte color_g = Convert.ToByte(colorHex.Substring(2, 2), 16);
            byte color_b = Convert.ToByte(colorHex.Substring(4, 2), 16);
            Color32 color32 = new Color32(color_r, color_g, color_b, 255);
            return color32;
        }

        public Danmaku(Bilibili.Community.Service.Dm.V1.DanmakuElem danmakuElem)
        {
            content = danmakuElem.Content;
            time = (float)danmakuElem.Progress/1000;
            type = danmakuElem.Mode;
            size = danmakuElem.Fontsize;
            color = danmakuElem.Color;
            sendTime = danmakuElem.Ctime;
            poolType = danmakuElem.Pool;
            midHash = danmakuElem.MidHash;
            dmid = danmakuElem.IdStr;
            weight = danmakuElem.Weight;
        }

        public Bilibili.Community.Service.Dm.V1.DanmakuElem ToDanmakuElem()
        {
            Bilibili.Community.Service.Dm.V1.DanmakuElem danmakuElem = new Bilibili.Community.Service.Dm.V1.DanmakuElem();
            danmakuElem.Content = content;
            danmakuElem.Progress = (int)(time*1000);
            danmakuElem.Mode = type;
            danmakuElem.Fontsize = size;
            danmakuElem.Color = color;
            danmakuElem.Ctime = sendTime;
            danmakuElem.Pool = poolType;
            danmakuElem.MidHash = midHash;
            danmakuElem.IdStr = dmid;
            danmakuElem.Weight = weight;
            return danmakuElem;
        }


        [Serializable]
        public class ColorFormatException : Exception
        {
            public ColorFormatException() : base("弹幕颜色格式错误") {}
            public ColorFormatException(string message) : base(message) { }
            public ColorFormatException(string message, Exception inner) : base(message, inner) { }
            protected ColorFormatException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}