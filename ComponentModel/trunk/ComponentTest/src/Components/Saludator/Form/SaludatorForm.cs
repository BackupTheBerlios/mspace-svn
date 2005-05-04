using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentModel.Interfaces;
using ComponentModel.Container;
using ComponentModel.VO;

namespace ComponentModel.ComponentTest.Components.Saludator.Form
{
	public class SaludatorForm : System.Windows.Forms.Form, IViewHandler
	{
		private System.Windows.Forms.Button button1;
		private bool changeButton;
                
                public SaludatorForm() : base ()
		{
		    changeButton = true;	
                    InitializeComponent();
			
		}
		
		private void InitializeComponent() {
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(56, 96);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 40);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);
                }

		void Button1Click(object sender, System.EventArgs ev) {
                    DefaultComponentModel dcm = (DefaultComponentModel) DefaultContainer.Instance.GetComponentByName ("Saludator"); 
                    //dcm.Execute ("Saluda", null);
                    dcm.Execute ("Saluda", false, false, null, this);
		}
		
                public void ResponseInitApp (ResponseMethodVO response) {
                    Console.WriteLine ("Response Initapp executed.");
                    button1.Text = "Co co";
                    this.ShowDialog ();
                }
        
            public void ResponseSaluda (ResponseMethodVO response) {
                Console.WriteLine (response.ExecutionSuccess);
                if (response.ExecutionSuccess)  {
                    if (!changeButton) {
                        button1.Text = "Cambio de pareja";
                        changeButton = true;
                    }
                    else {
                        button1.Text = "Cambio de nuevo";
                        changeButton = false;
                    }
                    Console.WriteLine ("//------------------//");
                    Console.WriteLine ("Hola luis.");
                    Console.WriteLine ("Response hola !!");
                    Console.WriteLine ((int)response.MethodResult);
                    Console.WriteLine ("//------------------//");
                }
            } 

        
            public void ResponseSaludaATodos (ResponseMethodVO response) {
                Console.WriteLine ("Aquí ejecutaré el response del caso de uso.\nBellaco lo será tu calavera en almibar.");
            }
        }
}
