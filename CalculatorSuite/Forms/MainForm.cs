using CalculatorSuite.Controls;
using System.Drawing.Printing;
namespace CalculatorSuite;
public class MainForm:Form
{
  Panel content=new(); Label pageTitle=new(); Button exportButton=new(), previewButton=new(), printButton=new();
 readonly Dictionary<string,Func<Control>> pages;
  public MainForm(){Text="Calculator Suite • Native C#";StartPosition=FormStartPosition.CenterScreen;WindowState=FormWindowState.Maximized;AutoScaleMode=AutoScaleMode.Font;MinimumSize=new Size(900,600);BackColor=Ui.Bg;Font=Ui.Body;var iconPath=Path.Combine(AppContext.BaseDirectory,"Assets","icon.ico");if(File.Exists(iconPath))Icon=new Icon(iconPath);
  pages=new(){["Dashboard"]=()=>new DashboardControl(this),["Age Calculator"]=()=>new AgeControl(),["Salaried Tax"]=()=>new SalariedTaxControl(),["Business Tax"]=()=>new BusinessTaxControl(),["Loan EMI"]=()=>new LoanControl(),["Code 39"]=()=>new Code39Control(),["PDF417"]=()=>new Pdf417Control(),["PDF to Image"]=()=>new PdfToImageControl()};
  BuildShell(); Navigate("Dashboard"); }
  void BuildShell(){var side=new Panel{Dock=DockStyle.Left,Width=245,BackColor=Ui.Navy};var logoPath=Path.Combine(AppContext.BaseDirectory,"Assets","logo.png");if(File.Exists(logoPath)){var logo=new PictureBox{Image=Image.FromFile(logoPath),SizeMode=PictureBoxSizeMode.Zoom,Dock=DockStyle.Top,Height=95,BackColor=Ui.Navy};side.Controls.Add(logo);}else{var brand=new Label{Text="CALCULATOR\nSUITE",ForeColor=Color.White,Font=new Font("Segoe UI",18,FontStyle.Bold),Dock=DockStyle.Top,Height=95,Padding=new Padding(25,22,0,0)};side.Controls.Add(brand);}var home=CreateMenuButton("Dashboard");home.Location=new Point(15,95);home.Width=215;home.Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right;side.Controls.Add(home);var menu=new FlowLayoutPanel{Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,WrapContents=false,Padding=new Padding(15,55,15,10),BackColor=Ui.Navy};side.Controls.Add(menu);home.BringToFront();foreach(var k in pages.Keys)if(k!="Dashboard")AddMenuButton(menu,k);side.Resize+=(_,_)=>{home.Width=Math.Max(150,side.ClientSize.Width-30);foreach(Button b in menu.Controls)b.Width=Math.Max(150,menu.ClientSize.Width-menu.Padding.Left-menu.Padding.Right);};
   var main=new Panel{Dock=DockStyle.Fill,BackColor=Ui.Bg};var top=new Panel{Dock=DockStyle.Top,Height=75,BackColor=Color.White,Padding=new Padding(28,18,28,10)};pageTitle.Text="Dashboard";pageTitle.Font=Ui.Title;pageTitle.ForeColor=Ui.Text;pageTitle.AutoSize=true;top.Controls.Add(pageTitle);exportButton=Ui.Button("Export PDF");exportButton.Dock=DockStyle.Right;exportButton.Width=125;exportButton.Height=38;exportButton.Click+=(_,_)=>ExportPdf();top.Controls.Add(exportButton);previewButton=Ui.Button("Preview");previewButton.Dock=DockStyle.Right;previewButton.Width=105;previewButton.Height=38;previewButton.Click+=(_,_)=>PreviewCurrentScreen();top.Controls.Add(previewButton);printButton=Ui.Button("Print");printButton.Dock=DockStyle.Right;printButton.Width=95;printButton.Height=38;printButton.Click+=(_,_)=>PrintWithPreview();top.Controls.Add(printButton);main.Controls.Add(top);content.Dock=DockStyle.Fill;content.AutoScroll=true;content.Padding=new Padding(25);main.Controls.Add(content);Controls.Add(main);Controls.Add(side);}
  Button CreateMenuButton(string key){var button=new Button{Text=key,Width=210,Height=44,FlatStyle=FlatStyle.Flat,BackColor=Ui.Navy,ForeColor=Color.Gainsboro,TextAlign=ContentAlignment.MiddleLeft,Font=new Font("Segoe UI",10),FlatAppearance={BorderSize=0}};button.Click+=(_,_)=>Navigate(key);return button;}
  void AddMenuButton(FlowLayoutPanel menu,string key){menu.Controls.Add(CreateMenuButton(key));}
  void Navigate(string key){pageTitle.Text=key;var showPrintTools=key!="Dashboard";exportButton.Visible=showPrintTools;previewButton.Visible=showPrintTools;printButton.Visible=showPrintTools;content.Controls.Clear();var c=pages[key]();c.Dock=DockStyle.Fill;content.Controls.Add(c);}
 public void Go(string key)=>Navigate(key);
  Bitmap CaptureCurrentScreen(){var bitmap=new Bitmap(content.ClientSize.Width,content.ClientSize.Height);content.DrawToBitmap(bitmap,content.ClientRectangle);return bitmap;}
  PrintDocument CreatePrintDocument(Bitmap bitmap)
  {
   var document=new PrintDocument{DocumentName=$"Calculator Suite - {pageTitle.Text}"};document.DefaultPageSettings.PaperSize=new PaperSize("A4",827,1169);document.DefaultPageSettings.Landscape=false;document.DefaultPageSettings.Margins=new Margins(25,25,25,25);
   document.PrintPage+=(sender,e)=>{var bounds=e.MarginBounds;var scale=Math.Min((float)bounds.Width/bitmap.Width,(float)bounds.Height/bitmap.Height);var width=(int)(bitmap.Width*scale);var height=(int)(bitmap.Height*scale);var x=bounds.Left;var y=bounds.Top;e.Graphics.DrawImage(bitmap,new Rectangle(x,y,width,height));e.HasMorePages=false;};
   return document;
  }
  void PrintCurrentScreen()
  {
   if(content.ClientSize.Width<=0||content.ClientSize.Height<=0)return;
   using var bitmap=CaptureCurrentScreen();using var document=CreatePrintDocument(bitmap);using var dialog=new PrintDialog{Document=document,UseEXDialog=true};if(dialog.ShowDialog(this)==DialogResult.OK)document.Print();
  }
  void PreviewCurrentScreen()
  {
   if(content.ClientSize.Width<=0||content.ClientSize.Height<=0)return;
   using var bitmap=CaptureCurrentScreen();using var document=CreatePrintDocument(bitmap);using var preview=new PrintPreviewDialog{Document=document,WindowState=FormWindowState.Maximized,UseAntiAlias=true};preview.ShowDialog(this);
  }
  void PrintWithPreview()
  {
   if(content.ClientSize.Width<=0||content.ClientSize.Height<=0)return;
   using var bitmap=CaptureCurrentScreen();using var document=CreatePrintDocument(bitmap);using var preview=new PrintPreviewDialog{Document=document,WindowState=FormWindowState.Maximized,UseAntiAlias=true};preview.ShowDialog(this);
   using var dialog=new PrintDialog{Document=document,UseEXDialog=true};if(dialog.ShowDialog(this)==DialogResult.OK)document.Print();
  }
  void ExportPdf()
  {
   if(content.ClientSize.Width<=0||content.ClientSize.Height<=0)return;
   var printer=PrinterSettings.InstalledPrinters.Cast<string>().FirstOrDefault(x=>x.Equals("Microsoft Print to PDF",StringComparison.OrdinalIgnoreCase));
   if(printer==null){MessageBox.Show("Microsoft Print to PDF is not installed on this computer.","Export PDF",MessageBoxButtons.OK,MessageBoxIcon.Warning);return;}
   using var dialog=new SaveFileDialog{Filter="PDF document|*.pdf",FileName=$"{pageTitle.Text}.pdf",Title="Export calculator screen to PDF"};if(dialog.ShowDialog(this)!=DialogResult.OK)return;
   using var bitmap=CaptureCurrentScreen();using var document=CreatePrintDocument(bitmap);document.PrinterSettings.PrinterName=printer;document.PrinterSettings.PrintToFile=true;document.PrinterSettings.PrintFileName=dialog.FileName;document.PrintController=new StandardPrintController();document.Print();MessageBox.Show("PDF exported successfully.","Export PDF",MessageBoxButtons.OK,MessageBoxIcon.Information);
  }
}
