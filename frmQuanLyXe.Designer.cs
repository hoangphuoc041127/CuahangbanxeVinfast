namespace Quanlybanoto
{
    partial class frmQuanLyXe
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
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboLocDong = new System.Windows.Forms.ComboBox();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.picAnh = new System.Windows.Forms.PictureBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnChon = new System.Windows.Forms.Button();
            this.dgvXe = new System.Windows.Forms.DataGridView();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMauSac = new System.Windows.Forms.TextBox();
            this.txtTenXe = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNamSX = new System.Windows.Forms.TextBox();
            this.txtMaXe = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXe)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(243, -1);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(298, 31);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "QUẢN LÝ XE VINFAST ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tìm kiếm:";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(109, 33);
            this.txtTimKiem.Multiline = true;
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(247, 36);
            this.txtTimKiem.TabIndex = 2;
            // 
            // btnTim
            // 
            this.btnTim.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTim.Location = new System.Drawing.Point(362, 33);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(94, 37);
            this.btnTim.TabIndex = 3;
            this.btnTim.Text = "Tìm ";
            this.btnTim.UseVisualStyleBackColor = true;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(462, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lọc dòng xe:";
            // 
            // cboLocDong
            // 
            this.cboLocDong.FormattingEnabled = true;
            this.cboLocDong.Location = new System.Drawing.Point(584, 47);
            this.cboLocDong.Name = "cboLocDong";
            this.cboLocDong.Size = new System.Drawing.Size(170, 21);
            this.cboLocDong.TabIndex = 5;
            // 
            // grpThongTin
            // 
            this.grpThongTin.Controls.Add(this.picAnh);
            this.grpThongTin.Controls.Add(this.btnThoat);
            this.grpThongTin.Controls.Add(this.btnLamMoi);
            this.grpThongTin.Controls.Add(this.btnXoa);
            this.grpThongTin.Controls.Add(this.btnSua);
            this.grpThongTin.Controls.Add(this.btnThem);
            this.grpThongTin.Controls.Add(this.btnChon);
            this.grpThongTin.Controls.Add(this.dgvXe);
            this.grpThongTin.Controls.Add(this.txtGhiChu);
            this.grpThongTin.Controls.Add(this.label9);
            this.grpThongTin.Controls.Add(this.txtSoLuong);
            this.grpThongTin.Controls.Add(this.txtGiaBan);
            this.grpThongTin.Controls.Add(this.label8);
            this.grpThongTin.Controls.Add(this.label7);
            this.grpThongTin.Controls.Add(this.txtMauSac);
            this.grpThongTin.Controls.Add(this.txtTenXe);
            this.grpThongTin.Controls.Add(this.label6);
            this.grpThongTin.Controls.Add(this.label5);
            this.grpThongTin.Controls.Add(this.txtNamSX);
            this.grpThongTin.Controls.Add(this.txtMaXe);
            this.grpThongTin.Controls.Add(this.label4);
            this.grpThongTin.Controls.Add(this.label3);
            this.grpThongTin.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTin.Location = new System.Drawing.Point(21, 71);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(767, 346);
            this.grpThongTin.TabIndex = 6;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin xe:";
            // 
            // picAnh
            // 
            this.picAnh.Location = new System.Drawing.Point(592, 20);
            this.picAnh.Name = "picAnh";
            this.picAnh.Size = new System.Drawing.Size(157, 98);
            this.picAnh.TabIndex = 24;
            this.picAnh.TabStop = false;
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(624, 202);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(125, 33);
            this.btnThoat.TabIndex = 23;
            this.btnThoat.Text = "Thoát ";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(472, 202);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(125, 32);
            this.btnLamMoi.TabIndex = 22;
            this.btnLamMoi.Text = "Làm mới ";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click_1);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(319, 203);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(126, 32);
            this.btnXoa.TabIndex = 21;
            this.btnXoa.Text = "Xóa ";
            this.btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(167, 202);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 32);
            this.btnSua.TabIndex = 20;
            this.btnSua.Text = "Sửa ";
            this.btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(26, 200);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(110, 34);
            this.btnThem.TabIndex = 19;
            this.btnThem.Text = "Thêm ";
            this.btnThem.UseVisualStyleBackColor = true;
            // 
            // btnChon
            // 
            this.btnChon.Location = new System.Drawing.Point(624, 124);
            this.btnChon.Name = "btnChon";
            this.btnChon.Size = new System.Drawing.Size(92, 34);
            this.btnChon.TabIndex = 18;
            this.btnChon.Text = "Chọn ảnh ";
            this.btnChon.UseVisualStyleBackColor = true;
            // 
            // dgvXe
            // 
            this.dgvXe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvXe.Location = new System.Drawing.Point(-1, 252);
            this.dgvXe.Name = "dgvXe";
            this.dgvXe.Size = new System.Drawing.Size(768, 120);
            this.dgvXe.TabIndex = 16;
            this.dgvXe.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvXe_CellContentClick);
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(113, 149);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(456, 32);
            this.txtGhiChu.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 24);
            this.label9.TabIndex = 12;
            this.label9.Text = "Ghi chú:";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(391, 112);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(151, 32);
            this.txtSoLuong.TabIndex = 11;
            // 
            // txtGiaBan
            // 
            this.txtGiaBan.Location = new System.Drawing.Point(114, 111);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.Size = new System.Drawing.Size(168, 32);
            this.txtGiaBan.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(288, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 24);
            this.label8.TabIndex = 9;
            this.label8.Text = "Số lượng:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 24);
            this.label7.TabIndex = 8;
            this.label7.Text = "Gía bán:";
            // 
            // txtMauSac
            // 
            this.txtMauSac.Location = new System.Drawing.Point(396, 73);
            this.txtMauSac.Name = "txtMauSac";
            this.txtMauSac.Size = new System.Drawing.Size(175, 32);
            this.txtMauSac.TabIndex = 7;
            // 
            // txtTenXe
            // 
            this.txtTenXe.Location = new System.Drawing.Point(310, 32);
            this.txtTenXe.Name = "txtTenXe";
            this.txtTenXe.Size = new System.Drawing.Size(232, 32);
            this.txtTenXe.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(289, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 24);
            this.label6.TabIndex = 5;
            this.label6.Text = "Màu sắc:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(240, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "Tên xe:";
            // 
            // txtNamSX
            // 
            this.txtNamSX.Location = new System.Drawing.Point(167, 73);
            this.txtNamSX.Name = "txtNamSX";
            this.txtNamSX.Size = new System.Drawing.Size(116, 32);
            this.txtNamSX.TabIndex = 3;
            // 
            // txtMaXe
            // 
            this.txtMaXe.Location = new System.Drawing.Point(100, 32);
            this.txtMaXe.Name = "txtMaXe";
            this.txtMaXe.Size = new System.Drawing.Size(134, 32);
            this.txtMaXe.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Năm sản xuất:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mã xe:";
            // 
            // frmQuanLyXe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 450);
            this.Controls.Add(this.grpThongTin);
            this.Controls.Add(this.cboLocDong);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnTim);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTieuDe);
            this.Name = "frmQuanLyXe";
            this.Text = "frmQuanLyXe";
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLocDong;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMauSac;
        private System.Windows.Forms.TextBox txtTenXe;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNamSX;
        private System.Windows.Forms.TextBox txtMaXe;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.TextBox txtGiaBan;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvXe;
        private System.Windows.Forms.Button btnChon;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.PictureBox picAnh;
    }
}