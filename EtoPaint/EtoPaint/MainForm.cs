using Eto.Drawing;
using Eto.Forms;

namespace EtoPaint
{
	public partial class MainForm : Form
	{
		private Drawable Canvas;

		public MainForm()
		{
			Title = "EtoPaint";
			ClientSize = new Size(640, 480);

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
							new Command((s, e) => MessageBox.Show(this, "hello"))
							{
								MenuText = "&New",
								Shortcut = Keys.F2
							},

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
					}
				}
			};

			Canvas = new Drawable();

			Content = new TableLayout()
			{
				Rows =
				{
					new TableRow()
					{
						ScaleHeight = true,
						Cells =
						{
							Canvas
						}
					},
					new TableRow()
					{
						Cells =
						{
							new StackLayout
							{
								Orientation = Orientation.Horizontal,
								HorizontalContentAlignment = HorizontalAlignment.Stretch,
								Items =
								{
									new Button() { Text = "Button 1" },
									new Button() { Text = "Button 2" },
									new Button() { Text = "Button 3" },
									new Button() { Text = "Button 4" }
								}
							}
						}
					}
				}
			};
		}
	}
}
