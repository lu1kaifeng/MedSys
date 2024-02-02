using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = MedSys.med;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Windows.Ink;
using FastMember;

namespace MedSys
{
    public partial class MainWindowViewModel
    {
        public string EnumerableToOrClauses(IEnumerable<string> list,string columnName)
        {
            string where = " (";
            foreach (var item in list)
            {
                where += "("+columnName+"=N\'"+item+"\') or ";
            }
            return where.Substring(0,where.Length-3)+") ";
        }
        public Task<PlotDataArgs> Query()
        {
            
                List<SqlParameter> paramList = new List<SqlParameter>();
            
                DateTime fromDate = FromDate;
                DateTime toDate = ToDate;
                string queryString = "";
                string queryStringSelect =
            "SELECT * from dbo.med where ";
                queryString += TimeTypeEntry + "> @fromDate and "+TimeTypeEntry +" < @toDate";
                if (TimeRangeEntry == "本周")
                {
                    fromDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
                }else if(TimeRangeEntry == "本月")
                {
                    fromDate = DateTime.Now.AddMonths(-1);
                }
                else if (TimeRangeEntry == "本季度")
                {
                    fromDate = DateTime.Now.AddMonths(-3);
                }
                else if (TimeRangeEntry == "本半年度")
                {
                    fromDate = DateTime.Now.AddMonths(-6);
                }
                else if (TimeRangeEntry == "本年度")
                {
                    fromDate = DateTime.Now.AddYears(-1);
                }else if( TimeRangeEntry == "上周")
                {
                    fromDate = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek + 7));
                    toDate = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek));
                }
                else if (TimeRangeEntry == "上月")
                {
                    fromDate = DateTime.Now.AddMonths(-((int)DateTime.Now.Month + 2));
                    toDate = DateTime.Now.AddMonths(-((int)DateTime.Now.Month + 1));
                }
                else if (TimeRangeEntry == "上季度")
                {
                    fromDate = DateTime.Now.AddMonths(-6);
                    toDate = DateTime.Now.AddMonths(-3);
                    //toDate = DateTime.Now.AddMonths(-((int)DateTime.Now.Month + 1));
                }
                else if (TimeRangeEntry == "上半年度")
                {
                    fromDate = DateTime.Now.AddMonths(-12);
                    toDate = DateTime.Now.AddMonths(-6);
                    //toDate = DateTime.Now.AddMonths(-((int)DateTime.Now.Month + 1));
                }
                else if (TimeRangeEntry == "上一年度")
                {
                    fromDate = DateTime.Now.AddMonths(-24);
                    toDate = DateTime.Now.AddMonths(-12);
                    //toDate = DateTime.Now.AddMonths(-((int)DateTime.Now.Month + 1));
                }
            paramList.Add(new SqlParameter("@fromDate",fromDate));
            paramList.Add(new SqlParameter("@toDate", toDate));
            if (SelectedTabIndex == 0)
            {
                if (MedName != string.Empty)
                {
                   
                        queryString += " and "+ MedNameTypeEntry + " like @medName";
                    
                    if (MedNameTypeExactOrContain)
                    {
                        paramList.Add(new SqlParameter("@medName", MedName));
                    }
                    else
                    {
                        paramList.Add(new SqlParameter("@medName", "%" + MedName + "%"));
                    }
                }
                if (ManufacturerName != string.Empty)
                {
                    queryString += " and 持有人或生产厂家 like @manufacturerName";
                    if (ManufacturerQueyTypeExactOrContain)
                    {
                        paramList.Add(new SqlParameter("@manufacturerName", ManufacturerName));
                    }
                    else
                    {
                        paramList.Add(new SqlParameter("@manufacturerName", "%" + ManufacturerName + "%"));
                    }
                }
            }
            else
            {
                var filteredMedName = this.MedNameList.Where(e=>e.Content != string.Empty);
                if (filteredMedName.Count() != 0 ) queryString+= " and ( "+string.Join(" or ", filteredMedName.Select((m)=>m.ToWhereClause()))+" ) ";
                var filteredManuName = ManufacturerNameList.Where(e => e.Content != string.Empty);
                if (filteredManuName.Count() != 0) queryString += " and ( " + string.Join(" or ", filteredManuName.Select((m) => m.ToWhereClause("持有人或生产厂家"))) + " ) ";
            }      
            
            if(ApprovalNo != string.Empty)
            {
                queryString += " and 批准文号 like @approvalNo";
                paramList.Add(new SqlParameter("@approvalNo", ApprovalNo));
            }

            if ( MedBatchNo != string.Empty)
            {
                queryString += " and 生产批号 like @medBatchNo";
                paramList.Add(new SqlParameter("@medBatchNo", MedBatchNo));
            }

            if (AdverseEffectName != string.Empty)
            {
                queryString += " and 系统不良反应术语 like @adverseEffectName";
                paramList.Add(new SqlParameter("@adverseEffectName", "%" + AdverseEffectName + "%"));
            }
            if(AdverseEffectResultTypeEntry != string.Empty)
            {
                queryString += " and 不良反应结果 like @adverseEffectResultTypeEntry";
                paramList.Add(new SqlParameter("@adverseEffectResultTypeEntry", "%" + AdverseEffectResultTypeEntry + "%"));
            }
            if(ReportSubject.Count != 0)
            {
                queryString += " and "+EnumerableToOrClauses(ReportSubject,"报告单位类别");
            }
            
            if(InfoSourceEntry != string.Empty)
            {
                queryString += " and 报告来源 like @infoSource";
                paramList.Add(new SqlParameter("@infoSource", "%" + InfoSourceEntry + "%"));
            }
            if(ReportEstimateEntry != string.Empty)
            {
                queryString += " and 是否非预期 like @reportEst";
                paramList.Add(new SqlParameter("@reportEst", "%" + ReportEstimateEntry + "%"));
            }
            if(ReportNoFrom != string.Empty)
            {
                queryString += " and 报告表编码 like @reportId";
                paramList.Add(new SqlParameter("@reportId",  ReportNoFrom + "%"));
            }
            if (IsDomesticEntry != string.Empty)
            {
                queryString += " and 是否境外报告 like @reportDomestic";
                paramList.Add(new SqlParameter("@reportDomestic", "%" + IsDomesticEntry + "%"));
            }
            if(DosageForm != string.Empty)
            {
                queryString += " and 剂型 like @dosageForm";
                paramList.Add(new SqlParameter("@dosageForm", DosageForm));
            }
            if (DeliveryEntry != string.Empty)
            {
                queryString += " and 给药途径 like @delivery";
                paramList.Add(new SqlParameter("@delivery", DeliveryEntry));
            }
            if (PatientName != string.Empty)
            {
                queryString += " and 患者姓名 like @patient";
                paramList.Add(new SqlParameter("@patient", PatientName));
            }
            if (SexEntry != string.Empty)
            {
                queryString += " and 性别 like @sex";
                paramList.Add(new SqlParameter("@sex", SexEntry));
            }
            if(AgeFrom != string.Empty)
            {
                queryString += " and 年龄 >= @ageF";
                paramList.Add(new SqlParameter("@ageF", int.Parse(AgeFrom)));
            }
            if (AgeTo != string.Empty)
            {
                queryString += " and 年龄 <= @ageT";
                paramList.Add(new SqlParameter("@ageT", int.Parse(AgeTo)));
            }
            if(ReportUnitName != string.Empty)
            {
                queryString += " and 报告单位名称 like @reportUnit";
                paramList.Add(new SqlParameter("@reportUnit", ReportUnitName));
            }
            if (HospitalName != string.Empty)
            {
                queryString += " and 医疗机构名称 like @reportHos";
                paramList.Add(new SqlParameter("@reportHos", HospitalName));
            }
            if (PreexistingCondition != string.Empty)
            {
                queryString += " and 疾病名称 like @preexist";
                paramList.Add(new SqlParameter("@preexist", PreexistingCondition));
            }
            if (MedReason != string.Empty)
            {
                queryString += " and 治疗适应症术语 like @reason";
                paramList.Add(new SqlParameter("@reason", "%"+MedReason+"%"));
            }
            if (CommentStateCityCommented)
            {
                queryString += " and 市评价人姓名 not like N'' ";
            }
            if (CommentStateProvinceCommented)
            {
                queryString += " and 省评价人姓名 not like N'' ";
            }
            return Task.Run(() =>
            {
                var args = new PlotDataArgs( " where "+ queryString, paramList.ToArray());
                var query = new medEntities().meds.SqlQuery(queryStringSelect + " " + queryString, args.ParamList);
                args.BackingData = query.ToList();
                return args;
            });
        }

        private PlotDataArgs _dataList;
        public PlotDataArgs DataList
        {
            get
            {
                return _dataList;
            }
            set { _dataList = value; OnPropertyChanged(); }
        }

    }

}
