using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSys
{
    public class Typing
    {
        private Typing() { }
        public static string[] TimeType = new string[] { "国家中心接收时间", "首次获知时间", "最后一次获知时间", "不良反应发生时间", "死亡时间" };
        public static string[] MedNameType = new string[] { "通用名称", "通用名称（不带剂型）", "品种" };
        public static string[] TimeRangeType = new string[] { "自定义", "本周", "本月", "本季度", "本半年度", "本年度", "上周", "上月", "上季度", "上半年度", "上一年度" };
        public static string[] AdverseEffectResultType = new string[] {"", "好转", "不详", "未好转", "有后遗症", "死亡", "痊愈" };
}
    
}
