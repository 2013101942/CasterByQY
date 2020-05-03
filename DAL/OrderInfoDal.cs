using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using CaterCommon;
using CaterModel;

namespace CaterDal {
    /// <summary>
    /// 订单 数据层
    /// </summary>
    public partial class OrderInfoDal
    {
        /// <summary>
        /// 查看此桌号的当前账单
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public List<int> GetOrderIdByTableId(int tableId)
        {
            string sql = "SELECT Id FROM orderinfo where tId=@tableid and Oispay=0";
            SqlParameter p = new SqlParameter("@tableid", tableId);
            return SQLHelper.ExecuteScalarList<int>(sql, p);
        }

        /// <summary>
        /// 获取订单详情列表
        /// 菜品的编号
        /// 菜品的名称
        /// 数量
        /// 价格
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderInfo> GetDetailOrder(int tableId)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            string sql = @"select distinct DI.id, 
                                            DI.DTitle, 
                                            OI.OMoney DPrice, 
                                            OI.Oamount DNumber
                                        from OrderInfo OI, DishInfo DI where OI.Did = DI.id and  OI.Mid = @MId and OI.Tid = @TId and OI.OIsPay = 0";
            paramList.Add(new SqlParameter("@MId", GlobalVariable.userId));
            paramList.Add(new SqlParameter("@TId", tableId));
            var list=SQLHelper.ExecuteScalarList< OrderInfo>(sql, paramList.ToArray());
            return list;
        }

        /// <summary>
        /// 获取订单总金额
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public decimal GetTotalMoneyByTableId(int TableId)
        {
            string sql = @"	SELECT SUM(OI.ODiscount) 
	                                        FROM orderinfo AS OI
	                                                    INNER JOIN dishinfo AS di
	                                                    ON OI.Did=di.id
	                                        WHERE OI.Tid=@TableId and OI.Mid = @Mid and OI.OIsPay = 0";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("@TableId", TableId));
            ps.Add(new SqlParameter("@Mid", GlobalVariable.userId));
            object obj = SQLHelper.ExecuteScalar(sql, ps.ToArray());
            if (obj == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// 下订单
        /// </summary>
        /// <returns></returns>
        public bool insertOrder(string tableId, string Did)
        {
            decimal oMoney = decimal.Zero;
            decimal disCount = decimal.Zero;
            string sql = string.Empty;
            List<SqlParameter> ps = new List<SqlParameter>();
            bool SuccessFlag = true;
            try 
            { 
                //查询是否已经存在此菜品
                sql = @"select id from OrderInfo where Mid = @Mid and Tid = @Tid and Did = @Did and OIsPay = 0";
                ps.Add(new SqlParameter("@Mid", GlobalVariable.userId));
                ps.Add(new SqlParameter("@Tid", tableId));
                ps.Add(new SqlParameter("@Did", Did));
                var orderInfoId = SQLHelper.ExecuteScalar(sql, ps.ToArray());
                //查原本金额
                sql = @"select DPrice from DishInfo where id = @Did";
                oMoney = Convert.ToDecimal(SQLHelper.ExecuteScalar(sql, ps.ToArray()));
                //查折扣金额
                sql = @"select MTypeDiscount from MemberTypeInfo where id = (select MTypeId from MemberInfo where id = @Mid)";
                disCount = Convert.ToDecimal(SQLHelper.ExecuteScalar(sql, ps.ToArray())) * Convert.ToDecimal(oMoney);
                //下订单(点餐, 默认点一份)
                if(orderInfoId == null)
                {
                    sql = @"insert into OrderInfo(Mid, Tid, Odate, OMoney, OIsPay, Did, ODiscount, Oamount) values(@Mid, @Tid, GETDATE(), @oMoney, 0, @Did, @Odiscount, 1);";
                }
                else
                {
                    sql = @"update OrderInfo set Oamount = Oamount+1 where Mid = @Mid and Tid = @Tid and Did = @Did and OIsPay = 0;";
                }
                ps.Add(new SqlParameter("@oMoney", oMoney));
                ps.Add(new SqlParameter("@Odiscount", disCount));
                //更新餐桌状态
                sql += @"UPDATE TableInfo SET tIsFree=1 WHERE Id=@tableId;";
                ps.Add(new SqlParameter("@tableId", tableId));
                SQLHelper.ExecuteNonQuery(sql, ps.ToArray());
            }catch(System.Exception ex)
            {
                SuccessFlag = false;
                Debug.WriteLine("[insertOrder ERROR:]"+ex.Message);
            }
            return SuccessFlag;
        }

        /// <summary>
        /// 删除订单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteDetailById(int tableId, int Id)
        {
            string sql = "DELETE FROM orderInfo WHERE Mid = @Mid and Tid = @Tid and OIsPay = 0 and Did = @Did";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("@Mid", GlobalVariable.userId));
            ps.Add(new SqlParameter("@Tid", tableId));
            ps.Add(new SqlParameter("@Did", Id));
            
            return SQLHelper.ExecuteNonQuery(sql, ps.ToArray());
        }

        /// <summary>
        /// 支付账单
        /// </summary>
        /// <param name="isUseMoney">是否使用余额</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="payMoney">支付金额</param>
        /// <param name="orderid">订单Id</param>
        /// <param name="discount">折扣</param>
        /// <returns></returns>
        public int Pay(bool isUseMoney,int memberId,decimal payMoney,List<int> orderIdList)
        {
            //创建数据库的链接对象
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQL_server_yqConnectionString"].ConnectionString))
            {
                int result = 0;
                //由数据库链接对象创建事务
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                //创建command对象
                SqlCommand cmd=new SqlCommand();
                cmd.Connection = conn;
                //将命令对象启用事务
                cmd.Transaction = tran;
                //执行各命令
                string sql = "";
                SqlParameter[] ps;
                try
                {
                    //1、根据是否使用余额决定扣款方式，如果不是用余额就只需改订单状态和餐桌状态
                    if (isUseMoney)
                    {
                        //使用余额
                        sql = @"UPDATE MemberInfo SET mMoney=mMoney-@payMoney WHERE id=@mid";
                        ps = new SqlParameter[]
                        {
                            new SqlParameter("@payMoney", payMoney),
                            new SqlParameter("@mid", memberId)
                        };
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(ps);
                        result+=cmd.ExecuteNonQuery();
                    }
                    //2、将订单状态为OisPay=1
                    string sqlParamStr = string.Empty;
                    foreach (int sqlParam in orderIdList)
                    {
                        sqlParamStr += Convert.ToString(sqlParam) + ",";
                    }
                    sqlParamStr = sqlParamStr.Substring(0, sqlParamStr.Length - 1);
                    sql = @"UPDATE orderInfo SET OisPay=1 WHERE Id in ("+ sqlParamStr+")";
                    cmd.CommandText = sql;
                    result += cmd.ExecuteNonQuery();

                    //3、将餐桌状态TIsFree=1
                    sql = @"UPDATE tableInfo SET tIsFree=1 WHERE Id in (SELECT tId FROM orderinfo WHERE id in ("+sqlParamStr+")) and IsDelete = 0";
                    cmd.CommandText = sql;
                    result += cmd.ExecuteNonQuery();
                    //提交事务
                    tran.Commit();
                }
                catch(System.Exception ex)
                {
                    result = 0;
                    Debug.WriteLine("[Pay ERROR:]"+ex.Message);
                    //回滚事务
                    tran.Rollback();
                }
                return result;
            }
        }
        //客户手动修改数量
        public int UpdateCountById(int tableId, int Did, int count)
        {
            string sql = @"update OrderInfo set Oamount = @Oamount,
                                                                 ODate = GETDATE(), 
                                                                 OMoney = @Oamount*(select DPrice from DishInfo where Id = @Did), 
                                                                 ODiscount = (select DPrice from DishInfo where Id = @Did)*(select MTypeDiscount from MemberTypeInfo MTI, MemberInfo MI where MI.MTypeId = MTI.id and MI.id = @Mid)*@Oamount
                                         where Mid = @Mid and Tid = @Tid and OIsPay = 0 and Did = @Did;";
            SqlParameter[] ps =  {
                                 new SqlParameter("@Oamount", count),
                                 new SqlParameter("@Mid", GlobalVariable.userId),
                                 new SqlParameter("@Tid", tableId),
                                 new SqlParameter("@Did", Did)
                                 };
            return Convert.ToInt32(SQLHelper.ExecuteNonQuery(sql, ps));
        }

        public decimal getConsumMoney(List<int> orderIdList)
        {
            string sqlParamStr = string.Empty;
            foreach (int sqlParam in orderIdList) 
            {
                sqlParamStr += Convert.ToString(sqlParam) + ",";
            }
            sqlParamStr = sqlParamStr.Substring(0,sqlParamStr.Length-1);
            string sql = @"select sum(OMoney) from OrderInfo where id in ("+ sqlParamStr +")";
            return Convert.ToDecimal(SQLHelper.ExecuteScalar(sql));
        }

        public decimal getWillPayMoney(List<int> orderIdList)
        {
            string sqlParamStr = string.Empty;
            foreach (int sqlParam in orderIdList)
            {
                sqlParamStr += Convert.ToString(sqlParam) + ",";
            }
            sqlParamStr = sqlParamStr.Substring(0, sqlParamStr.Length - 1);
            string sql = @"select sum(ODiscount) from OrderInfo where id in (" + sqlParamStr + ")";
            return Convert.ToDecimal(SQLHelper.ExecuteScalar(sql));
        }

        public decimal getBalance()
        {
            string sql = @"select MMoney from MemberInfo where id = @userId";
            SqlParameter p = new SqlParameter("@userId", GlobalVariable.userId);
            return Convert.ToDecimal(SQLHelper.ExecuteScalar(sql, p));
        }

        public string getMemberTypeTitle()
        {
            string sql = @"select MTI.MTypeTitle from MemberInfo MI, MemberTypeInfo MTI where MI.MTypeId = MTI.id and MI.id = @userId";
            SqlParameter p = new SqlParameter("@userId", GlobalVariable.userId);
            return Convert.ToString(SQLHelper.ExecuteScalar(sql, p));
        }
    }
}
