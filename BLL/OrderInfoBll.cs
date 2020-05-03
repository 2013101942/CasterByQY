using System.Collections.Generic;
using CaterDal;
using CaterModel;

namespace CaterBll {
    /// <summary>
    /// 账单信息业务层
    /// </summary>
    public partial class OrderInfoBll
    {
        private OrderInfoDal oiDal=new OrderInfoDal();
        /// <summary>
        /// 查看此桌号的当前账单
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public List<int> GetOrderIdByTableId(int tableId)
        {
            return oiDal.GetOrderIdByTableId(tableId);
        }

        /// <summary>
        /// 更新订单中的菜品数量
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UpdateCountByOid(int tableId, int Did, int count)
        {
            return oiDal.UpdateCountById(tableId,Did, count) > 0;
        }

        /// <summary>
        /// 获得订单的详细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<OrderInfo> 
            GetDetailOrder(int TableId)
        {
            return oiDal.GetDetailOrder(TableId);
        }

        /// <summary>
        /// 获取订单总金额
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public decimal GetTotalMoneyByTableId(int tableId)
        {
            return oiDal.GetTotalMoneyByTableId(tableId);
        }

        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="tableId">餐桌编号</param>
        /// <param name="Did">菜品编号</param>
        /// <returns></returns>
        public bool insertOrder(string tableId, string Did)
        {
            return oiDal.insertOrder(tableId, Did);
        }

        /// <summary>
        /// 删除订单详细
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public bool DeleteDetailById(int tableId,int oid)
        {
            return oiDal.DeleteDetailById(tableId,oid) > 0;
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
        public bool Pay(bool isUseMoney, int memberId, decimal payMoney, List<int> orderIdList)
        {
            return oiDal.Pay(isUseMoney, memberId, payMoney, orderIdList) > 0;
        }

        public decimal getConsumMoney(List<int> orderIdList)
        {
            return oiDal.getConsumMoney(orderIdList);
        }

        public decimal getWillPayMoney(List<int> orderIdList)
        {
            return oiDal.getWillPayMoney(orderIdList);
        }

        public decimal getBalance()
        {
            return oiDal.getBalance();
        }

        public string getMemberTypeTitle()
        {
            return oiDal.getMemberTypeTitle();
        }
    }
}
