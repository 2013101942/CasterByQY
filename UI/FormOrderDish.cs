using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CaterBll;
using CaterModel;

namespace CaterUI {
    /// <summary>
    /// 点菜窗口
    /// </summary>
    public partial class FormOrderDish : Form
    {
        public FormOrderDish()
        {
            InitializeComponent();
        }
        OrderInfoBll oiBll = new OrderInfoBll();
        //load事件会默认传参数（一个是控件对象，一个是事件参数）
        private void FormOrderDish_Load(object sender, EventArgs e)
        {
            LoadDishType();
            LoadDishInfo();

            LoadDetailList();
        }

        private void LoadDishInfo()
        {
            //拼接查询条件 
            Dictionary<string,string> dic=new Dictionary<string, string>();
            if (txtTitle.Text.ToString().Trim() != "")
            {
                dic.Add("dchar",txtTitle.Text);
            }
            if (ddlType.SelectedValue.ToString().Trim() != "全部")
            {
                dic.Add("DTypeTitle", ddlType.SelectedValue.ToString());
            }

            //查询菜品，显示到表格中
            DishInfoBll diBll=new DishInfoBll();
            dgvAllDish.AutoGenerateColumns = false;
            dgvAllDish.DataSource = diBll.GetList(dic);
        }

        private void LoadDishType()
        {
            DishTypeInfoBll dtiBll=new DishTypeInfoBll();
            var list=dtiBll.GetList();

            list.Insert(0,new DishTypeInfo()
            {
                Id = 0,
                DTypeTitle = "全部"
            });
            List<string> dishTypelist = new List<string>();
            foreach(DishTypeInfo dishType in list)
            {
                dishTypelist.Add(dishType.DTypeTitle);
            }
            ddlType.DataSource = dishTypelist;
        }

        private void LoadDetailList()
        {
            //Tag是桌子编号
            int TableId = Convert.ToInt32(this.Tag);
            dgvOrderDetail.AutoGenerateColumns = false;
            dgvOrderDetail.DataSource = oiBll.GetDetailOrder(TableId);
            GetTotalMoneyByOrderId();
        }

        private void GetTotalMoneyByOrderId()
        {
            int tableId = Convert.ToInt32(this.Tag);
            lblMoney.Text = oiBll.GetTotalMoneyByTableId(tableId).ToString();
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void dgvAllDish_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //双击点菜
            //订单编号
            string orderId = Convert.ToString(this.Tag);

            //菜单编号 
            string dishId = Convert.ToString(dgvAllDish.Rows[e.RowIndex].Cells[0].Value);

            //执行点菜操作
            if (oiBll.insertOrder(orderId, dishId))
            {
                //点菜成功
                MessageBox.Show("下单成功");
                LoadDetailList();
            }
        }
        //客户手动修改数量
        private void dgvOrderDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                //停止编辑数量列，才进行更改
                //获取桌号
                int tableId = Convert.ToInt32(this.Tag);
                //获取修改的行
                var row = dgvOrderDetail.Rows[e.RowIndex];
                //获取菜品编号
                int Did = Convert.ToInt32(row.Cells[0].Value);
                //获取数量
                int count = Convert.ToInt32(row.Cells[2].Value);
                //更新操作(更新数量、价格等)
                oiBll.UpdateCountByOid(tableId, Did, count);

                //重新计算总价
                GetTotalMoneyByOrderId();
            }
        }
        //点击下单点菜
        private void btnOrder_Click(object sender, EventArgs e)
        {
            //插入一条订单记录(this.Tag是桌子编号)
            string tableId = Convert.ToString(this.Tag);
            //获取菜品编号(已设置不可选多行)
            string Did = Convert.ToString(dgvAllDish.SelectedRows[0].Cells[0].Value);
            //更新订单
            if (oiBll.insertOrder(tableId, Did))
            {
                MessageBox.Show("下单成功");
                LoadDetailList();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            //获取桌号
            int tableId = Convert.ToInt32(this.Tag);
            //获取菜品编号
            int oid = Convert.ToInt32(dgvOrderDetail.SelectedRows[0].Cells[0].Value);
            //执行删除
            if (oiBll.DeleteDetailById(tableId, oid))
            {
                LoadDetailList();
            }
        }
    }
}
