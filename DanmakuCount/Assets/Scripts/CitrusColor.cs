using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitrusDammakuCount
{
    public static class CitrusColor
    {
        public static string ColorToHTML(Color32 color)
        {
            return $"#{color.r:x2}{color.g:x2}{color.b:x2}";
        }
    }
}