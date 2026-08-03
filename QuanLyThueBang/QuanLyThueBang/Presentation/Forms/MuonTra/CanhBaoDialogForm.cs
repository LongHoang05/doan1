using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.MuonTra
{
    public class CanhBaoDialogForm : Form
    {
        private BangQuaHanDTO _data;

        public CanhBaoDialogForm(BangQuaHanDTO data)
        {
            _data = data;
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Cảnh báo quá hạn";
            this.Size = new Size(400, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Color bgColor = Color.FromArgb(252, 238, 237);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = bgColor };
            var lblTitle = new Label { Text = "👁 Xem trước tin nhắn", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 40, 80), AutoSize = true, Location = new Point(15, 15) };
            pnlHeader.Controls.Add(lblTitle);

            var pnlChat = new Panel { Dock = DockStyle.Fill, BackColor = bgColor };

            var flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(25, 15, 25, 15),
                AutoScroll = true
            };

            string msg1 = $"Chào {_data.HoTenKhachHang},\nĐây là tin nhắn nhắc nhở từ cửa hàng cho thuê băng đĩa. Cuốn băng '{_data.TuaDe}' của bạn đã hết hạn trả vào ngày {_data.NgayDuKienTra:dd/MM/yyyy}.";
            string msg2 = "Vui lòng mang trả băng sớm nhất có thể.\nNếu bạn muốn gia hạn thêm, vui lòng phản hồi lại tin nhắn này.";

            flowLayout.Controls.Add(CreateChatBubble(msg1));
            flowLayout.Controls.Add(new Panel { Height = 5, BackColor = Color.Transparent }); // Spacer
            flowLayout.Controls.Add(CreateChatBubble(msg2));

            pnlChat.Controls.Add(flowLayout);

            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.White };
            var btnSend = new Button { Text = "Gửi Tin Nhắn", DialogResult = DialogResult.OK, Width = 140, Height = 38, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(13, 110, 253), ForeColor = Color.White, Cursor = Cursors.Hand, Font = new Font("Segoe UI Semibold", 10F) };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Location = new Point(this.ClientSize.Width - btnSend.Width - 15, 13);
            
            var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 80, Height = 38, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(241, 243, 245), ForeColor = Color.Black, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 10F) };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Location = new Point(btnSend.Left - btnCancel.Width - 10, 13);

            pnlFooter.Controls.Add(btnSend);
            pnlFooter.Controls.Add(btnCancel);

            this.Controls.Add(pnlChat);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlFooter);
        }

        private Panel CreateChatBubble(string text)
        {
            var pnl = new Panel
            {
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Width = 310
            };

            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 30, 30),
                MaximumSize = new Size(270, 0),
                AutoSize = true,
                Location = new Point(18, 15),
                BackColor = Color.White
            };

            pnl.Height = lbl.PreferredHeight + 30;
            pnl.Controls.Add(lbl);

            pnl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var gp = new GraphicsPath();
                int r = 12;
                var rect = new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1);
                gp.AddArc(rect.X, rect.Y, r, r, 180, 90);
                gp.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                gp.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                gp.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                gp.CloseFigure();

                using var brush = new SolidBrush(Color.White);
                e.Graphics.FillPath(brush, gp);
            };

            return pnl;
        }
    }
}
