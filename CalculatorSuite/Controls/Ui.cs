using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
namespace CalculatorSuite.Controls;
public static class Ui
{
 public static Color Navy=Color.FromArgb(15,23,42), Blue=Color.FromArgb(2,132,199), Bg=Color.FromArgb(248,250,252), Text=Color.FromArgb(30,41,59), Muted=Color.FromArgb(100,116,139), Border=Color.FromArgb(226,232,240);
 public static Font Title=new("Segoe UI",20,FontStyle.Bold), H2=new("Segoe UI",14,FontStyle.Bold), Body=new("Segoe UI",10.5f), Small=new("Segoe UI",9.5f);
 public static Panel Card(string title){var p=new Panel{BackColor=Color.White,Padding=new Padding(22),Dock=DockStyle.Fill};p.Paint+=(s,e)=>{using var pen=new Pen(Border);e.Graphics.DrawRectangle(pen,0,0,p.Width-1,p.Height-1);};var l=new Label{Text=title,Font=H2,ForeColor=Text,AutoSize=true,Location=new Point(22,18)};p.Controls.Add(l);return p;}
 public static Button Button(string text,Color? c=null){var b=new Button{Text=text,AutoSize=false,Height=42,Width=170,FlatStyle=FlatStyle.Flat,BackColor=c??Blue,ForeColor=Color.White,Font=new Font("Segoe UI",10,FontStyle.Bold),Cursor=Cursors.Hand};b.FlatAppearance.BorderSize=0;return b;}
 public static Label Label(string t){return new Label{Text=t,AutoSize=true,Font=Body,ForeColor=Text,Margin=new Padding(0,8,0,4)};}
 public static TextBox Input(){return new TextBox{Height=32,Font=Body,BorderStyle=BorderStyle.FixedSingle};}
 public static ComboBox Combo(params string[] items){var c=new ComboBox{DropDownStyle=ComboBoxStyle.DropDownList,Font=Body,Height=32};c.Items.AddRange(items);if(c.Items.Count>0)c.SelectedIndex=0;return c;}
 public static TableLayoutPanel Grid(int cols=2){return new TableLayoutPanel{Dock=DockStyle.Top,AutoSize=true,ColumnCount=cols,Padding=new Padding(0,45,0,0),ColumnStyles={}};}
}
