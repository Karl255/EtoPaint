using Eto;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.IO;
using System.Timers;

namespace EtoPaint
{
	public partial class MainForm : Form
	{
		private ImageView Img;
		private Bitmap Bmp;
		private readonly byte[] BlankBuffer;
		private byte[] Buffer;
		private Color PaintColor = Color.FromRgb(0xffffff);

		public MainForm()
		{
			Title = "EtoPaint";

			BlankBuffer = File.ReadAllBytes("Blank.bmp");

			// menu
			Menu = new MenuBar
			{
				Items =
				{
					// menu/file
					new ButtonMenuItem
					{
						Text = "&File",
						Items =
						{
							// menu/file/new
							new Command((s, e) => {
								Buffer = (byte[])BlankBuffer.Clone();
								Repaint();
							}) { MenuText = "New", Shortcut = Keys.F2 },

							// menu/file/about
							new ButtonMenuItem((s, e) =>
								new Dialog()
								{
									Content = new Label
									{
										Text = "This is just a test app to see the posibilities and performance of Eto.Forms.",
									},
									ClientSize = new Size(250, 100),
									Padding = new Padding(15)
								}.ShowModal(this)
							)
							{
								Text = "&About..."
							},

							// menu/file/quit
							new Command((s, e) => Application.Instance.Quit())
							{
								MenuText = "&Quit",
								Shortcut = Application.Instance.CommonModifier | Keys.Q
							}
						}
					},
					new ButtonMenuItem((s, e) =>
					{
						var d = new ColorDialog { Color = PaintColor, AllowAlpha = false };
						d.ShowDialog(this);
						PaintColor = d.Color;
					}) { Text = "Change color"}
				}
			};

			Buffer = (byte[])BlankBuffer.Clone();
			Bmp = new Bitmap(Buffer);
			Content = Img = new ImageView { Image = Bmp, Size = new Size(256, 240) };

			//due to a bug, this won't work on linux (Eto issues #1730 on github)
			Img.MouseMove += (s, e) =>
			{
				int x = (int)e.Location.X;
				int y = 239 - (int)e.Location.Y;

				if (e.Buttons.HasFlag(MouseButtons.Primary) && x >= 0 && x <= 255 && y >= 0 && y <= 239)
					Draw(x, y);
			};

			Closing += (s, e) =>
			{
				Bmp.Dispose();
			};
		}

		private void Draw(int x, int y)
		{
			int start = Buffer[0x0A];

			Buffer[start + (y * 256 + x) * 3 + 0] = (byte)PaintColor.Bb;
			Buffer[start + (y * 256 + x) * 3 + 1] = (byte)PaintColor.Gb;
			Buffer[start + (y * 256 + x) * 3 + 2] = (byte)PaintColor.Rb;

			Repaint();
		}

		private void Repaint()
		{
			Bmp.Dispose();
			Bmp = new Bitmap(Buffer);
			Img.Image = Bmp;
		}
	}
}
