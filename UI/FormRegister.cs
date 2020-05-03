using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CaterBll;
using CaterModel;

namespace UI
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        MemberInfoBll MIB = new MemberInfoBll();
        MemberTypeInfoBll MemTypeBll = new MemberTypeInfoBll();
        List<MemberTypeInfo> memTypeList = null;
        List<string> memTypeTitleList = new List<string>();
        List<int> memTypeIdList = new List<int>();
        //加载
        private void FormRegister_Load(object sender, EventArgs e)
        {
            //绑定查询的下拉列表
            memTypeList = MemTypeBll.GetTypeList();
            foreach(MemberTypeInfo memType in memTypeList)
            {
                memTypeTitleList.Add(memType.MTypeTitle);
                memTypeIdList.Add(memType.Id);
            }
            this.RegisterMemberType.DataSource = memTypeTitleList;
        }

        //注册（员工注册由店主添加）
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            MemberInfo MI = new MemberInfo();
            MI.MName = Convert.ToString(this.textMName.Text.Trim());
            MI.MPwd = Convert.ToString(this.textMPwd.Text.Trim());
            MI.MPhone = Convert.ToString(this.textMPhone.Text.Trim());
            MI.MTypeId = (memTypeList.Where(p => p.MTypeTitle.Equals(this.RegisterMemberType.SelectedItem.ToString()))).ToList()[0].Id;
            if(string.Empty.Equals(MI.MName) || string.Empty.Equals(MI.MPwd) || string.Empty.Equals(MI.MPhone) || string.Empty.Equals(MI.MTypeId))
            {
                MessageBox.Show("请填写完整信息");
                return;
            }
            if (MIB.GetByName(MI.MName) != null)
            {
                MessageBox.Show("您已注册");
                return;
            }
            switch(MI.MTypeId)
            {
                case 1: MessageBox.Show("请充值20元"); break;
                case 2: MessageBox.Show("请充值30元"); break;
                case 3: MessageBox.Show("请充值40元"); break;
                default:   return;
            }
            if (MIB.Add(MI)) 
            {
                MessageBox.Show("注册成功，您可以登录了");
                this.Close();
            }
        }
    }
}
