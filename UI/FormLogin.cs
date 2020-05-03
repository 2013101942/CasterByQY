using System;
using System.Windows.Forms;
using CaterBll;
using CaterModel;
using CaterCommon;
using UI;

namespace CaterUI {
    /// <summary>
    /// 登陆窗口
    /// </summary>
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        //点击退出
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //点击登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //获取用户输入的信息
            string name = txtName.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            //调用代码
            int type;
            var miBll=new ManagerInfoBll();
            var loginState = miBll.Login(name, pwd,out type);
            switch (loginState)
            {
                case LoginState.Ok:
                    var main=new FormMain();
                    main.Tag = type;//将员工级别传递过去
                    main.Show();
                    //将登录窗体隐藏
                    this.Hide();
                    break;
                case LoginState.NameError:
                    MessageBox.Show("该用户不存在");
                    break;
                case LoginState.PwdError:
                    MessageBox.Show("密码错误");
                    break;

            }
        }

        private void txtName_TextChanged(object sender, EventArgs e) {

        }

        private void txtPwd_TextChanged(object sender, EventArgs e) {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
        //点击注册
        private void btnRegister_Click(object sender, EventArgs e)
        {
            var fromRegister = new FormRegister();
            fromRegister.Show();
        }
    }
}
