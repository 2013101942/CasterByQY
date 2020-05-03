namespace UI
{
    partial class FormRegister
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.registerBtn = new System.Windows.Forms.Button();
            this.textMName = new System.Windows.Forms.TextBox();
            this.textMPhone = new System.Windows.Forms.TextBox();
            this.MName = new System.Windows.Forms.Label();
            this.MPhone = new System.Windows.Forms.Label();
            this.labelMPwd = new System.Windows.Forms.Label();
            this.textMPwd = new System.Windows.Forms.TextBox();
            this.labelMemberType = new System.Windows.Forms.Label();
            this.RegisterMemberType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // registerBtn
            // 
            this.registerBtn.Location = new System.Drawing.Point(362, 243);
            this.registerBtn.Name = "registerBtn";
            this.registerBtn.Size = new System.Drawing.Size(110, 32);
            this.registerBtn.TabIndex = 3;
            this.registerBtn.Text = "开始注册";
            this.registerBtn.UseVisualStyleBackColor = true;
            this.registerBtn.Click += new System.EventHandler(this.BtnRegister_Click);
            // 
            // textMName
            // 
            this.textMName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textMName.Location = new System.Drawing.Point(425, 40);
            this.textMName.Multiline = true;
            this.textMName.Name = "textMName";
            this.textMName.Size = new System.Drawing.Size(168, 30);
            this.textMName.TabIndex = 4;
            // 
            // textMPhone
            // 
            this.textMPhone.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textMPhone.Location = new System.Drawing.Point(425, 138);
            this.textMPhone.Multiline = true;
            this.textMPhone.Name = "textMPhone";
            this.textMPhone.Size = new System.Drawing.Size(168, 30);
            this.textMPhone.TabIndex = 5;
            // 
            // MName
            // 
            this.MName.BackColor = System.Drawing.SystemColors.Control;
            this.MName.Location = new System.Drawing.Point(333, 40);
            this.MName.Name = "MName";
            this.MName.Size = new System.Drawing.Size(68, 30);
            this.MName.TabIndex = 6;
            this.MName.Text = "姓  名";
            this.MName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MPhone
            // 
            this.MPhone.Location = new System.Drawing.Point(333, 138);
            this.MPhone.Name = "MPhone";
            this.MPhone.Size = new System.Drawing.Size(68, 30);
            this.MPhone.TabIndex = 7;
            this.MPhone.Text = "电话";
            this.MPhone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMPwd
            // 
            this.labelMPwd.Location = new System.Drawing.Point(333, 90);
            this.labelMPwd.Name = "labelMPwd";
            this.labelMPwd.Size = new System.Drawing.Size(66, 30);
            this.labelMPwd.TabIndex = 8;
            this.labelMPwd.Text = "密码";
            this.labelMPwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textMPwd
            // 
            this.textMPwd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textMPwd.Location = new System.Drawing.Point(425, 90);
            this.textMPwd.Multiline = true;
            this.textMPwd.Name = "textMPwd";
            this.textMPwd.PasswordChar = '*';
            this.textMPwd.Size = new System.Drawing.Size(168, 30);
            this.textMPwd.TabIndex = 9;
            // 
            // labelMemberType
            // 
            this.labelMemberType.Location = new System.Drawing.Point(335, 187);
            this.labelMemberType.Name = "labelMemberType";
            this.labelMemberType.Size = new System.Drawing.Size(64, 33);
            this.labelMemberType.TabIndex = 10;
            this.labelMemberType.Text = "会员类型";
            this.labelMemberType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RegisterMemberType
            // 
            this.RegisterMemberType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RegisterMemberType.FormattingEnabled = true;
            this.RegisterMemberType.Location = new System.Drawing.Point(425, 194);
            this.RegisterMemberType.Name = "RegisterMemberType";
            this.RegisterMemberType.Size = new System.Drawing.Size(121, 20);
            this.RegisterMemberType.TabIndex = 11;
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UI.Properties.Resources.registerBg;
            this.ClientSize = new System.Drawing.Size(605, 300);
            this.Controls.Add(this.RegisterMemberType);
            this.Controls.Add(this.labelMemberType);
            this.Controls.Add(this.textMPwd);
            this.Controls.Add(this.labelMPwd);
            this.Controls.Add(this.MPhone);
            this.Controls.Add(this.MName);
            this.Controls.Add(this.textMPhone);
            this.Controls.Add(this.textMName);
            this.Controls.Add(this.registerBtn);
            this.Name = "FormRegister";
            this.Text = "FormRegister";
            this.Load += new System.EventHandler(this.FormRegister_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button registerBtn;
        private System.Windows.Forms.TextBox textMName;
        private System.Windows.Forms.TextBox textMPhone;
        private System.Windows.Forms.Label MName;
        private System.Windows.Forms.Label MPhone;
        private System.Windows.Forms.Label labelMPwd;
        private System.Windows.Forms.TextBox textMPwd;
        private System.Windows.Forms.Label labelMemberType;
        private System.Windows.Forms.ComboBox RegisterMemberType;
    }
}