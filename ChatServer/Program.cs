using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;

namespace ChatServer;

internal class Program
{
	private static readonly ConcurrentDictionary<TcpClient, string> clientToUsername = new();
	private static readonly object broadcastLock = new();

	private static async Task Main(string[] args)
	{
		int port = ParsePort(args, defaultPort: 5000);
		var listener = new TcpListener(IPAddress.Any, port);
		listener.Start();
		Console.WriteLine($"[SERVER] Dinleniyor: 0.0.0.0:{port}");

		while (true)
		{
			TcpClient client;
			try
			{
				client = await listener.AcceptTcpClientAsync();
			}
			catch (ObjectDisposedException)
			{
				break;
			}

			_ = HandleClientAsync(client);
		}
	}

	private static int ParsePort(string[] args, int defaultPort)
	{
		if (args.Length > 0 && int.TryParse(args[0], out var p) && p > 0 && p < 65536)
		{
			return p;
		}
		return defaultPort;
	}

	private static async Task HandleClientAsync(TcpClient client)
	{
		var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
		Console.WriteLine($"[CONNECT] {endpoint}");
		using var networkStream = client.GetStream();
		using var reader = new StreamReader(networkStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
		using var writer = new StreamWriter(networkStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)) { AutoFlush = true };

		string? username = null;
		try
		{
			// İlk satır USER <name> beklenir
			var helloLine = await reader.ReadLineAsync();
			if (helloLine is null || !helloLine.StartsWith("USER "))
			{
				await writer.WriteLineAsync("ERROR Beklenen: USER <kullanici_adi>");
				client.Close();
				return;
			}
			username = helloLine.Substring(5).Trim();
			if (string.IsNullOrWhiteSpace(username))
			{
				await writer.WriteLineAsync("ERROR Kullanici adi bos olamaz");
				client.Close();
				return;
			}

			clientToUsername[client] = username;
			// Önce katılan istemciye OK gönder, sonra diğerlerine USERJOIN yayınla
			await writer.WriteLineAsync("OK");
			Broadcast($"USERJOIN {username}", except: client);
			Console.WriteLine($"[USER] {endpoint} -> {username}");

			// Mesaj döngüsü
			while (true)
			{
				var line = await reader.ReadLineAsync();
				if (line is null) break; // bağlantı kapandı
				if (line.StartsWith("MSG "))
				{
					var message = line.Substring(4);
					Broadcast($"MESSAGE {username} {message}", except: null);
				}
				else if (line.Equals("PING", StringComparison.OrdinalIgnoreCase))
				{
					await writer.WriteLineAsync("PONG");
				}
				else
				{
					await writer.WriteLineAsync("ERROR Bilinmeyen komut");
				}
			}
		}
		catch (IOException)
		{
			// istemci beklenmedik ayrıldı
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[ERROR] {endpoint}: {ex.Message}");
		}
		finally
		{
			if (username != null)
			{
				Broadcast($"USERLEFT {username}", except: client);
			}
			clientToUsername.TryRemove(client, out _);
			client.Close();
			Console.WriteLine($"[DISCONNECT] {endpoint}");
		}
	}

	private static void Broadcast(string line, TcpClient? except)
	{
		var bytes = Encoding.UTF8.GetBytes(line + "\n");
		lock (broadcastLock)
		{
			foreach (var kvp in clientToUsername.Keys.ToList())
			{
				var c = kvp;
				if (except != null && ReferenceEquals(c, except)) continue;
				try
				{
					var stream = c.GetStream();
					stream.Write(bytes, 0, bytes.Length);
				}
				catch
				{
					// gönderilemeyenleri kapat
					try { c.Close(); } catch { }
					clientToUsername.TryRemove(c, out _);
				}
			}
		}
	}
}
