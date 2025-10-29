using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient;

public partial class MainForm : Form
{
	private TcpClient? tcpClient;
	private StreamReader? reader;
	private StreamWriter? writer;
	private Task? receiveLoopTask;
	private CancellationTokenSource? receiveCts;

	public MainForm()
	{
		InitializeComponent();
	}

	private async void btnConnect_Click(object sender, EventArgs e)
	{
		if (tcpClient is null)
		{
			await ConnectAsync();
		}
		else
		{
			Disconnect();
		}
	}

	private async Task ConnectAsync()
	{
		var host = txtHost.Text.Trim();
		if (!int.TryParse(txtPort.Text.Trim(), out int port)) { MessageBox.Show("Port hatalı"); return; }
		var username = txtUser.Text.Trim();
		if (string.IsNullOrWhiteSpace(username)) { MessageBox.Show("Kullanıcı adı boş olamaz"); return; }

		btnConnect.Enabled = false;
		try
		{
			tcpClient = new TcpClient();
			await tcpClient.ConnectAsync(host, port);
			var ns = tcpClient.GetStream();
			reader = new StreamReader(ns, Encoding.UTF8, false, 1024, leaveOpen: true);
			writer = new StreamWriter(ns, new UTF8Encoding(false)) { AutoFlush = true };

			await writer.WriteLineAsync($"USER {username}");
			var resp = await reader.ReadLineAsync();
			if (!string.Equals(resp, "OK", StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception(resp ?? "Sunucudan yanıt yok");
			}

			receiveCts = new CancellationTokenSource();
			receiveLoopTask = Task.Run(() => ReceiveLoop(receiveCts.Token));
			btnConnect.Text = "Kes";
			AppendChat($"[Bağlandı] {host}:{port} olarak {username}\r\n");
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Bağlantı hatası: {ex.Message}");
			Disconnect();
		}
		finally
		{
			btnConnect.Enabled = true;
		}
	}

	private void Disconnect()
	{
		try { receiveCts?.Cancel(); } catch { }
		try { tcpClient?.Close(); } catch { }
		tcpClient = null;
		reader = null;
		writer = null;
		btnConnect.Text = "Bağlan";
		AppendChat("[Bağlantı kesildi]\r\n");
	}

	private async Task ReceiveLoop(CancellationToken token)
	{
		try
		{
			while (!token.IsCancellationRequested && reader != null)
			{
				var line = await reader.ReadLineAsync();
				if (line is null) break;
				HandleServerLine(line);
			}
		}
		catch { }
		finally
		{
			if (!token.IsCancellationRequested)
			{
				this.Invoke(new Action(Disconnect));
			}
		}
	}

	private void HandleServerLine(string line)
	{
		if (line.StartsWith("USERJOIN "))
		{
			AppendChat($"* {line.Substring(9)} katıldı\r\n");
		}
		else if (line.StartsWith("USERLEFT "))
		{
			AppendChat($"* {line.Substring(9)} ayrıldı\r\n");
		}
		else if (line.StartsWith("MESSAGE "))
		{
			var rest = line.Substring(8);
			var idx = rest.IndexOf(' ');
			if (idx > 0)
			{
				var user = rest.Substring(0, idx);
				var msg = rest.Substring(idx + 1);
				AppendChat($"{user}: {msg}\r\n");
			}
		}
	}

	private void AppendChat(string text)
	{
		if (txtChat.InvokeRequired)
		{
			txtChat.Invoke(new Action<string>(AppendChat), text);
			return;
		}
		txtChat.AppendText(text);
	}

	private async void btnSend_Click(object sender, EventArgs e)
	{
		var msg = txtInput.Text;
		if (string.IsNullOrWhiteSpace(msg) || writer == null) return;
		try
		{
			await writer.WriteLineAsync($"MSG {msg}");
			txtInput.Clear();
		}
		catch
		{
			Disconnect();
		}
	}
}
