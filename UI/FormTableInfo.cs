using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CaterBll;
using CaterModel;
using System.Data;

namespace CaterUI {
    /// <summary>
    /// 餐桌管理窗口
    /// </summary>
    public partial class FormTableInfo : Form
    {
        public FormTableInfo()
        {
            InitializeComponent();
        }

        private TableInfoBll tiBll=new TableInfoBll();
        public event Action Refresh;

        private void FormTableInfo_Load(object sender, EventArgs e)
        {
            LoadSearchList();

            LoadList();
        }

        private void LoadList()
        {
            Dictionary<string,string> dic=new Dictionary<string, string>();
            if (ddlHallSearch.SelectedIndex > 0)
            {
                dic.Add("hi.hTitle", ddlHallSearch.SelectedValue.ToString());
            }
            if (ddlFreeSearch.SelectedIndex > 0)
            {
                string freeStatus = (ddlFreeSearch.SelectedValue.ToString() == "使用中")?"false":"true";
                dic.Add("ti.tIsFree",freeStatus);
            }

            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = tiBll.GetList(dic);
        }

        private void LoadSearchList()
        {
            HallInfoBll hiBll=new HallInfoBll();
            var list = hiBll.GetList();
            List<string> hallList = new List<string>();
            //搜索
            hallList.Add("-请选择-");
            foreach (HallInfo hall in list)
            {
                hallList.Add(hall.HTitle);
            }
            ddlHallAdd.DataSource = hallList;
            hallList.Clear();
            //添加、删除
            list.Insert(0,new HallInfo()
            {
                Id = 0,
                HTitle = "全部"
            });
            foreach (HallInfo hall in list)
            {
                hallList.Add(hall.HTitle);
            }
            ddlHallSearch.DataSource = hallList;

            List<DdlModel> listDdl =new List<DdlModel>()
            {
                new DdlModel("-1","全部"),
                new DdlModel("1","空闲"),
                new DdlModel("0","使用中")
            };
            List<string> ddlModelStringList = new List<string>();
            foreach(DdlModel ddl in listDdl)
            {
                ddlModelStringList.Add(ddl.Title);
            }
            ddlFreeSearch.DataSource = ddlModelStringList;
        }

        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.Value = Convert.ToBoolean(e.Value) ? "√" : "×";
            }
        }
        //选择了厅包
        private void ddlHallSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }
        //选择了空闲/使用中/全部
        private void ddlFreeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        //列表所有
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            ddlHallSearch.SelectedIndex = 0;
            ddlFreeSearch.SelectedIndex = 0;
            LoadList();
        }

        //添加
        private void btnSave_Click(object sender, EventArgs e)
        {
            if ("-请选择-".Equals(ddlHallAdd.SelectedValue.ToString()) || "全部".Equals(ddlHallAdd.SelectedValue.ToString()))
            {
                MessageBox.Show("请选择厅包");
                return;
            }
            //接收用户值，构造对象
            TableInfo ti=new TableInfo()
            {
                TTitle = txtTitle.Text.Trim(),
                HTitle = ddlHallAdd.SelectedValue.ToString(),
                TIsFree = rbFree.Checked
            };

            if (txtId.Text == "添加时无编号")
            {
                #region 添加

                if (tiBll.Add(ti))
                {
                    LoadList();
                }
                #endregion
            }
            else
            {
                #region 修改

                ti.Id = int.Parse(txtId.Text);
                if (tiBll.Edit(ti))
                {
                    LoadList();
                }

                #endregion
            }

            //恢复控件值
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            ddlHallAdd.SelectedIndex = 0;
            rbFree.Checked = true;
            btnSave.Text = "添加";

            Refresh();
        }
        //填好值后不添加（取消）
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //恢复控件值
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            ddlHallAdd.SelectedIndex = 0;
            rbFree.Checked = true;
            btnSave.Text = "添加";
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            ddlHallAdd.Text = row.Cells[2].Value.ToString();
            if (Convert.ToBoolean(row.Cells[3].Value))
            {
                rbFree.Checked = true;
            }
            else
            {
                rbUnFree.Checked = true;
            }
            btnSave.Text = "修改";
        }

        //删除选中的行数据
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvList.SelectedRows[0].Cells[0].Value);
            DialogResult result = MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                if (tiBll.Remove(id))
                {
                    LoadList();
                }
            }

            Refresh();
        }
        //添加厅包
        private void btnAddHall_Click(object sender, EventArgs e)
        {
            FormHallInfo formHallInfo=new FormHallInfo();
            formHallInfo.MyUpdateForm += LoadSearchList;
            formHallInfo.MyUpdateForm += LoadList;
            formHallInfo.Show();
        }
    }
}
