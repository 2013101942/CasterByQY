using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CaterBll;
using CaterModel;
using CaterCommon;

namespace CaterUI {
    /// <summary>
    /// 结账窗口
    /// </summary>
    public partial class FormOrderPay : Form
    {
        public FormOrderPay()
        {
            InitializeComponent();
        }
        private OrderInfoBll oiBll=new OrderInfoBll();
        private List<int> orderIdList;
        public event Action Refresh;

        private void FormOrderPay_Load(object sender, EventArgs e)
        {
            //获取传递过来的订单编号
            orderIdList = this.Tag  as List<int>;

            gbMember.Enabled = false;
            //会员类别
            this.lblTypeTitle.Text = oiBll.getMemberTypeTitle();
            //账户余额
            this.lblMoney.Text = Convert.ToString(oiBll.getBalance());
            //消费金额
            this.lblPayMoney.Text = Convert.ToString(oiBll.getConsumMoney(orderIdList));
            //应收金额
            this.lblPayMoneyDiscount.Text = Convert.ToString(oiBll.getWillPayMoney(orderIdList));
            //折扣金额
            this.lblDiscount.Text = Convert.ToString(Convert.ToDecimal(this.lblPayMoney.Text.Trim()) - Convert.ToDecimal(this.lblPayMoneyDiscount.Text.Trim()));
        }

        private void cbkMember_CheckedChanged(object sender, EventArgs e)
        {
            gbMember.Enabled = cbkMember.Checked;
        }

        private void LoadMember()
        {
            //根据会员编号和会员电话进行查询
            if(string.Empty.Equals(txtId.Text.Trim()) || string.Empty.Equals(txtPhone.Text.Trim()))
            {
                MessageBox.Show("请填写会员信息");
                return;
            }
            if(!GlobalVariable.userId.Equals(txtId.Text.Trim()) || !GlobalVariable.phone.Equals(txtPhone.Text.Trim()))
            {
                MessageBox.Show("会员信息有误");
                return;
            }
        }

        private void txtId_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        private void btnOrderPay_Click(object sender, EventArgs e)
        {
            //1、根据是否使用余额决定扣款方式
            //2、将订单状态为OIsPay=1
            //3、将餐桌状态TIsFree=1
            //是会员，扣余额
            if (cbkMember.Checked && oiBll.Pay(cbkMoney.Checked, int.Parse(txtId.Text.Trim()), Convert.ToDecimal(lblPayMoneyDiscount.Text.Trim()), orderIdList))
            {
                MessageBox.Show("买单成功");
                Refresh();
                this.Close();
            }
            //不是会员
            else if (!cbkMember.Checked && oiBll.Pay(false, int.Parse(txtId.Text.Trim()), Convert.ToDecimal(lblPayMoneyDiscount.Text.Trim()), orderIdList))
            {
                MessageBox.Show("买单成功");
                Refresh();
                this.Close();
            }
            else
            {
                MessageBox.Show("买单失败");
            }

        }
        //暂不结账
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
