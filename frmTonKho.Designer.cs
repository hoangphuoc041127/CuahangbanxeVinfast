namespace Quanlybanoto
{
    partial class frmTonKho
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTuKhoa = new System.Windows.Forms.TextBox();
            this.cboHang = new System.Windows.Forms.ComboBox();
            this.nudNguong = new System.Windows.Forms.NumericUpDown();
            this.dgvTonKho = new System.Windows.Forms.DataGridView();
            this.btnXem = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.lblTongKet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudNguong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTonKho)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "THỐNG KÊ TỒN KHO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Từ khóa:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(324, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Hãng:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(584, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Sắp hết khi tồn:";
            // 
            // txtTuKhoa
            // 
            this.txtTuKhoa.Location = new System.Drawing.Point(110, 47);
            this.txtTuKhoa.Name = "txtTuKhoa";
            this.txtTuKhoa.Size = new System.Drawing.Size(210, 20);
            this.txtTuKhoa.TabIndex = 4;
            // 
            // cboHang
            // 
            this.cboHang.FormattingEnabled = true;
            this.cboHang.Location = new System.Drawing.Point(396, 46);
            this.cboHang.Name = "cboHang";
            this.cboHang.Size = new System.Drawing.Size(182, 21);
            this.cboHang.TabIndex = 5;
            // 
            // nudNguong
            // 
            this.nudNguong.Location = new System.Drawing.Point(740, 49);
            this.nudNguong.Name = "nudNguong";
            this.nudNguong.Size = new System.Drawing.Size(120, 20);
            this.nudNguong.TabIndex = 6;
            // 
            // dgvTonKho
            // 
            this.dgvTonKho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTonKho.Location = new System.Drawing.Point(10, 70);
            this.dgvTonKho.Name = "dgvTonKho";
            this.dgvTonKho.Size = new System.Drawing.Size(850, 334);
            this.dgvTonKho.TabIndex = 7;
            // 
            // btnXem
            // 
            this.btnXem.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXem.Location = new System.Drawing.Point(26, 436);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(198, 43);
            this.btnXem.TabIndex = 8;
            this.btnXem.Text = "Xem ";
            this.btnXem.UseVisualStyleBackColor = true;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.Location = new System.Drawing.Point(328, 436);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(215, 43);
            this.btnLamMoi.TabIndex = 9;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click_1);
            // 
            // btnThoat
            // 
            this.btnThoat.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.Location = new System.Drawing.Point(632, 436);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(203, 43);
            this.btnThoat.TabIndex = 10;
            this.btnThoat.Text = "Thoát ";
            this.btnThoat.UseVisualStyleBackColor = true;
            // 
            // lblTongKet
            // 
            this.lblTongKet.AutoSize = true;
            this.lblTongKet.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongKet.ForeColor = System.Drawing.Color.Red;
            this.lblTongKet.Location = new System.Drawing.Point(12, 409);
            this.lblTongKet.Name = "lblTongKet";
            this.lblTongKet.Size = new System.Drawing.Size(199, 24);
            this.lblTongKet.TabIndex = 11;
            this.lblTongKet.Text = "Tổng 22 xe trong kho";
            // 
            // frmTonKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 491);
            this.Controls.Add(this.lblTongKet);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnXem);
            this.Controls.Add(this.dgvTonKho);
            this.Controls.Add(this.nudNguong);
            this.Controls.Add(this.cboHang);
            this.Controls.Add(this.txtTuKhoa);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmTonKho";
            this.Text = "frmTonKho";
            this.Load += new System.EventHandler(this.frmTonKho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudNguong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTonKho)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTuKhoa;
        private System.Windows.Forms.ComboBox cboHang;
        private System.Windows.Forms.NumericUpDown nudNguong;
        private System.Windows.Forms.DataGridView dgvTonKho;
        private System.Windows.Forms.Button btnXem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Label lblTongKet;
    }
}