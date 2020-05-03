using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterCommon;
using CaterModel;

namespace CaterDal
{
    public partial class MemberTypeInfoDal
    {
        //根据会员类型名称查询会员类型编号
        public int getMemberTypeId(string memberTypeName) 
        {
            string sql = "select Id from MemberTypeInfo where MTypeTitle = @MTypeTitle";
            SqlParameter  sqlParam= new SqlParameter("@MTypeTitle", memberTypeName);
            DataTable memberTypeIdDt = SQLHelper.GetDataTable(sql, sqlParam);
            return (int)memberTypeIdDt.Rows[0]["Id"];
        }
        //查询未删除数据
        public List<MemberTypeInfo> GetList()
        {
            //查询未删除的数据
            string sql = "select * from memberTypeInfo where IsDelete=0";
            //执行查询返回表格
            DataTable dt = SQLHelper.GetDataTable(sql);
            //定义集合对象
            List<MemberTypeInfo> list=new List<MemberTypeInfo>();

            //遍历表格，将数据转存到集合中
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MemberTypeInfo()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    MTypeTitle = row["mTypetitle"].ToString(),
                    MTypeDiscount = Convert.ToDecimal(row["mTypediscount"])
                });
            }

            //返回集合
            return list;
        } 

        //添加
        public int Insert(MemberTypeInfo mti)
        {
            //构造insert语句
            string sql = "insert into MemberTypeInfo(mTypetitle,mTypediscount,isDelete) values(@title,@discount,0)";
            //为sql语句构造参数
            SqlParameter[] ps =
            {
                new SqlParameter("@title",mti.MTypeTitle), 
                new SqlParameter("@discount",mti.MTypeDiscount)
            };
            //执行
            return SQLHelper.ExecuteNonQuery(sql, ps);
        }

        //修改
        public int Update(MemberTypeInfo mti)
        {
            //构造update语句
            string sql = "update memberTypeInfo set mTypetitle=@title,mTypediscount=@discount where Id=@id";
            //为语句构造参数
            SqlParameter[] ps =
            {
                new SqlParameter("@title",mti.MTypeTitle), 
                new SqlParameter("@discount",mti.MTypeDiscount), 
                new SqlParameter("@id",mti.Id)
            };
            //执行
            return SQLHelper.ExecuteNonQuery(sql, ps);
        }

        //删除
        public int Delete(int id)
        {
            //进行逻辑删除的sql语句
            string sql = "update memberTypeInfo set IsDelete=1 where Id=@id";
            //参数
            SqlParameter p=new SqlParameter("@id",id);
            //执行并返回受影响行数
            return SQLHelper.ExecuteNonQuery(sql, p);
        }

        public List<MemberTypeInfo> GetTypeList()
        {
            string sql = @"select * from MemberTypeInfo where IsDelete = 0";
            return SQLHelper.ExecuteScalarList<MemberTypeInfo>(sql);
        }
    }
}
