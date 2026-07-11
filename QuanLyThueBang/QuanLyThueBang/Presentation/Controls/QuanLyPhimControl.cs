using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Presentation.Forms.Phim;

namespace QuanLyThueBang.Presentation.Controls
{
    /// <summary>
    /// UserControl Quản lý Phim - Nhúng trực tiếp vào MainShellForm (Giao diện chuẩn 2)
    /// </summary>
    public class QuanLyPhimControl : UserControl
    {
        private readonly PhimService _phimService;

        private List<PhimDTO> _allPhims = new List<PhimDTO>();
        private int _currentPage = 1;
        private int _pageSize = 15;

        private Panel pnlHeader = null!;
        private Panel pnlFilter = null!;
        private Panel pnlFooter = null!;
        private DataGridView dgvPhim = null!;

        private TextBox txtSearch = null!;
        private ComboBox cboTheLoaiFilter = null!;
        private Button btnAddMovie = null!;

        private Label lblPageInfo = null!;
        private Button btnPrevPage = null!;
        private Button btnNextPage = null!;
        private Label lblCurrentPage = null!;

        public QuanLyPhimControl(PhimService phimService)
        {
            _phimService = phimService ?? throw new ArgumentNullException(nameof(phimService));
            InitializeComponent();
            this.Load += (s, e) =>
            {
                LoadCategoryFilter();
                LoadData();
            };
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

            // Panel chứa nút bên phải (luôn bám phải không bao giờ bị lệch ra ngoài)
            var pnlActionRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 210,
                BackColor = Color.Transparent
            };

            btnAddMovie = new Button
            {
                Text = "+ Thêm Phim Mới",
                Size = new Size(165, 42),
                Location = new Point(20, 21),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125), // Tone màu hồng đất sang trọng
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddMovie.FlatAppearance.BorderSize = 0;
            btnAddMovie.Click += BtnAddMovie_Click;
            pnlActionRight.Controls.Add(btnAddMovie);

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
                Size = new Size(340, 27),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Tìm kiếm theo mã phim hoặc tựa đề..."
            };
            txtSearch.TextChanged += (s, e) => { LoadData(); };

            cboTheLoaiFilter = new ComboBox
            {
                Location = new Point(415, 22),
                Size = new Size(220, 27),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboTheLoaiFilter.SelectedIndexChanged += (s, e) => { LoadData(); };

            // Thêm nút Làm mới / Tải lại ngay thanh filter
            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(650, 21),
                Size = new Size(110, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(73, 80, 87),
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); cboTheLoaiFilter.SelectedIndex = 0; LoadData(); };

            pnlFilter.Controls.Add(lblSearchIcon);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(cboTheLoaiFilter);
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
                RowTemplate = { Height = 42 }
            };

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
                SelectionBackColor = Color.FromArgb(250, 235, 235),
                SelectionForeColor = Color.FromArgb(184, 123, 125),
                Padding = new Padding(10, 0, 10, 0)
            };

            dgvPhim.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(252, 253, 254)
            };

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

            // Cột Nút Hành Động: Sửa
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

            // Cột Nút Hành Động: Xóa
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

            dgvPhim.CellClick += DgvPhim_CellClick;
            dgvPhim.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditMovieAtRow(e.RowIndex); };

            pnlGridContainer.Controls.Add(dgvPhim);
            this.Controls.Add(pnlGridContainer);
            pnlGridContainer.BringToFront();
        }

        #endregion

        #region NGHIỆP VỤ & SỰ KIỆN

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

            int startRecord = totalRecords == 0 ? 0 : (_currentPage - 1) * _pageSize + 1;
            int endRecord = Math.Min(_currentPage * _pageSize, totalRecords);
            lblPageInfo.Text = $"Hiển thị {startRecord} - {endRecord} của {totalRecords} tựa phim";
            lblCurrentPage.Text = $"{_currentPage} / {totalPages}";

            btnPrevPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < totalPages;
        }

        private void DgvPhim_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = dgvPhim.Columns[e.ColumnIndex].Name;
            if (colName == "colActionEdit")
            {
                EditMovieAtRow(e.RowIndex);
            }
            else if (colName == "colActionDelete")
            {
                DeleteMovieAtRow(e.RowIndex);
            }
        }

        private void BtnAddMovie_Click(object? sender, EventArgs e)
        {
            var theLoais = _phimService.GetAllTheLoai();
            if (theLoais.Count == 0)
            {
                MessageBox.Show("Hệ thống chưa có Thể loại phim nào. Vui lòng thêm Thể loại trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nextMaPhim = _phimService.GenerateNextMaPhim();
            using var dlg = new PhimEditDialogForm(theLoais, null, nextMaPhim);
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
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

        private void EditMovieAtRow(int rowIndex)
        {
            if (dgvPhim.Rows[rowIndex].DataBoundItem is not PhimDTO selectedPhim) return;

            var theLoais = _phimService.GetAllTheLoai();
            using var dlg = new PhimEditDialogForm(theLoais, selectedPhim);
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
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

        private void DeleteMovieAtRow(int rowIndex)
        {
            if (dgvPhim.Rows[rowIndex].DataBoundItem is not PhimDTO selectedPhim) return;

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
