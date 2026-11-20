namespace Quanlybanoto
{
    partial class frmNhapXe
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
            this.txtMaPhieu = new System.Windows.Forms.TextBox();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.txtNguoiLap = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboNhaCungCap = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboXe = new System.Windows.Forms.ComboBox();
            this.numSoLuong = new System.Windows.Forms.NumericUpDown();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnPhieuMoi = new System.Windows.Forms.Button();
            this.btnThemXe = new System.Windows.Forms.Button();
            this.btnXoaXe = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(207, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(379, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "PHIẾU NHẬP XE Ô TÔ ĐIỆN VINFAST";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mã Phiếu:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(270, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ngày Lập:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(574, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Người Lập:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtMaPhieu
            // 
            this.txtMaPhieu.Location = new System.Drawing.Point(123, 67);
            this.txtMaPhieu.Name = "txtMaPhieu";
            this.txtMaPhieu.Size = new System.Drawing.Size(150, 20);
            this.txtMaPhieu.TabIndex = 4;
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.Location = new System.Drawing.Point(368, 65);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(200, 20);
            this.dtpNgayLap.TabIndex = 5;
            // 
            // txtNguoiLap
            // 
            this.txtNguoiLap.Location = new System.Drawing.Point(688, 67);
            this.txtNguoiLap.Name = "txtNguoiLap";
            this.txtNguoiLap.Size = new System.Drawing.Size(100, 20);
            this.txtNguoiLap.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "Nhà cung cấp:";
            // 
            // cboNhaCungCap
            // 
            this.cboNhaCungCap.FormattingEnabled = true;
            this.cboNhaCungCap.Location = new System.Drawing.Point(156, 106);
            this.cboNhaCungCap.Name = "cboNhaCungCap";
            this.cboNhaCungCap.Size = new System.Drawing.Size(547, 21);
            this.cboNhaCungCap.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(-2, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "Chọn xe:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(445, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 24);
            this.label7.TabIndex = 10;
            this.label7.Text = "SL:";
            // 
            // cboXe
            // 
            this.cboXe.FormattingEnabled = true;
            this.cboXe.Location = new System.Drawing.Point(84, 152);
            this.cboXe.Name = "cboXe";
            this.cboXe.Size = new System.Drawing.Size(355, 21);
            this.cboXe.TabIndex = 11;
            // 
            // numSoLuong
            // 
            this.numSoLuong.Location = new System.Drawing.Point(484, 152);
            this.numSoLuong.Name = "numSoLuong";
            this.numSoLuong.Size = new System.Drawing.Size(102, 20);
            this.numSoLuong.TabIndex = 12;
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.Location = new System.Drawing.Point(12, 179);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.Size = new System.Drawing.Size(608, 165);
            this.dgvChiTiet.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(24, 360);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 24);
            this.label8.TabIndex = 14;
            this.label8.Text = "Ghi chú:";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(108, 360);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(605, 20);
            this.txtGhiChu.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 404);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 24);
            this.label9.TabIndex = 16;
            this.label9.Text = "Tổng tiền:";
            // 
            // txtTongTien
            // 
            this.txtTongTien.Location = new System.Drawing.Point(108, 408);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(404, 20);
            this.txtTongTien.TabIndex = 17;
            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.Location = new System.Drawing.Point(532, 401);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(106, 34);
            this.btnLuu.TabIndex = 18;
            this.btnLuu.Text = "Lưu phiếu nhập";
            this.btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnPhieuMoi
            // 
            this.btnPhieuMoi.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhieuMoi.Location = new System.Drawing.Point(661, 400);
            this.btnPhieuMoi.Name = "btnPhieuMoi";
            this.btnPhieuMoi.Size = new System.Drawing.Size(115, 34);
            this.btnPhieuMoi.TabIndex = 19;
            this.btnPhieuMoi.Text = "Phiếu mới";
            this.btnPhieuMoi.UseVisualStyleBackColor = true;
            // 
            // btnThemXe
            // 
            this.btnThemXe.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemXe.Location = new System.Drawing.Point(654, 188);
            this.btnThemXe.Name = "btnThemXe";
            this.btnThemXe.Size = new System.Drawing.Size(122, 46);
            this.btnThemXe.TabIndex = 20;
            this.btnThemXe.Text = "Thêm xe";
            this.btnThemXe.UseVisualStyleBackColor = true;
            // 
            // btnXoaXe
            // 
            this.btnXoaXe.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaXe.Location = new System.Drawing.Point(654, 240);
            this.btnXoaXe.Name = "btnXoaXe";
            this.btnXoaXe.Size = new System.Drawing.Size(122, 44);
            this.btnXoaXe.TabIndex = 21;
            this.btnXoaXe.Text = "Xóa xe";
            this.btnXoaXe.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(592, 149);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 24);
            this.label10.TabIndex = 22;
            this.label10.Text = "Đơn giá:";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Location = new System.Drawing.Point(678, 152);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(110, 20);
            this.txtDonGia.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(654, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 42);
            this.button1.TabIndex = 24;
            this.button1.Text = "Thoát ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmNhapXe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDonGia);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnXoaXe);
            this.Controls.Add(this.btnThemXe);
            this.Controls.Add(this.btnPhieuMoi);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.txtTongTien);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtGhiChu);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgvChiTiet);
            this.Controls.Add(this.numSoLuong);
            this.Controls.Add(this.cboXe);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboNhaCungCap);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtNguoiLap);
            this.Controls.Add(this.dtpNgayLap);
            this.Controls.Add(this.txtMaPhieu);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmNhapXe";
            this.Text = "frmNhapXe";
            this.Load += new System.EventHandler(this.frmNhapXe_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaPhieu;
        private System.Windows.Forms.DateTimePicker dtpNgayLap;
        private System.Windows.Forms.TextBox txtNguoiLap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboXe;
        private System.Windows.Forms.NumericUpDown numSoLuong;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnPhieuMoi;
        private System.Windows.Forms.Button btnThemXe;
        private System.Windows.Forms.Button btnXoaXe;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Button button1;
    }
}