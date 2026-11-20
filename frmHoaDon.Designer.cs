namespace Quanlybanoto
{
    partial class frmHoaDon
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
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.cboKhachHang = new System.Windows.Forms.ComboBox();
            this.txtNguoiLap = new System.Windows.Forms.TextBox();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaHD = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboXe = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudSoLuong = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnMoi = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieuDe.Location = new System.Drawing.Point(12, 9);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(332, 26);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "HÓA ĐƠN BÁN XE VINFAST ";
            // 
            // grpThongTin
            // 
            this.grpThongTin.Controls.Add(this.btnIn);
            this.grpThongTin.Controls.Add(this.cboKhachHang);
            this.grpThongTin.Controls.Add(this.txtNguoiLap);
            this.grpThongTin.Controls.Add(this.dtpNgayLap);
            this.grpThongTin.Controls.Add(this.label5);
            this.grpThongTin.Controls.Add(this.txtMaHD);
            this.grpThongTin.Controls.Add(this.label4);
            this.grpThongTin.Controls.Add(this.label3);
            this.grpThongTin.Controls.Add(this.label2);
            this.grpThongTin.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTin.Location = new System.Drawing.Point(17, 38);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(823, 135);
            this.grpThongTin.TabIndex = 1;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin Hợp đồng";
            // 
            // cboKhachHang
            // 
            this.cboKhachHang.FormattingEnabled = true;
            this.cboKhachHang.Location = new System.Drawing.Point(125, 95);
            this.cboKhachHang.Name = "cboKhachHang";
            this.cboKhachHang.Size = new System.Drawing.Size(412, 32);
            this.cboKhachHang.TabIndex = 7;
            // 
            // txtNguoiLap
            // 
            this.txtNguoiLap.Location = new System.Drawing.Point(117, 60);
            this.txtNguoiLap.Name = "txtNguoiLap";
            this.txtNguoiLap.Size = new System.Drawing.Size(386, 32);
            this.txtNguoiLap.TabIndex = 6;
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.Location = new System.Drawing.Point(483, 22);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(311, 32);
            this.dtpNgayLap.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(389, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "Ngày lập:";
            // 
            // txtMaHD
            // 
            this.txtMaHD.Location = new System.Drawing.Point(125, 25);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.Size = new System.Drawing.Size(258, 32);
            this.txtMaHD.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 24);
            this.label4.TabIndex = 2;
            this.label4.Text = "Khách hàng:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "Người lập:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mã hóa đơn:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(23, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 24);
            this.label6.TabIndex = 2;
            this.label6.Text = "Chọn xe:";
            // 
            // cboXe
            // 
            this.cboXe.FormattingEnabled = true;
            this.cboXe.Location = new System.Drawing.Point(119, 179);
            this.cboXe.Name = "cboXe";
            this.cboXe.Size = new System.Drawing.Size(281, 21);
            this.cboXe.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(406, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 24);
            this.label7.TabIndex = 4;
            this.label7.Text = "SL:";
            // 
            // nudSoLuong
            // 
            this.nudSoLuong.Location = new System.Drawing.Point(445, 179);
            this.nudSoLuong.Name = "nudSoLuong";
            this.nudSoLuong.Size = new System.Drawing.Size(120, 20);
            this.nudSoLuong.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(571, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 24);
            this.label8.TabIndex = 6;
            this.label8.Text = "Đơn giá:";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Location = new System.Drawing.Point(655, 179);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(185, 20);
            this.txtDonGia.TabIndex = 7;
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.Location = new System.Drawing.Point(18, 214);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.Size = new System.Drawing.Size(822, 150);
            this.dgvChiTiet.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(23, 380);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 24);
            this.label9.TabIndex = 9;
            this.label9.Text = "Ghi chú:";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(105, 384);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(721, 20);
            this.txtGhiChu.TabIndex = 10;
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.Location = new System.Drawing.Point(23, 417);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(101, 24);
            this.lblTongTien.TabIndex = 11;
            this.lblTongTien.Text = "Tổng tiền:";
            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.Location = new System.Drawing.Point(27, 445);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(163, 33);
            this.btnLuu.TabIndex = 12;
            this.btnLuu.Text = "Lưu hóa đơn ";
            this.btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnMoi
            // 
            this.btnMoi.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoi.Location = new System.Drawing.Point(226, 444);
            this.btnMoi.Name = "btnMoi";
            this.btnMoi.Size = new System.Drawing.Size(153, 35);
            this.btnMoi.TabIndex = 13;
            this.btnMoi.Text = "Hóa đơn mới";
            this.btnMoi.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.Location = new System.Drawing.Point(423, 445);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(127, 35);
            this.btnXoa.TabIndex = 14;
            this.btnXoa.Text = "Xóa ";
            this.btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.Location = new System.Drawing.Point(591, 445);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(110, 35);
            this.btnSua.TabIndex = 15;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThoat
            // 
            this.btnThoat.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.Location = new System.Drawing.Point(739, 445);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(101, 35);
            this.btnThoat.TabIndex = 16;
            this.btnThoat.Text = "Thoát ";
            this.btnThoat.UseVisualStyleBackColor = true;
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(627, 72);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(140, 33);
            this.btnIn.TabIndex = 8;
            this.btnIn.Text = "In hóa đơn ";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // frmHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 510);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnMoi);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.lblTongTien);
            this.Controls.Add(this.txtGhiChu);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dgvChiTiet);
            this.Controls.Add(this.txtDonGia);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nudSoLuong);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboXe);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.grpThongTin);
            this.Controls.Add(this.lblTieuDe);
            this.Name = "frmHoaDon";
            this.Text = "frmHoaDon";
            this.Load += new System.EventHandler(this.frmHoaDon_Load);
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNguoiLap;
        private System.Windows.Forms.DateTimePicker dtpNgayLap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaHD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboKhachHang;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboXe;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudSoLuong;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnMoi;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnIn;
    }
}