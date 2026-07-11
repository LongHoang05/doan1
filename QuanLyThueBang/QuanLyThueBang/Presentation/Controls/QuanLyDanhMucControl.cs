using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Presentation.Forms.DanhMuc;

namespace QuanLyThueBang.Presentation.Controls
{
    /// <summary>
    /// UserControl Quản lý Danh Mục (Thể loại phim) - Giao diện chuẩn 2
    /// </summary>
    public class QuanLyDanhMucControl : UserControl
    {
        private readonly PhimService _phimService;

        private List<TheLoaiDTO> _allTheLoais = new List<TheLoaiDTO>();
        private int _currentPage = 1;
        private int _pageSize = 15;

        private Panel pnlHeader = null!;
        private Panel pnlFilter = null!;
        private Panel pnlFooter = null!;
        private DataGridView dgvDanhMuc = null!;

        private TextBox txtSearch = null!;
        private Button btnAddCategory = null!;

        private Label lblPageInfo = null!;
        private Button btnPrevPage = null!;
        private Button btnNextPage = null!;
        private Label lblCurrentPage = null!;

        public QuanLyDanhMucControl(PhimService phimService)
        {
            _phimService = phimService ?? throw new ArgumentNullException(nameof(phimService));
            InitializeComponent();
            this.Load += (s, e) => LoadData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);

            BuildHeaderPanel();
            BuildFilterPanel();
            BuildFooterPanel();
            BuildDataGrid();
        }

        #region BỐ CỤC GIAO DIỆN

        private void BuildHeaderPanel()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.White,
                Padding = new Padding(25, 15, 25, 15)
            };

            // Panel chứa nút bên phải đảm bảo luôn hiển thị góc phải
            var pnlActionRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 220,
                BackColor = Color.Transparent
            };

            btnAddCategory = new Button
            {
                Text = "+ Thêm Danh Mục Mới",
                Size = new Size(185, 42),
                Location = new Point(15, 21),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddCategory.FlatAppearance.BorderSize = 0;
            btnAddCategory.Click += BtnAddCategory_Click;
            pnlActionRight.Controls.Add(btnAddCategory);

            var lblTitle = new Label
            {
                Text = "Quản Lý Danh Mục Phim",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 15),
                AutoSize = true
            };

            var lblSubtitle = new Label
            {
                Text = "Phân loại phim theo thể loại (Hành động, Hoạt hình, Sci-Fi, Tình cảm...) và số tựa phim trực thuộc.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 47),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(pnlActionRight);
            this.Controls.Add(pnlHeader);
        }

        private void BuildFilterPanel()
        {
            pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(25, 15, 25, 15)
            };

            var lblSearchIcon = new Label
            {
                Text = "🔍",
                Location = new Point(28, 24),
                Size = new Size(25, 25),
                Font = new Font("Segoe UI", 10F)
            };

            txtSearch = new TextBox
            {
                Location = new Point(55, 22),
                Size = new Size(360, 27),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Tìm kiếm theo tên danh mục..."
            };
            txtSearch.TextChanged += (s, e) => { LoadData(); };

            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(430, 21),
                Size = new Size(110, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(73, 80, 87),
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadData(); };

            pnlFilter.Controls.Add(lblSearchIcon);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnRefresh);
            this.Controls.Add(pnlFilter);
        }

        private void BuildFooterPanel()
        {
            pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White
            };

            lblPageInfo = new Label
            {
                Text = "Hiển thị 0 bản ghi",
                Location = new Point(25, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            btnNextPage = new Button
            {
                Text = ">",
                Size = new Size(40, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(980, 14),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 242, 245),
                Cursor = Cursors.Hand
            };
            btnNextPage.FlatAppearance.BorderSize = 0;
            btnNextPage.Click += (s, e) => { _currentPage++; RenderGridPage(); };

            lblCurrentPage = new Label
            {
                Text = "1 / 1",
                Size = new Size(60, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(915, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };

            btnPrevPage = new Button
            {
                Text = "<",
                Size = new Size(40, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(870, 14),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 242, 245),
                Cursor = Cursors.Hand
            };
            btnPrevPage.FlatAppearance.BorderSize = 0;
            btnPrevPage.Click += (s, e) => { if (_currentPage > 1) { _currentPage--; RenderGridPage(); } };

            pnlFooter.Controls.Add(lblPageInfo);
            pnlFooter.Controls.Add(btnPrevPage);
            pnlFooter.Controls.Add(lblCurrentPage);
            pnlFooter.Controls.Add(btnNextPage);
            this.Controls.Add(pnlFooter);
        }

        private void BuildDataGrid()
        {
            var pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25, 0, 25, 10),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            dgvDanhMuc = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 44 }
            };

            dgvDanhMuc.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(243, 244, 246),
                ForeColor = Color.FromArgb(73, 80, 87),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(10, 8, 10, 8)
            };

            dgvDanhMuc.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                SelectionBackColor = Color.FromArgb(250, 235, 235),
                SelectionForeColor = Color.FromArgb(184, 123, 125),
                Padding = new Padding(10, 0, 10, 0)
            };

            dgvDanhMuc.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(252, 253, 254)
            };

            dgvDanhMuc.AutoGenerateColumns = false;
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(TheLoaiDTO.MaTheLoai),
                HeaderText = "Mã Danh Mục",
                Width = 130
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(TheLoaiDTO.TenTheLoai),
                HeaderText = "Tên Danh Mục Phim (Thể Loại)",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(TheLoaiDTO.SoLuongPhim),
                HeaderText = "Số Tựa Phim Trực Thuộc",
                Width = 210,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvDanhMuc.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colActionEdit",
                HeaderText = "Hành Động",
                Text = "✏️ Sửa",
                UseColumnTextForButtonValue = true,
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvDanhMuc.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colActionDelete",
                HeaderText = "",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvDanhMuc.CellClick += DgvDanhMuc_CellClick;
            dgvDanhMuc.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditCategoryAtRow(e.RowIndex); };

            pnlGridContainer.Controls.Add(dgvDanhMuc);
            this.Controls.Add(pnlGridContainer);
            pnlGridContainer.BringToFront();
        }

        #endregion

        #region NGHIỆP VỤ & SỰ KIỆN

        private void LoadData()
        {
            string? keyword = txtSearch.Text;
            _allTheLoais = _phimService.GetAllTheLoai(keyword);
            _currentPage = 1;
            RenderGridPage();
        }

        private void RenderGridPage()
        {
            int totalRecords = _allTheLoais.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (_currentPage > totalPages) _currentPage = totalPages;

            var pageData = _allTheLoais
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            dgvDanhMuc.DataSource = pageData;

            int startRecord = totalRecords == 0 ? 0 : (_currentPage - 1) * _pageSize + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);
            lblPageInfo.Text = $"Hiển thị {startRecord} - {endRecord} của {totalRecords} danh mục";
            lblCurrentPage.Text = $"{_currentPage} / {totalPages}";

            btnPrevPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < totalPages;
        }

        private void DgvDanhMuc_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvDanhMuc.Columns[e.ColumnIndex].Name;
            if (colName == "colActionEdit")
            {
                EditCategoryAtRow(e.RowIndex);
            }
            else if (colName == "colActionDelete")
            {
                DeleteCategoryAtRow(e.RowIndex);
            }
        }

        private void BtnAddCategory_Click(object? sender, EventArgs e)
        {
            using var dlg = new TheLoaiEditDialogForm();
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                var result = _phimService.AddTheLoai(dlg.TenTheLoai);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EditCategoryAtRow(int rowIndex)
        {
            if (dgvDanhMuc.Rows[rowIndex].DataBoundItem is not TheLoaiDTO selectedTheLoai) return;

            using var dlg = new TheLoaiEditDialogForm(selectedTheLoai);
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                var result = _phimService.UpdateTheLoai(selectedTheLoai.MaTheLoai, dlg.TenTheLoai);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteCategoryAtRow(int rowIndex)
        {
            if (dgvDanhMuc.Rows[rowIndex].DataBoundItem is not TheLoaiDTO selectedTheLoai) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa danh mục '{selectedTheLoai.TenTheLoai}' (Mã: {selectedTheLoai.MaTheLoai}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = _phimService.DeleteTheLoai(selectedTheLoai.MaTheLoai);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion
    }
}
