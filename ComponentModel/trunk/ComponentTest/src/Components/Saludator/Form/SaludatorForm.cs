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
		private Button button1;
                private Button button2;
		private bool changeButton;
                
                public SaludatorForm() : base ()
		{
		    changeButton = true;	
                    InitializeComponent();
			
		}
		
		private void InitializeComponent() {
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new Button ();
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
			
                        this.button2.Location = new Point (200, 96);
                        this.button2.Size = new Size (88, 40);
                        this.button1.TabIndex = 0;
                        this.button2.Click += new EventHandler (this.Button2Click);
                        this.button2.Text = "Hello from container";
                        // 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.button1);
                        this.Controls.Add (this.button2);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);
                }
                
                void Virtual (ResponseMethodVO responseMethodVO) {
                    Console.WriteLine ("Que pasa nenggg, soy el fucking virtual !!");
                }
                
                void Virtual2 (ResponseMethodVO responseMethodVO) {
                    Console.WriteLine ("Vritual 2 respondiendo");
                }
                
                void Button2Click (object sender, EventArgs evt) {
                    DefaultContainer.Instance.GetComponentByName ("Saludator").VirtualMethod += new VirtualMethod (this.Virtual);
                    DefaultContainer.Instance.GetComponentByName ("Saludator").VirtualMethod += new VirtualMethod (this.Virtual2);
                    DefaultContainer.Instance.Execute ("Saludator", "Saluda", null, this);
                    
                }
                
		void Button1Click(object sender, System.EventArgs ev) {
                    DefaultComponentModel dcm = (DefaultComponentModel) DefaultContainer.Instance.GetComponentByName ("Saludator"); 
                    //dcm.Execute ("Saluda", null);
                    dcm.Execute ("Saluda", null, this);
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
