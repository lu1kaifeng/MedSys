using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedSys
{
    public class Typing
    {
        private Typing() { }
        public static string[] TimeType = new string[] { "国家中心接收时间", "首次获知时间", "最后一次获知时间", "不良反应发生时间", "死亡时间" };
        public static string[] MedNameType = new string[] { "通用名称", "通用名称（不带剂型）", "品种" };
        public static string[] TimeRangeType = new string[] { "自定义", "本周", "本月", "本季度", "本半年度", "本年度", "上周", "上月", "上季度", "上半年度", "上一年度" };
        public static string[] AdverseEffectResultType = new string[] { string.Empty, "好转", "不详", "未好转", "有后遗症", "死亡", "痊愈" };
        public static string[] ReportEstimateType = new string[] { "全部", "预期（已知）", "非预期（新的）" };
        public static string[] IsDomesticType = new string[] { "全部", "境外报告", "境内报告" };
        public static string[] SexType = new string[] { "全部", "男", "女", "不详" };
        public static string[] ReportTypeType = new string[] { "全部", "严重", "一般" };
        public static string[] InfoSourceType = new string[] { string.Empty, "监管机构", "患者/亲友", "医疗机构", "经营企业", "文献", "研究", "项目", "其他" };
        public static string[] MedTypeType = new string[] { "全部", "怀疑用药", "合并用药" };
        public static string[] BaseMedType = new string[] { "基药2012", "基药2018", "第一怀疑药2012", "第一怀疑药2018" };
        public static string[] DeliveryType = new string[] { string.Empty, "膀胱内给药", "泵内注射", "鼻甲注射", "鼻饲", "臂丛麻醉", "表面麻醉", "冲洗", "骶管注射", "动脉给药", "封闭", "腹膜腔内给药", "腹膜腔内注射", "关节内给药", "管道喂养", "灌注", "含服", "肌内注射", "结膜下注射", "经鼻给药", "经耳给药", "经眼给药", "颈丛阻滞", "静脉滴注", "静脉注射", "局部给药", "局部浸润麻醉", "局部注射", "口服", "口腔/咽喉给药", "尿道给药", "皮下注射", "气管/支气管内给药", "前房内注射", "鞘内给药", "球后注射", "舌下给药", "术中栓塞", "漱口", "外用", "吸入给药", "心内注射", "胸膜腔内给药", "胸膜腔内注射", "血液透析", "眼球内给药", "羊膜腔内给药", "阴道给药", "硬膜外给药", "造瘘管注入", "直肠给药", "植入给药", "肿瘤内注射", "蛛网膜下腔给药", "注射", "椎管给药", "子宫颈给药", "子宫内给药", "牙科给药", "肝动脉灌注", "皮内注射", "不详" };
        public static string[] FocusTypeType = new string[] { string.Empty, "新冠肺炎防控品种","4 + 7集中采购品种","创新药品种","中药关注品种"};
    }
    
}
