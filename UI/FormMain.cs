using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CaterBll;

namespace CaterUI {
    /// <summary>
    /// 主界面
    /// </summary>
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
        }
        //账单业务实体
        OrderInfoBll orderInfoBLL = new OrderInfoBll();

        //退出
        private void menuQuit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        //退出
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e) {
            //将当前应用程序退出，而不仅是关闭当前窗体
            Application.Exit();
        }

        private void FormMain_Load(object sender, EventArgs e) {
            //判断登录进来的员工的级别，以确定是否显示menuManager图标
            //type只可能是0,1,2
            int type = Convert.ToInt32(this.Tag);
            if( type == 0)
            {
                //如果是店员,则管理员菜单和餐桌不需要显示
                menuManagerInfo.Visible = false;
                tcHallInfo.Visible = false;
                menuOrder.Visible = false;
            }
            else if(type == 1)
            {
                //店长
                tcHallInfo.Visible = false;
                menuOrder.Visible = false;
            }
            else if(type == 2)
            {
                //顾客
                menuManagerInfo.Visible = false;
            }
            LoadHallInfo();
        }

        //加载所有的厅包信息
        private void LoadHallInfo() {
            //2.1、获取所有的厅包对象
            var hallInfoBll = new HallInfoBll();
            var list = hallInfoBll.GetList();

            //2.2、遍历集合，向标签页中添加信息
            tcHallInfo.TabPages.Clear();
            var tiBll = new TableInfoBll();
            foreach (var hallInfo in list) {
                //根据厅包对象创建标签页对象
                var tp = new TabPage(hallInfo.HTitle);
                //3.1、动态创建列表添加到标签页上
                var lvTableInfo = new ListView();
                //添加双击事件，完成开单功能
                lvTableInfo.DoubleClick += lvTableInfo_DoubleClick;

                //3.2、让列表使用图片
                lvTableInfo.LargeImageList = imageList1;
                lvTableInfo.Dock = DockStyle.Fill;
                tp.Controls.Add(lvTableInfo);
                //4.1、获取当前厅包对象的所有餐桌
                var dic = new Dictionary<string, string> {
                    { "hi.id", hallInfo.Id.ToString() }
                };
                var listTableInfo = tiBll.GetList(dic);
                //4.2、向列表中添加餐桌信息
                foreach (var ti in listTableInfo) {
                    var lvi = new ListViewItem(ti.TTitle, ti.TIsFree ? 0 : 1);
                    //后续操作需要用到餐桌编号，所以在这里存一下
                    lvi.Tag = ti.Id;

                    lvTableInfo.Items.Add(lvi);
                }

                //2.3、将标签页加入标签容器
                tcHallInfo.TabPages.Add(tp);
            }
        }

        //双击餐桌（sender是由控件内部传入的控件，双击的事件作为媒介，这里只要假设它就是底层已经传进来了）
        private void lvTableInfo_DoubleClick(object sender, EventArgs e) {
            //获取被点的餐桌项
            var lv1 = sender as ListView;
            var lvi = lv1.SelectedItems[0];
            //获取餐桌编号
            int tableId = Convert.ToInt32(lvi.Tag);

            if (lvi.ImageIndex == 0) {
                //3、更新项的图标为占用
                lv1.SelectedItems[0].ImageIndex = 1;
            }
            //4、打开点菜窗体
            FormOrderDish formOrderDish = new FormOrderDish();
            formOrderDish.Tag = lvi.Tag;
            //调用的是show()方法，底层执行的是load事件(可以是给属性和事件赋值)
            formOrderDish.Show();
        }

        private void menuManagerInfo_Click(object sender, EventArgs e) {
            FormManagerInfo formManagerInfo = FormManagerInfo.Create();//new FormManagerInfo();
            formManagerInfo.Show();
            formManagerInfo.Focus();//将当前窗体获得焦点
            //如果是最小化的，则恢复到正常状态
            formManagerInfo.WindowState = FormWindowState.Normal;
        }

        private void menuMemberInfo_Click(object sender, EventArgs e) {
            FormMemberInfo formMemberInfo = new FormMemberInfo();
            formMemberInfo.Show();
        }

        private void menuTableInfo_Click(object sender, EventArgs e) {
            FormTableInfo formTableInfo = new FormTableInfo();
            formTableInfo.Refresh += LoadHallInfo;
            formTableInfo.Show();
        }

        private void menuDishInfo_Click(object sender, EventArgs e) {
            FormDishInfo formDishInfo = new FormDishInfo();
            formDishInfo.Show();
        }
        //点击付款
        private void menuOrder_Click(object sender, EventArgs e) {
            //先找到选中的标签页，再找到listView,再找到选中的项，项中存储了餐桌编号，由餐桌编号查到订单编号
            var listView = tcHallInfo.SelectedTab.Controls[0] as ListView;
            if (listView.SelectedItems.Count < 1) {
                MessageBox.Show("请选择餐桌");
                return;
            }
            var lvTable = listView.SelectedItems[0];
            if (lvTable.ImageIndex == 0) {
                MessageBox.Show("餐桌还未使用，无法结账");
                return;
            }
            int tableId = Convert.ToInt32(lvTable.Tag);
            List<int> orderIdList = orderInfoBLL.GetOrderIdByTableId(tableId);

            //打开付款窗体
            FormOrderPay formOrderPay = new FormOrderPay();
            formOrderPay.Tag = orderIdList;
            formOrderPay.Refresh += LoadHallInfo;
            formOrderPay.Show();
        }
    }
}
