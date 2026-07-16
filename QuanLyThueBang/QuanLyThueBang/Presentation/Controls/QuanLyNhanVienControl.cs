using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.Presentation.Controls
{
    public class QuanLyNhanVienControl : UserControl
    {
        private readonly NhanVienService _nvService;
        private readonly CuaHangService _chService;
        
        private List<NhanVienDTO> _allNhanVien = new List<NhanVienDTO>();
        private NhanVienDTO? _selectedNhanVien = null;
        private bool _isAddNew = false;

        // UI Variables
        private TextBox txtSearch = null!;
        private FlowLayoutPanel flpList = null!;
        private Panel pnlRight = null!;
        
        // Right Pane Controls
        private Label lblAvatar = null!;
        private Label lblHeaderName = null!;
        private Label lblHeaderMaNV = null!;
        private Panel pnlStatusBadge = null!;
        private Label lblStatusText = null!;

        private TextBox txtHoTen = null!;
        private TextBox txtMaNV = null!;
        private TextBox txtCCCD = null!;
        private TextBox txtSDT = null!;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private ComboBox cboVaiTro = null!;
        private ComboBox cboCuaHang = null!;

        private Button btnSave = null!;
        private Button btnDelete = null!;
        private Button btnCancel = null!;

        // Theme Colors (Serene Boutique)
        private readonly Color Surface = Color.FromArgb(251, 249, 249); // #fbf9f9
        private readonly Color SurfaceContainerLow = Color.FromArgb(245, 243, 243); // #f5f3f3
        private readonly Color SurfaceContainer = Color.FromArgb(239, 237, 237); // #efeded
        private readonly Color OutlineVariant = Color.FromArgb(210, 195, 196); // #d2c3c4
        private readonly Color Primary = Color.FromArgb(112, 88, 92); // #70585c
        private readonly Color PrimaryContainer = Color.FromArgb(245, 214, 218); // #f5d6da
        private readonly Color OnPrimaryContainer = Color.FromArgb(115, 91, 95); // #735b5f
        private readonly Color OnSurface = Color.FromArgb(27, 28, 28); // #1b1c1c
        private readonly Color OnSurfaceVariant = Color.FromArgb(78, 68, 69); // #4e4445
        private readonly Color SecondaryContainer = Color.FromArgb(229, 226, 225); // #e5e2e1
        private readonly Color OnSecondaryContainer = Color.FromArgb(101, 100, 100); // #656464
        private readonly Color Error = Color.FromArgb(186, 26, 26); // #ba1a1a

        public QuanLyNhanVienControl(NhanVienService nvService, CuaHangService chService)
        {
            _nvService = nvService;
            _chService = chService;
            InitializeUI();
            this.Load += (s, e) => LoadData();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Surface;
            this.Font = new Font("Segoe UI", 9.5F);

            // ── Top Header ──────────────────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Surface };
            var pnlHeaderBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = OutlineVariant };
            pnlHeader.Controls.Add(pnlHeaderBorder);

            var lblTitle = new Label { Text = "Quản Lý Nhân Viên", Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(30, 15), AutoSize = true };
            var lblSubTitle = new Label { Text = "Quản lý hồ sơ nhân viên và phân quyền truy cập.", Font = new Font("Segoe UI", 9.5F), ForeColor = OnSurfaceVariant, Location = new Point(32, 45), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubTitle);

            var btnAddTop = new Button
            {
                Text = "➕ Thêm Nhân Viên",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.Width - 160, 20), // Will anchor
                Size = new Size(130, 38),
                BackColor = Color.FromArgb(17, 17, 17), // #111111
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddTop.FlatAppearance.BorderSize = 0;
            btnAddTop.Click += (s, e) => StartAddNew();
            pnlHeader.Controls.Add(btnAddTop);

            this.Controls.Add(pnlHeader);

            // ── Split View Container ─────────────────────────────────────────────
            var pnlSplit = new Panel { Dock = DockStyle.Fill, BackColor = Surface };
            this.Controls.Add(pnlSplit);
            pnlSplit.BringToFront(); // Ensure it's under header

            // Left Pane (List)
            var pnlLeft = new Panel { Dock = DockStyle.Left, Width = 320, BackColor = Color.White };
            var pnlLeftBorder = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = OutlineVariant };
            pnlLeft.Controls.Add(pnlLeftBorder);

            var pnlSearch = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = SurfaceContainerLow };
            var pnlSearchBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = OutlineVariant };
            pnlSearch.Controls.Add(pnlSearchBorder);
            txtSearch = new TextBox
            {
                Location = new Point(15, 18),
                Size = new Size(290, 29),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "🔍 Tìm kiếm nhân viên..."
            };
            txtSearch.TextChanged += (s, e) => FilterList();
            pnlSearch.Controls.Add(txtSearch);
            pnlLeft.Controls.Add(pnlSearch);

            flpList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            flpList.Resize += (s, e) => { foreach (Control c in flpList.Controls) c.Width = flpList.ClientSize.Width - 20; };
            pnlLeft.Controls.Add(flpList);
            flpList.BringToFront();

            // Right Pane (Details)
            pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = Surface, Padding = new Padding(30) };
            
            // Build Form inside Right Pane
            BuildRightForm();

            pnlSplit.Controls.Add(pnlRight);
            pnlSplit.Controls.Add(pnlLeft);
        }

        private void BuildRightForm()
        {
            var pnlCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0)
            };
            // Add subtle border
            pnlCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(OutlineVariant, 1);
                var rect = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
                // Simple rounded rect simulation for border
                e.Graphics.DrawPath(pen, RoundedRect(rect, 8));
            };
            
            // Header of Card
            var pnlCardHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.Transparent };
            var pnlCardHeaderBorder = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = OutlineVariant };
            pnlCardHeader.Controls.Add(pnlCardHeaderBorder);

            lblAvatar = new Label
            {
                Size = new Size(60, 60),
                Location = new Point(25, 20),
                BackColor = PrimaryContainer,
                ForeColor = OnPrimaryContainer,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "NA"
            };
            lblAvatar.Paint += RoundedLabelPaint;

            lblHeaderName = new Label { Text = "Chọn một nhân viên", Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(100, 25), AutoSize = true };
            lblHeaderMaNV = new Label { Text = "EMP-000", Font = new Font("Segoe UI", 9.5F), ForeColor = OnSurfaceVariant, Location = new Point(205, 58), AutoSize = true };

            pnlStatusBadge = new Panel { Location = new Point(100, 56), Size = new Size(95, 22), BackColor = Color.FromArgb(221, 221, 221) };
            pnlStatusBadge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(pnlStatusBadge.BackColor);
                e.Graphics.FillPath(brush, RoundedRect(new Rectangle(0, 0, pnlStatusBadge.Width - 1, pnlStatusBadge.Height - 1), 10));
            };
            lblStatusText = new Label { Text = "Đang làm việc", ForeColor = Color.FromArgb(96, 97, 97), Font = new Font("Segoe UI", 8F, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            pnlStatusBadge.Controls.Add(lblStatusText);

            pnlCardHeader.Controls.Add(lblAvatar);
            pnlCardHeader.Controls.Add(lblHeaderName);
            pnlCardHeader.Controls.Add(lblHeaderMaNV);
            pnlCardHeader.Controls.Add(pnlStatusBadge);

            // Footer of Card
            var pnlCardFooter = new Panel { Dock = DockStyle.Bottom, Height = 75, BackColor = Color.FromArgb(246, 246, 246) };
            var pnlCardFooterBorder = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = OutlineVariant };
            pnlCardFooter.Controls.Add(pnlCardFooterBorder);

            btnDelete = new Button { Text = "🗑️ Xóa Nhân Viên", Location = new Point(25, 20), Size = new Size(160, 36), FlatStyle = FlatStyle.Flat, ForeColor = Error, Font = new Font("Segoe UI Semibold", 9.5F), Cursor = Cursors.Hand };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            btnCancel = new Button { Text = "Hủy", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlRight.Width - 270, 20), Size = new Size(90, 36), FlatStyle = FlatStyle.Flat, BackColor = SurfaceContainer, ForeColor = OnSurfaceVariant, Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderColor = OutlineVariant;
            btnCancel.Click += BtnCancel_Click;

            btnSave = new Button { Text = "Lưu Thay Đổi", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlRight.Width - 170, 20), Size = new Size(125, 36), FlatStyle = FlatStyle.Flat, BackColor = PrimaryContainer, ForeColor = Color.FromArgb(40, 23, 26), Font = new Font("Segoe UI Semibold", 9.5F), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnlCardFooter.Controls.Add(btnDelete);
            pnlCardFooter.Controls.Add(btnCancel);
            pnlCardFooter.Controls.Add(btnSave);

            // Body of Card (Form Fields)
            var pnlCardBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30, 20, 30, 20), AutoScroll = true, BackColor = Color.Transparent };

            int yPos = 20;
            // -- Section 1 --
            var lblSec1 = new Label { Text = "Thông Tin Cá Nhân", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(30, yPos), AutoSize = true };
            yPos += 30;
            var pnlDiv1 = new Panel { Location = new Point(30, yPos), Size = new Size(600, 1), BackColor = OutlineVariant, Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right };
            yPos += 20;

            txtHoTen = CreateInputField(pnlCardBody, "Họ và Tên", 30, yPos, 280, out Label _);
            txtMaNV = CreateInputField(pnlCardBody, "Mã Nhân Viên", 340, yPos, 280, out Label _);
            txtMaNV.Enabled = false; // Luôn readonly (tự sinh hoặc đã có)
            yPos += 70;

            txtCCCD = CreateInputField(pnlCardBody, "CCCD / CMND", 30, yPos, 280, out Label _);
            txtSDT = CreateInputField(pnlCardBody, "Số Điện Thoại", 340, yPos, 280, out Label _);
            yPos += 70;

            txtUsername = CreateInputField(pnlCardBody, "Tên Đăng Nhập", 30, yPos, 280, out Label _);
            txtPassword = CreateInputField(pnlCardBody, "Mật Khẩu", 340, yPos, 280, out Label _);
            txtPassword.UseSystemPasswordChar = true;
            yPos += 80;

            // -- Section 2 --
            var lblSec2 = new Label { Text = "Thông Tin Phân Công", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(30, yPos), AutoSize = true };
            yPos += 30;
            var pnlDiv2 = new Panel { Location = new Point(30, yPos), Size = new Size(600, 1), BackColor = OutlineVariant, Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right };
            yPos += 20;

            cboVaiTro = CreateComboBox(pnlCardBody, "Vai Trò", 30, yPos, 280);
            cboCuaHang = CreateComboBox(pnlCardBody, "Chi Nhánh", 340, yPos, 280);

            pnlCardBody.Controls.Add(lblSec1);
            pnlCardBody.Controls.Add(pnlDiv1);
            pnlCardBody.Controls.Add(lblSec2);
            pnlCardBody.Controls.Add(pnlDiv2);

            pnlCard.Controls.Add(pnlCardBody);
            pnlCard.Controls.Add(pnlCardHeader);
            pnlCard.Controls.Add(pnlCardFooter);
            pnlRight.Controls.Add(pnlCard);

            // Handle anchor resizing for form elements
            pnlCardBody.Resize += (s, e) =>
            {
                int halfWidth = (pnlCardBody.Width - 90) / 2;
                if (halfWidth < 200) halfWidth = 200;
                
                txtHoTen.Width = halfWidth;
                txtMaNV.Left = 30 + halfWidth + 30;
                txtMaNV.Width = halfWidth;

                txtCCCD.Width = halfWidth;
                txtSDT.Left = 30 + halfWidth + 30;
                txtSDT.Width = halfWidth;

                txtUsername.Width = halfWidth;
                txtPassword.Left = 30 + halfWidth + 30;
                txtPassword.Width = halfWidth;

                cboVaiTro.Width = halfWidth;
                cboCuaHang.Left = 30 + halfWidth + 30;
                cboCuaHang.Width = halfWidth;

                pnlDiv1.Width = pnlCardBody.Width - 60;
                pnlDiv2.Width = pnlCardBody.Width - 60;
            };
        }

        private TextBox CreateInputField(Panel parent, string label, int x, int y, int width, out Label lbl)
        {
            lbl = new Label { Text = label, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = OnSurfaceVariant, Location = new Point(x, y), AutoSize = true };
            var txt = new TextBox { Location = new Point(x, y + 25), Size = new Size(width, 30), Font = new Font("Segoe UI", 10.5F), BorderStyle = BorderStyle.FixedSingle };
            parent.Controls.Add(lbl);
            parent.Controls.Add(txt);
            return txt;
        }

        private ComboBox CreateComboBox(Panel parent, string label, int x, int y, int width)
        {
            var lbl = new Label { Text = label, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = OnSurfaceVariant, Location = new Point(x, y), AutoSize = true };
            var cbo = new ComboBox { Location = new Point(x, y + 25), Size = new Size(width, 30), Font = new Font("Segoe UI", 10.5F), DropDownStyle = ComboBoxStyle.DropDownList };
            parent.Controls.Add(lbl);
            parent.Controls.Add(cbo);
            return cbo;
        }

        // Helper graphics methods
        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void RoundedLabelPaint(object? sender, PaintEventArgs e)
        {
            var lbl = sender as Label;
            if (lbl == null) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(lbl.BackColor);
            e.Graphics.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
            using var textBrush = new SolidBrush(lbl.ForeColor);
            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString(lbl.Text, lbl.Font, textBrush, new Rectangle(0, 0, lbl.Width, lbl.Height), format);
        }

        // ── Data Logic ────────────────────────────────────────────────────────
        private void LoadData()
        {
            // Load ComboBoxes
            var vaiTros = _nvService.GetAllVaiTro();
            cboVaiTro.DataSource = vaiTros;
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";

            var cuaHangs = _chService.GetAllCuaHang();
            cuaHangs.Insert(0, new QuanLyThueBang.Domain.DTOs.CuaHangDTO { MaCuaHang = "", DiaChi = "Toàn hệ thống (Quản trị)" });
            cboCuaHang.DataSource = cuaHangs;
            cboCuaHang.DisplayMember = "DiaChi";
            cboCuaHang.ValueMember = "MaCuaHang";

            _allNhanVien = _nvService.GetAllNhanVien(txtSearch.Text);
            RenderList();

            if (_allNhanVien.Count > 0)
                SelectItem(_allNhanVien[0]);
            else
                ClearForm();
        }

        private void FilterList()
        {
            _allNhanVien = _nvService.GetAllNhanVien(txtSearch.Text);
            RenderList();
            if (_allNhanVien.Count > 0 && !_isAddNew)
                SelectItem(_allNhanVien[0]);
        }

        private void RenderList()
        {
            flpList.SuspendLayout();
            flpList.Controls.Clear();

            foreach (var nv in _allNhanVien)
            {
                var pnlItem = new Panel
                {
                    Width = flpList.ClientSize.Width - 20,
                    Height = 70,
                    BackColor = _selectedNhanVien?.MaNhanVien == nv.MaNhanVien && !_isAddNew ? SurfaceContainerHigh : Color.Transparent,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 0, 5),
                    Tag = nv
                };

                // Border left for selection
                if (_selectedNhanVien?.MaNhanVien == nv.MaNhanVien && !_isAddNew)
                {
                    var pnlLeftStripe = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = Primary };
                    pnlItem.Controls.Add(pnlLeftStripe);
                    pnlItem.BackColor = SurfaceContainer;
                }

                // Hover effect
                pnlItem.MouseEnter += (s, e) => { if (pnlItem.BackColor == Color.Transparent) pnlItem.BackColor = SurfaceContainerLow; };
                pnlItem.MouseLeave += (s, e) => { if (pnlItem.BackColor == SurfaceContainerLow) pnlItem.BackColor = Color.Transparent; };

                // Avatar
                var lblAva = new Label
                {
                    Size = new Size(40, 40),
                    Location = new Point(15, 15),
                    BackColor = SecondaryContainer,
                    ForeColor = OnSecondaryContainer,
                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                    Text = GetInitials(nv.HoTen),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                lblAva.Paint += RoundedLabelPaint;

                // Name & ID
                var lblName = new Label { Text = nv.HoTen, Font = new Font("Segoe UI Semibold", 10F), ForeColor = OnSurface, Location = new Point(65, 14), AutoSize = true };
                var lblID = new Label { Text = nv.MaNhanVien, Font = new Font("Segoe UI", 8.5F), ForeColor = OnSurfaceVariant, Location = new Point(66, 36), AutoSize = true };

                pnlItem.Controls.Add(lblAva);
                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(lblID);

                // Click event
                pnlItem.Click += (s, e) => SelectItem(nv);
                foreach (Control c in pnlItem.Controls)
                    c.Click += (s, e) => SelectItem(nv);

                flpList.Controls.Add(pnlItem);
            }
            flpList.ResumeLayout();
        }

        private Color SurfaceContainerHigh => Color.FromArgb(233, 232, 231);

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "NA";
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return (parts[0].Substring(0, 1) + parts.Last().Substring(0, 1)).ToUpper();
        }

        private void SelectItem(NhanVienDTO nv)
        {
            _isAddNew = false;
            _selectedNhanVien = nv;
            RenderList(); // update selection highlight

            // Populate form
            lblHeaderName.Text = nv.HoTen;
            lblHeaderMaNV.Text = nv.MaNhanVien;
            lblAvatar.Text = GetInitials(nv.HoTen);
            
            txtHoTen.Text = nv.HoTen;
            txtMaNV.Text = nv.MaNhanVien;
            txtCCCD.Text = nv.CMND;
            txtSDT.Text = nv.SoDienThoai;
            
            // Cannot edit username/password for existing users in this flow based on original logic
            txtUsername.Text = nv.TenDangNhap;
            txtUsername.Enabled = false;
            txtPassword.Text = "********";
            txtPassword.Enabled = false;

            cboVaiTro.SelectedValue = nv.MaVaiTro;
            cboCuaHang.SelectedValue = string.IsNullOrEmpty(nv.MaCuaHang) ? "" : nv.MaCuaHang;

            btnDelete.Visible = true;
            pnlRight.Visible = true;
        }

        private void StartAddNew()
        {
            _isAddNew = true;
            _selectedNhanVien = null;
            RenderList();

            lblHeaderName.Text = "Nhân Viên Mới";
            lblHeaderMaNV.Text = "Mã tự động tạo";
            lblAvatar.Text = "+";

            txtHoTen.Text = "";
            txtMaNV.Text = _nvService.GenerateNextMaNhanVien();
            txtCCCD.Text = "";
            txtSDT.Text = "";
            
            // Enable auth fields for new user
            txtUsername.Text = "";
            txtUsername.Enabled = true;
            txtPassword.Text = "";
            txtPassword.Enabled = true;

            if (cboVaiTro.Items.Count > 0) cboVaiTro.SelectedIndex = 0;
            if (cboCuaHang.Items.Count > 0) cboCuaHang.SelectedIndex = 0;

            btnDelete.Visible = false;
            pnlRight.Visible = true;
            txtHoTen.Focus();
        }

        private void ClearForm()
        {
            pnlRight.Visible = false;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (_isAddNew)
            {
                if (_allNhanVien.Count > 0) SelectItem(_allNhanVien[0]);
                else ClearForm();
            }
            else if (_selectedNhanVien != null)
            {
                SelectItem(_selectedNhanVien); // Reset fields to original
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string cmnd = txtCCCD.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            int vaiTro = (int)(cboVaiTro.SelectedValue ?? 0);
            string cuaHang = cboCuaHang.SelectedValue?.ToString() ?? "";
            if (cuaHang == "") cuaHang = null!; // mapping back to null for system-wide

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Vui lòng nhập Họ và Tên.", "Lỗi Xác Thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isAddNew)
            {
                string user = txtUsername.Text.Trim();
                string pass = txtPassword.Text;
                var res = _nvService.AddNhanVien(txtMaNV.Text, hoTen, cmnd, sdt, user, pass, vaiTro, cuaHang);
                
                if (res.Success)
                {
                    LoadData();
                    // Auto select the new one (it's in the list now, likely at the end or based on search)
                    var added = _allNhanVien.FirstOrDefault(n => n.MaNhanVien == txtMaNV.Text);
                    if (added != null) SelectItem(added);
                    MessageBox.Show("Thêm nhân viên mới thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (_selectedNhanVien != null)
            {
                var res = _nvService.UpdateNhanVien(_selectedNhanVien.MaNhanVien, hoTen, cmnd, sdt, vaiTro, cuaHang);
                if (res.Success)
                {
                    LoadData();
                    var updated = _allNhanVien.FirstOrDefault(n => n.MaNhanVien == _selectedNhanVien.MaNhanVien);
                    if (updated != null) SelectItem(updated);
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedNhanVien == null) return;
            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên '{_selectedNhanVien.HoTen}' không?", "Xác Nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                var res = _nvService.DeleteNhanVien(_selectedNhanVien.MaNhanVien);
                if (res.Success)
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
