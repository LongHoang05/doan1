using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.Phim
{
    /// <summary>
    /// Màn hình Quản lý Phim - Triển khai theo Giao diện chuẩn 2 (Full Data Table + Filter Bar + Pagination)
    /// </summary>
    public class QuanLyPhimForm : Form
    {
        private readonly PhimService _phimService;

        // Dữ liệu & Phân trang
        private List<PhimDTO> _allPhims = new List<PhimDTO>();
        private int _currentPage = 1;
        private int _pageSize = 15;

        // UI Controls
        private Panel pnlHeader = null!;
        private Panel pnlFilter = null!;
        private Panel pnlFooter = null!;
        private DataGridView dgvPhim = null!;

        private TextBox txtSearch = null!;
        private ComboBox cboTheLoaiFilter = null!;
        private Button btnAddMovie = null!;
        private Button btnEditMovie = null!;
        private Button btnDeleteMovie = null!;

        private Label lblPageInfo = null!;
        private Button btnPrevPage = null!;
        private Button btnNextPage = null!;
        private Label lblCurrentPage = null!;

        public QuanLyPhimForm(PhimService phimService)
        {
            _phimService = phimService ?? throw new ArgumentNullException(nameof(phimService));
            InitializeComponent();
            this.Load += QuanLyPhimForm_Load;
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ thống Quản lý Thuê Băng - Quản lý Danh mục Phim";
            this.Size = new Size(1150, 720);
            this.MinimumSize = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);

            BuildHeaderPanel();
            BuildFilterPanel();
            BuildFooterPanel();
            BuildDataGrid();
        }

        #region BỐ CỤC GIAO DIỆN (UI BUILDERS)

        private void BuildHeaderPanel()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.White,
                Padding = new Padding(25, 15, 25, 15)
            };

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
                Text = "Quản lý danh sách tựa phim gốc, thể loại, năm phát hành và số lượng bản sao trong kho.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 47),
                AutoSize = true
            };

            btnAddMovie = new Button
            {
                Text = "+ Thêm Phim Mới",
                Size = new Size(145, 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlHeader.Width - 170, 22),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125), // Tone hồng/đất ấm áp theo ảnh mẫu 2
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddMovie.FlatAppearance.BorderSize = 0;
            btnAddMovie.Click += BtnAddMovie_Click;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(btnAddMovie);
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

            // Search Box
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
                Size = new Size(320, 27),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Tìm kiếm theo mã phim hoặc tựa đề..."
            };
            txtSearch.TextChanged += (s, e) => { LoadData(); };

            // ComboBox Lọc theo Thể loại
            cboTheLoaiFilter = new ComboBox
            {
                Location = new Point(390, 22),
                Size = new Size(210, 27),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboTheLoaiFilter.SelectedIndexChanged += (s, e) => { LoadData(); };

            // Các nút hành động Edit / Delete góc phải
            btnDeleteMovie = new Button
            {
                Text = "Xóa Phim",
                Size = new Size(100, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlFilter.Width - 135, 18),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnDeleteMovie.FlatAppearance.BorderSize = 0;
            btnDeleteMovie.Click += BtnDeleteMovie_Click;

            btnEditMovie = new Button
            {
                Text = "Sửa Phim",
                Size = new Size(100, 34),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlFilter.Width - 245, 18),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnEditMovie.FlatAppearance.BorderSize = 0;
            btnEditMovie.Click += BtnEditMovie_Click;

            pnlFilter.Controls.Add(lblSearchIcon);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(cboTheLoaiFilter);
            pnlFilter.Controls.Add(btnEditMovie);
            pnlFilter.Controls.Add(btnDeleteMovie);
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
                Location = new Point(pnlFooter.Width - 65, 14),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 242, 245),
                Cursor = Cursors.Hand
            };
            btnNextPage.FlatAppearance.BorderSize = 0;
            btnNextPage.Click += (s, e) => { _currentPage++; RenderGridPage(); };

            lblCurrentPage = new Label
            {
                Text = "1",
                Size = new Size(50, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlFooter.Width - 120, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };

            btnPrevPage = new Button
            {
                Text = "<",
                Size = new Size(40, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlFooter.Width - 165, 14),
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

            dgvPhim = new DataGridView
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
                RowTemplate = { Height = 40 }
            };

            // Thiết kế Header Grid hiện đại
            dgvPhim.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(243, 244, 246),
                ForeColor = Color.FromArgb(73, 80, 87),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(10, 8, 10, 8)
            };

            dgvPhim.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                SelectionBackColor = Color.FromArgb(250, 235, 235), // Màu trỏ hover nhạt
                SelectionForeColor = Color.FromArgb(184, 123, 125),
                Padding = new Padding(10, 0, 10, 0)
            };

            dgvPhim.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(252, 253, 254)
            };

            // Cấu hình cột
            dgvPhim.AutoGenerateColumns = false;
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PhimDTO.MaPhim),
                HeaderText = "Mã Phim",
                Width = 110
            });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PhimDTO.TuaDe),
                HeaderText = "Tựa Đề Phim",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PhimDTO.TenTheLoai),
                HeaderText = "Thể Loại",
                Width = 150
            });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PhimDTO.NamPhatHanh),
                HeaderText = "Năm PH",
                Width = 100,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PhimDTO.DoDaiPhut),
                HeaderText = "Thời Lượng (phút)",
                Width = 150,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PhimDTO.SoLuongBanSao),
                HeaderText = "Băng Trong Kho",
                Width = 140,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Hành động: Sửa
            dgvPhim.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colActionEdit",
                HeaderText = "Hành Động",
                Text = "✏️ Sửa",
                UseColumnTextForButtonValue = true,
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Hành động: Xóa
            dgvPhim.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colActionDelete",
                HeaderText = "",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvPhim.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditMovie_Click(s, e); };
            dgvPhim.CellClick += DgvPhim_CellClick;

            pnlGridContainer.Controls.Add(dgvPhim);
            this.Controls.Add(pnlGridContainer);
            pnlGridContainer.BringToFront();
        }

        private void DgvPhim_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvPhim.Columns[e.ColumnIndex].Name;
            if (colName == "colActionEdit")
            {
                BtnEditMovie_Click(sender, e);
            }
            else if (colName == "colActionDelete")
            {
                BtnDeleteMovie_Click(sender, e);
            }
        }

        #endregion

        #region XỬ LÝ DỮ LIỆU & SỰ KIỆN

        private void QuanLyPhimForm_Load(object? sender, EventArgs e)
        {
            LoadCategoryFilter();
            LoadData();
        }

        private void LoadCategoryFilter()
        {
            var theLoais = _phimService.GetAllTheLoai();
            var filterList = new List<TheLoaiDTO>
            {
                new TheLoaiDTO { MaTheLoai = 0, TenTheLoai = "Tất Cả Thể Loại" }
            };
            filterList.AddRange(theLoais);

            cboTheLoaiFilter.DataSource = filterList;
            cboTheLoaiFilter.DisplayMember = "TenTheLoai";
            cboTheLoaiFilter.ValueMember = "MaTheLoai";
            cboTheLoaiFilter.SelectedIndex = 0;
        }

        private void LoadData()
        {
            string? keyword = txtSearch.Text;
            int? filterMaTheLoai = null;
            if (cboTheLoaiFilter.SelectedValue is int maTL && maTL > 0)
            {
                filterMaTheLoai = maTL;
            }

            _allPhims = _phimService.GetAllPhim(keyword, filterMaTheLoai);
            _currentPage = 1;
            RenderGridPage();
        }

        private void RenderGridPage()
        {
            int totalRecords = _allPhims.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (_currentPage > totalPages) _currentPage = totalPages;

            var pageData = _allPhims
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            dgvPhim.DataSource = pageData;

            // Cập nhật Footer phân trang
            int startRecord = totalRecords == 0 ? 0 : (_currentPage - 1) * _pageSize + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);
            lblPageInfo.Text = $"Hiển thị {startRecord} - {endRecord} của {totalRecords} tựa phim";
            lblCurrentPage.Text = $"{_currentPage} / {totalPages}";

            btnPrevPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < totalPages;
        }

        private void BtnAddMovie_Click(object? sender, EventArgs e)
        {
            var theLoais = _phimService.GetAllTheLoai();
            if (theLoais.Count == 0)
            {
                MessageBox.Show("Hệ thống chưa có Thể loại phim nào. Vui lòng thêm Thể loại trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new PhimEditDialogForm(theLoais);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var result = _phimService.AddPhim(dlg.MaPhim, dlg.TuaDe, dlg.NamPhatHanh, dlg.DoDaiPhut, dlg.MaTheLoai);
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

        private void BtnEditMovie_Click(object? sender, EventArgs e)
        {
            if (dgvPhim.CurrentRow?.DataBoundItem is not PhimDTO selectedPhim)
            {
                MessageBox.Show("Vui lòng chọn bộ phim cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var theLoais = _phimService.GetAllTheLoai();
            using var dlg = new PhimEditDialogForm(theLoais, selectedPhim);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var result = _phimService.UpdatePhim(selectedPhim.MaPhim, dlg.TuaDe, dlg.NamPhatHanh, dlg.DoDaiPhut, dlg.MaTheLoai);
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

        private void BtnDeleteMovie_Click(object? sender, EventArgs e)
        {
            if (dgvPhim.CurrentRow?.DataBoundItem is not PhimDTO selectedPhim)
            {
                MessageBox.Show("Vui lòng chọn bộ phim cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tựa phim '{selectedPhim.TuaDe}' ({selectedPhim.MaPhim}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = _phimService.DeletePhim(selectedPhim.MaPhim);
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
