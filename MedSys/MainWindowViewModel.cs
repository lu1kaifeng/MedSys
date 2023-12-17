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
        public List<med> Query()
        {
            
                List<SqlParameter> paramList = new List<SqlParameter>();
                DateTime fromDate = FromDate;
                DateTime toDate = ToDate;
                string queryString =
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
                if(MedName != string.Empty)
            {
                if(MedNameTypeEntry == "通用名称")
                {
                    queryString += " and 通用名称 like @medName";
                }
                if (MedNameTypeExactOrContain)
                {
                    paramList.Add(new SqlParameter("@medName", MedName));
                }
                else
                {
                    paramList.Add(new SqlParameter("@medName", "%" + MedName + "%"));
                }
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

            if (ManufacturerName != string.Empty)
            {
                queryString += " and 持有人/生产厂家 like @manufacturerName";
                if (ManufacturerQueyTypeExactOrContain)
                {
                    paramList.Add(new SqlParameter("@manufacturerName", ManufacturerName));
                }
                else
                {
                    paramList.Add(new SqlParameter("@manufacturerName", "%" + ManufacturerName + "%"));
                }
            }

            if (AdverseEffectName != string.Empty)
            {
                queryString += " and 不良反应-术语(原始术语) like @adverseEffectName";
                paramList.Add(new SqlParameter("@adverseEffectName", AdverseEffectName));
            }

            List<med> medList = new List<med>();
            using (SqlConnection connection = new medEntities().Database.Connection as SqlConnection)
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddRange(paramList.ToArray());
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                
                while (reader.Read())
                {    
                    medList.Add(ConvertToObject<med>(reader));
                }
         
            }
            return medList;

        }


        public T ConvertToObject<T>(SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }
    }

}
