using CalculatorSuite.Controls;
using PdfiumViewer;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;

namespace CalculatorSuite;

public class Code39Control : UserControl
{
    readonly TextBox data = Ui.Input();
    readonly PictureBox pic = new() { SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.White };
    readonly Panel settings = new() { Visible = false, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
    readonly NumericUpDown imageWidth = new() { Minimum = 100, Maximum = 2000, Value = 700, Increment = 50, ThousandsSeparator = true };
    readonly NumericUpDown imageHeight = new() { Minimum = 50, Maximum = 1000, Value = 180, Increment = 10, ThousandsSeparator = true };
    readonly NumericUpDown margin = new() { Minimum = 0, Maximum = 100, Value = 10 };
    readonly ComboBox format = Ui.Combo("PNG", "GIF");
    readonly ComboBox rotation = Ui.Combo("0°", "90°");

    public Code39Control()
    {
        var root = Ui.Card("Code 39 Barcode Generator");
        root.Controls.Add(Ui.Label("Barcode data"));
        data.Top = 75; data.Width = 600; data.Text = "CODE39-123"; root.Controls.Add(data);
        var generate = Ui.Button("Generate"); generate.Top = 120; generate.Click += (_, _) => Generate(); root.Controls.Add(generate);
        var save = Ui.Button("Save PNG"); save.Top = 170; save.Click += (_, _) => Save(); root.Controls.Add(save);
        var settingsButton = Ui.Button("Settings"); settingsButton.Top = 220; settingsButton.Left = 22; settingsButton.Width = 140; settingsButton.Click += (_, _) => { settings.Visible = true; settings.BringToFront(); }; root.Controls.Add(settingsButton);
        pic.Top = 275; pic.Left = 22; pic.Width = 700; pic.Height = 220; root.Controls.Add(pic);
        BuildSettings(); root.Controls.Add(settings); settings.BringToFront(); Controls.Add(root); Generate();
    }

    void BuildSettings()
    {
        settings.Dock = DockStyle.Fill; settings.Padding = new Padding(20);
        var title = Ui.Label("Settings"); title.Font = Ui.H2; settings.Controls.Add(title);
        AddSetting("Image Width", imageWidth, 45); AddSetting("Image Height", imageHeight, 105); AddSetting("Margin", margin, 165); AddSetting("Image Format", format, 225); AddSetting("Image Rotation", rotation, 285);
        var close = Ui.Button("Close"); close.Top = 355; close.Left = 20; close.Width = 100; close.Click += (_, _) => settings.Visible = false; settings.Controls.Add(close);
    }

    void AddSetting(string text, Control control, int top)
    {
        var label = Ui.Label(text); label.Top = top; label.Left = 20; settings.Controls.Add(label);
        control.Top = top + 25; control.Left = 20; control.Width = 240; settings.Controls.Add(control);
    }

    void Generate()
    {
        try
        {
            var writer = new BarcodeWriterPixelData { Format = BarcodeFormat.CODE_39, Options = new EncodingOptions { Width = (int)imageWidth.Value, Height = (int)imageHeight.Value, Margin = (int)margin.Value } };
            var pixels = writer.Write(data.Text);
            using var source = new Bitmap(pixels.Width, pixels.Height, PixelFormat.Format32bppRgb);
            var bits = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.WriteOnly, source.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(pixels.Pixels, 0, bits.Scan0, pixels.Pixels.Length); source.UnlockBits(bits);
            var result = new Bitmap(source); if (rotation.SelectedIndex == 1) result.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var old = pic.Image; pic.Image = result; old?.Dispose();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Code 39"); }
    }

    void Save()
    {
        if (pic.Image == null) return;
        var extension = format.SelectedIndex == 0 ? "png" : "gif";
        using var dialog = new SaveFileDialog { Filter = $"{format.Text} Image|*.{extension}", FileName = $"code39.{extension}" };
        if (dialog.ShowDialog() == DialogResult.OK) pic.Image.Save(dialog.FileName, format.SelectedIndex == 0 ? ImageFormat.Png : ImageFormat.Gif);
    }
}

public class Pdf417Control : UserControl
{
    readonly TextBox data = new() { Multiline = true, ScrollBars = ScrollBars.Vertical, Text = "x4YY++pZMyuhIfl/gEc6DjwZC1rDI7y48mAuFQKOA3hjBS1FFERdCkVPhgBhoVk47ra5kpdKUjYIwhHkTo+wC6o3Mzh7boB1SjbFh/nbWu9HThQKEfrmuGVGBdiWXBNUEaeBNXDGQkFZRSoUapDZlw==,data" };
    readonly ComboBox moduleUnit = Ui.Combo("Fit (Default)");
    readonly TextBox moduleWidth = new() { Text = "auto" };
    readonly ComboBox dpi = Ui.Combo("96", "150", "300");
    readonly ComboBox format = Ui.Combo("GIF (Default)", "PNG");
    readonly ComboBox rotation = Ui.Combo("0°", "90°");
    readonly CheckBox onePerRow = new() { Text = "Generate one barcode per row", AutoSize = true };
    readonly CheckBox escapeSequences = new() { Text = "Evaluate escape sequences", AutoSize = true };
    readonly PictureBox pic = new() { SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.White };
    readonly Panel settings = new() { Visible = false, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

    public Pdf417Control()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Color.White };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42)); root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));
        var left = Ui.Card("PDF417 Barcode Generator");
        var heading = Ui.Label("PDF417  ⓘ"); heading.Font = Ui.H2; heading.Top = 18; left.Controls.Add(heading);
        var settingsButton = Ui.Button("Settings"); settingsButton.Top = 440; settingsButton.Left = 22; settingsButton.Width = 140; settingsButton.Click += (_, _) => { settings.Visible = true; settings.BringToFront(); }; left.Controls.Add(settingsButton);
        var label = Ui.Label("Data"); label.Top = 58; left.Controls.Add(label); data.Top = 82; data.Left = 22; data.Width = 350; data.Height = 190; left.Controls.Add(data);
        onePerRow.Top = 290; onePerRow.Left = 22; onePerRow.CheckedChanged += (_, _) => Generate(); left.Controls.Add(onePerRow);
        escapeSequences.Top = 320; escapeSequences.Left = 22; escapeSequences.CheckedChanged += (_, _) => Generate(); left.Controls.Add(escapeSequences);
        var help = Ui.Label("Use \\F for FNC1, \\t for TAB, and \\n for ENTER."); help.Top = 350; help.ForeColor = Ui.Muted; left.Controls.Add(help);
        var refresh = Ui.Button("Refresh"); refresh.Top = 395; refresh.Left = 22; refresh.Width = 110; refresh.Click += (_, _) => Generate(); left.Controls.Add(refresh);
        var right = new TableLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, ColumnCount = 1, RowCount = 2 };
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); right.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        pic.Dock = DockStyle.Fill; pic.Padding = new Padding(25); right.Controls.Add(pic, 0, 0);
        var save = Ui.Button("📥  Download"); save.Dock = DockStyle.Fill; save.Font = new Font("Segoe UI", 12, FontStyle.Bold); save.BackColor = Color.FromArgb(211, 47, 47); save.Click += (_, _) => Save(); right.Controls.Add(save, 0, 1);
        BuildSettings(); left.Controls.Add(settings); settings.BringToFront(); root.Controls.Add(left, 0, 0); root.Controls.Add(right, 1, 0); Controls.Add(root); Generate();
    }

    void BuildSettings()
    {
        settings.Dock = DockStyle.Fill; settings.Padding = new Padding(20);
        var title = Ui.Label("Settings"); title.Font = Ui.H2; settings.Controls.Add(title);
        AddSetting("Module Width Unit", moduleUnit, 45); AddSetting("Module Width", moduleWidth, 105); AddSetting("Image Resolution (DPI)", dpi, 165); AddSetting("Image Format", format, 265); AddSetting("Image Rotation", rotation, 325);
        var close = Ui.Button("Close"); close.Top = 385; close.Left = 20; close.Width = 100; close.Click += (_, _) => settings.Visible = false; settings.Controls.Add(close);
    }

    void AddSetting(string text, Control control, int top)
    {
        var label = Ui.Label(text); label.Top = top; label.Left = 20; settings.Controls.Add(label); control.Top = top + 25; control.Left = 20; control.Width = 240; settings.Controls.Add(control);
    }

    void Generate()
    {
        try
        {
            var values = onePerRow.Checked ? data.Text.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries) : [data.Text];
            using var result = Combine(values.Select(CreateBarcode).ToList()); var old = pic.Image; pic.Image = new Bitmap(result); old?.Dispose();
        }
        catch (Exception ex) { MessageBox.Show("Barcode generation failed: " + ex.Message, "PDF417"); }
    }

    Bitmap CreateBarcode(string value)
    {
        var writer = new BarcodeWriterPixelData { Format = BarcodeFormat.PDF_417, Options = new EncodingOptions { Width = 700, Height = 180, Margin = 10 } };
        var pixels = writer.Write(value); var bitmap = new Bitmap(pixels.Width, pixels.Height, PixelFormat.Format32bppRgb); var bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat); System.Runtime.InteropServices.Marshal.Copy(pixels.Pixels, 0, bits.Scan0, pixels.Pixels.Length); bitmap.UnlockBits(bits); return bitmap;
    }

    Bitmap Combine(List<Bitmap> images)
    {
        var result = new Bitmap(images.Max(x => x.Width), images.Sum(x => x.Height), PixelFormat.Format32bppArgb); using var graphics = Graphics.FromImage(result); var y = 0; foreach (var image in images) { graphics.DrawImageUnscaled(image, 0, y); y += image.Height; image.Dispose(); } return result;
    }

    void Save()
    {
        if (pic.Image == null) return; using var dialog = new SaveFileDialog { Filter = "Image file|*.png", FileName = "pdf417.png" }; if (dialog.ShowDialog() == DialogResult.OK) pic.Image.Save(dialog.FileName, ImageFormat.Png);
    }
}

public class PdfToImageControl : UserControl
{
    readonly Label status = new() { AutoSize = true, ForeColor = Ui.Muted };
    readonly FlowLayoutPanel previews = new() { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10), BackColor = Color.White };
    readonly List<Bitmap> images = [];

    public PdfToImageControl()
    {
        var card = Ui.Card("PDF to Image Converter"); var open = Ui.Button("Choose PDF"); open.Top = 65; open.Click += (_, _) => Open(); var save = Ui.Button("Save Selected Page"); save.Top = 115; save.Click += (_, _) => SaveSelected(); status.Top = 170; card.Controls.AddRange([open, save, status, previews]); Controls.Add(card);
    }

    void Open()
    {
        using var dialog = new OpenFileDialog { Filter = "PDF files|*.pdf" }; if (dialog.ShowDialog() != DialogResult.OK) return;
        foreach (var image in images) image.Dispose(); images.Clear(); previews.Controls.Clear();
        using var document = PdfDocument.Load(dialog.FileName); for (var page = 0; page < document.PageCount; page++) { var size = document.PageSizes[page]; var image = document.Render(page, (int)size.Width * 2, (int)size.Height * 2, 96, 96, PdfRenderFlags.Annotations); images.Add(new Bitmap(image)); var preview = new PictureBox { Image = images[^1], SizeMode = PictureBoxSizeMode.Zoom, Width = 300, Height = 400, Tag = images.Count - 1 }; previews.Controls.Add(preview); }
        status.Text = $"Loaded {images.Count} page(s).";
    }

    void SaveSelected()
    {
        var selected = previews.Controls.OfType<PictureBox>().FirstOrDefault(x => x.BorderStyle == BorderStyle.FixedSingle) ?? previews.Controls.OfType<PictureBox>().FirstOrDefault(); if (selected?.Image == null) return; using var dialog = new SaveFileDialog { Filter = "PNG image|*.png", FileName = "page.png" }; if (dialog.ShowDialog() == DialogResult.OK) selected.Image.Save(dialog.FileName, ImageFormat.Png);
    }
}
