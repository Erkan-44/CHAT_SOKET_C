namespace ChatClient;

partial class MainForm
{
	/// <summary>
	///  Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	///  Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	///  Required method for Designer support - do not modify
	///  the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.txtHost = new System.Windows.Forms.TextBox();
		this.txtPort = new System.Windows.Forms.TextBox();
		this.txtUser = new System.Windows.Forms.TextBox();
		this.btnConnect = new System.Windows.Forms.Button();
		this.txtChat = new System.Windows.Forms.TextBox();
		this.txtInput = new System.Windows.Forms.TextBox();
		this.btnSend = new System.Windows.Forms.Button();
		this.SuspendLayout();
		// 
		// txtHost
		// 
		this.txtHost.Location = new System.Drawing.Point(12, 12);
		this.txtHost.Name = "txtHost";
		this.txtHost.PlaceholderText = "Sunucu (ör. 127.0.0.1)";
		this.txtHost.Size = new System.Drawing.Size(160, 23);
		this.txtHost.TabIndex = 0;
		this.txtHost.Text = "127.0.0.1";
		// 
		// txtPort
		// 
		this.txtPort.Location = new System.Drawing.Point(178, 12);
		this.txtPort.Name = "txtPort";
		this.txtPort.PlaceholderText = "Port";
		this.txtPort.Size = new System.Drawing.Size(60, 23);
		this.txtPort.TabIndex = 1;
		this.txtPort.Text = "5000";
		// 
		// txtUser
		// 
		this.txtUser.Location = new System.Drawing.Point(244, 12);
		this.txtUser.Name = "txtUser";
		this.txtUser.PlaceholderText = "Kullanıcı adı";
		this.txtUser.Size = new System.Drawing.Size(140, 23);
		this.txtUser.TabIndex = 2;
		// 
		// btnConnect
		// 
		this.btnConnect.Location = new System.Drawing.Point(390, 12);
		this.btnConnect.Name = "btnConnect";
		this.btnConnect.Size = new System.Drawing.Size(75, 23);
		this.btnConnect.TabIndex = 3;
		this.btnConnect.Text = "Bağlan";
		this.btnConnect.UseVisualStyleBackColor = true;
		this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
		// 
		// txtChat
		// 
		this.txtChat.Location = new System.Drawing.Point(12, 50);
		this.txtChat.Multiline = true;
		this.txtChat.Name = "txtChat";
		this.txtChat.ReadOnly = true;
		this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.txtChat.Size = new System.Drawing.Size(534, 300);
		this.txtChat.TabIndex = 4;
		// 
		// txtInput
		// 
		this.txtInput.Location = new System.Drawing.Point(12, 360);
		this.txtInput.Name = "txtInput";
		this.txtInput.Size = new System.Drawing.Size(453, 23);
		this.txtInput.TabIndex = 5;
		// 
		// btnSend
		// 
		this.btnSend.Location = new System.Drawing.Point(471, 360);
		this.btnSend.Name = "btnSend";
		this.btnSend.Size = new System.Drawing.Size(75, 23);
		this.btnSend.TabIndex = 6;
		this.btnSend.Text = "Gönder";
		this.btnSend.UseVisualStyleBackColor = true;
		this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
		// 
		// MainForm
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(558, 395);
		this.Controls.Add(this.btnSend);
		this.Controls.Add(this.txtInput);
		this.Controls.Add(this.txtChat);
		this.Controls.Add(this.btnConnect);
		this.Controls.Add(this.txtUser);
		this.Controls.Add(this.txtPort);
		this.Controls.Add(this.txtHost);
		this.Name = "MainForm";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Basit Sohbet İstemcisi";
		this.ResumeLayout(false);
		this.PerformLayout();

	}

	#endregion

	private TextBox txtHost;
	private TextBox txtPort;
	private TextBox txtUser;
	private Button btnConnect;
	private TextBox txtChat;
	private TextBox txtInput;
	private Button btnSend;
}
