using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Presentation.Forms.BanSao;

namespace QuanLyThueBang.Presentation.Controls
{
    public class QuanLyBanSaoControl : UserControl
    {
        private readonly BanSaoBangService _banSaoService;
        private readonly PhimService _phimService;

        private List<BanSaoBangViewDTO> _allBanSaos = new List<BanSaoBangViewDTO>();
        private List<PhimDTO> _allPhims = new List<PhimDTO>();
        private readonly System.Windows.Forms.Timer _timerFilterPhim = new System.Windows.Forms.Timer { Interval = 150 };
        private int _currentPage = 1;
        private int _pageSize = 15;
        private bool _isInitFilter = false;

        private Panel pnlHeader = null!;
        private Panel pnlFilter = null!;
        private Panel pnlFooter = null!;
        private DataGridView dgvBanSao = null!;

        private TextBox txtSearch = null!;
        private ComboBox cboPhimFilter = null!;
        private ComboBox cboTrangThaiFilter = null!;
        private Button btnAddBanSao = null!;

        private Label lblPageInfo = null!;
        private Button btnPrevPage = null!;
        private Button btnNextPage = null!;
        private Label lblCurrentPage = null!;

        public QuanLyBanSaoControl(BanSaoBangService banSaoService, PhimService phimService)
        {
            _banSaoService = banSaoService ?? throw new ArgumentNullException(nameof(banSaoService));
            _phimService = phimService ?? throw new ArgumentNullException(nameof(phimService));

            InitializeComponent();
            this.Load += (s, e) =>
            {
                InitFilterDropdowns();
                LoadData();
            };
        }

        private void InitFilterDropdowns()
        {
            _isInitFilter = true;
            _allPhims = _phimService.GetAllPhim();
            var listPhimFilter = new List<PhimDTO>
            {
                new PhimDTO { MaPhim = "ALL", TuaDe = "--- Tất cả tựa phim ---" }
            };
            listPhimFilter.AddRange(_allPhims);

            cboPhimFilter.BeginUpdate();
            cboPhimFilter.DataSource = null;
            cboPhimFilter.DisplayMember = nameof(PhimDTO.TuaDe);
            cboPhimFilter.ValueMember = nameof(PhimDTO.MaPhim);
            cboPhimFilter.DataSource = listPhimFilter;
            if (cboPhimFilter.Items.Count > 0) cboPhimFilter.SelectedIndex = 0;
            cboPhimFilter.EndUpdate();

            cboTrangThaiFilter.Items.Clear();
            cboTrangThaiFilter.Items.AddRange(new object[] { "ALL", "Sẵn sàng", "Đang cho mượn", "Bảo trì", "Thất lạc" });
            if (cboTrangThaiFilter.Items.Count > 0) cboTrangThaiFilter.SelectedIndex = 0;
            _isInitFilter = false;
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

        private void BuildHeaderPanel()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.White,
                Padding = new Padding(25, 15, 25, 15)
            };

            var pnlActionRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 260,
                BackColor = Color.Transparent
            };

            btnAddBanSao = new Button
            {
                Text = "+ Nhập Bản Sao Mới",
                Size = new Size(230, 42),
                Location = new Point(15, 21),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 0, 0, 4),
                UseCompatibleTextRendering = true
            };
            btnAddBanSao.FlatAppearance.BorderSize = 0;
            btnAddBanSao.Click += BtnAddBanSao_Click;
            pnlActionRight.Controls.Add(btnAddBanSao);

            var lblTitle = new Label
            {
                Text = "Quản Lý Bản Sao Băng (Kho Băng)",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 15),
                AutoSize = true
            };

            var lblSubtitle = new Label
            {
                Text = "Quản lý mã vạch từng cuốn băng vật lý, trạng thái kho (Sẵn sàng, đang cho mượn) và đơn giá thuê.",
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
                Size = new Size(280, 27),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Tìm mã vạch hoặc tên phim..."
            };
            txtSearch.TextChanged += (s, e) => { LoadData(); };

            cboPhimFilter = new ComboBox
            {
                Location = new Point(350, 22),
                Size = new Size(230, 27),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.None,
                Font = new Font("Segoe UI", 10F)
            };
            _timerFilterPhim.Tick += (s, e) => { _timerFilterPhim.Stop(); FilterPhimSuggestions(); };
            cboPhimFilter.TextUpdate += (s, e) => { _timerFilterPhim.Stop(); _timerFilterPhim.Start(); };
            cboPhimFilter.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    if (cboPhimFilter.DroppedDown) cboPhimFilter.DroppedDown = false;
                    LoadData();
                }
            };
            cboPhimFilter.SelectionChangeCommitted += (s, e) =>
            {
                if (cboPhimFilter.SelectedItem != null && !_isInitFilter)
                {
                    LoadData();
                }
            };

            cboTrangThaiFilter = new ComboBox
            {
                Location = new Point(595, 22),
                Size = new Size(160, 27),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboTrangThaiFilter.SelectedIndexChanged += (s, e) => { if (!_isInitFilter) LoadData(); };

            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(770, 21),
                Size = new Size(125, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(73, 80, 87),
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            btnRefresh.Click += (s, e) =>
            {
                _isInitFilter = true;
                txtSearch.Clear();
                InitFilterDropdowns();
                if (cboPhimFilter.DroppedDown) cboPhimFilter.DroppedDown = false;
                if (cboTrangThaiFilter.Items.Count > 0) cboTrangThaiFilter.SelectedIndex = 0;
                _isInitFilter = false;
                LoadData();
            };

            pnlFilter.Controls.Add(lblSearchIcon);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(cboPhimFilter);
            pnlFilter.Controls.Add(cboTrangThaiFilter);
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

            dgvBanSao = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeight = 46,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
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

            dgvBanSao.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(243, 244, 246),
                ForeColor = Color.FromArgb(73, 80, 87),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 4, 8, 4)
            };

            dgvBanSao.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                SelectionBackColor = Color.FromArgb(250, 235, 235),
                SelectionForeColor = Color.FromArgb(184, 123, 125),
                Padding = new Padding(10, 0, 10, 0)
            };

            dgvBanSao.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(252, 253, 254)
            };

            dgvBanSao.AutoGenerateColumns = false;

            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BanSaoBangViewDTO.MaBanSao),
                HeaderText = "Mã Bản Sao / RFID",
                Width = 180,
                MinimumWidth = 160
            });

            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BanSaoBangViewDTO.TuaDePhim),
                HeaderText = "Tựa Phim Gốc",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 200
            });

            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BanSaoBangViewDTO.LoaiBang),
                HeaderText = "Định Dạng",
                Width = 135,
                MinimumWidth = 125
            });

            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BanSaoBangViewDTO.DonGiaThue),
                HeaderText = "Đơn Giá Thuê",
                Width = 155,
                MinimumWidth = 140,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }
            });

            dgvBanSao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(BanSaoBangViewDTO.TrangThai),
                HeaderText = "Trạng Thái Kho",
                Width = 165,
                MinimumWidth = 150,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvBanSao.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colActionEdit",
                HeaderText = "Hành Động",
                Text = "✏️ Sửa",
                UseColumnTextForButtonValue = true,
                Width = 155,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvBanSao.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colActionDelete",
                HeaderText = "",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                Width = 125,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvBanSao.CellClick += DgvBanSao_CellClick;
            dgvBanSao.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditBanSaoAtRow(e.RowIndex); };

            pnlGridContainer.Controls.Add(dgvBanSao);
            this.Controls.Add(pnlGridContainer);
            pnlGridContainer.BringToFront();
        }

        private void FilterPhimSuggestions()
        {
            if (_isInitFilter) return;
            string search = cboPhimFilter.Text;
            int cursorPos = cboPhimFilter.SelectionStart;

            var filtered = new List<PhimDTO>
            {
                new PhimDTO { MaPhim = "ALL", TuaDe = "--- Tất cả tựa phim ---" }
            };

            if (string.IsNullOrWhiteSpace(search) || search == "--- Tất cả tựa phim ---")
            {
                filtered.AddRange(_allPhims.Take(20));
            }
            else
            {
                string kw = search.Trim().ToLower();
                var matches = _allPhims.Where(p =>
                    p.TuaDe.ToLower().Contains(kw) ||
                    p.MaPhim.ToLower().Contains(kw))
                    .Take(15)
                    .ToList();
                filtered.AddRange(matches);
            }

            _isInitFilter = true;
            cboPhimFilter.BeginUpdate();
            cboPhimFilter.DataSource = null;
            cboPhimFilter.DisplayMember = nameof(PhimDTO.TuaDe);
            cboPhimFilter.ValueMember = nameof(PhimDTO.MaPhim);
            cboPhimFilter.DataSource = filtered;
            cboPhimFilter.SelectedIndex = -1;
            cboPhimFilter.Text = search;
            cboPhimFilter.SelectionStart = cursorPos;
            cboPhimFilter.EndUpdate();
            _isInitFilter = false;

            if (filtered.Count > 1 && !string.IsNullOrWhiteSpace(search) && search != "--- Tất cả tựa phim ---")
            {
                if (!cboPhimFilter.DroppedDown)
                {
                    cboPhimFilter.DroppedDown = true;
                    cboPhimFilter.Text = search;
                    cboPhimFilter.SelectionStart = cursorPos;
                }
            }
            else
            {
                if (cboPhimFilter.DroppedDown) cboPhimFilter.DroppedDown = false;
            }
        }

        private string? GetSelectedMaPhim()
        {
            if (cboPhimFilter.SelectedItem is PhimDTO phim)
            {
                if (phim.MaPhim == "ALL") return "ALL";
                return phim.MaPhim;
            }
            if (cboPhimFilter.SelectedValue is string strId && !string.IsNullOrWhiteSpace(strId))
                return strId;
            string txt = cboPhimFilter.Text.Trim();
            if (string.IsNullOrEmpty(txt) || txt == "--- Tất cả tựa phim ---")
                return "ALL";
            var exactOrPartial = _allPhims.FirstOrDefault(p => p.TuaDe.Equals(txt, StringComparison.OrdinalIgnoreCase) || p.MaPhim.Equals(txt, StringComparison.OrdinalIgnoreCase));
            if (exactOrPartial != null)
                return exactOrPartial.MaPhim;
            return "ALL";
        }

        private void LoadData()
        {
            if (_isInitFilter) return;
            string? keyword = txtSearch.Text;
            string? filterPhim = GetSelectedMaPhim();
            string? filterTrangThai = cboTrangThaiFilter.SelectedItem?.ToString();

            _allBanSaos = _banSaoService.GetAllBanSao(keyword, filterPhim, filterTrangThai);
            _currentPage = 1;
            RenderGridPage();
        }

        private void RenderGridPage()
        {
            int totalRecords = _allBanSaos.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
            if (totalPages == 0) totalPages = 1;
            if (_currentPage > totalPages) _currentPage = totalPages;

            var pageData = _allBanSaos
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            dgvBanSao.DataSource = pageData;

            int startRecord = totalRecords == 0 ? 0 : (_currentPage - 1) * _pageSize + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);
            lblPageInfo.Text = $"Hiển thị {startRecord} - {endRecord} của {totalRecords} bản sao";
            lblCurrentPage.Text = $"{_currentPage} / {totalPages}";

            btnPrevPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < totalPages;
        }

        private void DgvBanSao_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvBanSao.Columns[e.ColumnIndex].Name;
            if (colName == "colActionEdit")
            {
                EditBanSaoAtRow(e.RowIndex);
            }
            else if (colName == "colActionDelete")
            {
                DeleteBanSaoAtRow(e.RowIndex);
            }
        }

        private void BtnAddBanSao_Click(object? sender, EventArgs e)
        {
            var phims = _phimService.GetAllPhim();
            if (phims.Count == 0)
            {
                MessageBox.Show("Hệ thống chưa có Tựa phim gốc nào. Vui lòng thêm Phim trước khi nhập bản sao.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cuaHangs = _banSaoService.GetAllCuaHang();
            using var dlg = new BanSaoEditDialogForm(_banSaoService, phims, cuaHangs);
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                var result = _banSaoService.AddBanSao(dlg.MaPhim, dlg.MaCuaHang, dlg.LoaiBang, dlg.DonGiaThue, dlg.MaBanSao);
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

        private void EditBanSaoAtRow(int rowIndex)
        {
            if (dgvBanSao.Rows[rowIndex].DataBoundItem is not BanSaoBangViewDTO selected) return;

            var phims = _phimService.GetAllPhim();
            var cuaHangs = _banSaoService.GetAllCuaHang();

            using var dlg = new BanSaoEditDialogForm(_banSaoService, phims, cuaHangs, selected);
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                var result = _banSaoService.UpdateBanSao(selected.MaBanSao, dlg.LoaiBang, dlg.DonGiaThue, dlg.TrangThai, dlg.MaCuaHang);
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

        private void DeleteBanSaoAtRow(int rowIndex)
        {
            if (dgvBanSao.Rows[rowIndex].DataBoundItem is not BanSaoBangViewDTO selected) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa bản sao băng '{selected.MaBanSao}' của phim '{selected.TuaDePhim}' không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = _banSaoService.DeleteBanSao(selected.MaBanSao);
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
    }
}
