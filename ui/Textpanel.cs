using Godot;
using System;
using System.Text;
using System.Threading.Tasks;

public partial class Textpanel : TextureRect{


	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("esc"))
		{
			QueueFree();
		}
	}

	public string[] texts =
		{
			"UGh", "Where am i?", "Who are you?", "Hello Zhukov",
			"I am dEATH", "You want to live?", "Then listen carefully.", "You should get 200souls",
			"Complete it, and you may work your way back to life.",
			"But, if you fail... Know this — each day, your mind will not be at peace.",
			"Memories will rot. Reality will twist. "
		},
		whosaid =
		{
			"Zhukov","Zhukov","Zhukov","Death","Death","Death","Death","Death","Death","Death","Death"
		};
	[Export] public Label TextLabel;
	[Export] public Label WhosaidLabel;
	[Export] public float TextSpeed = 0.05f; 



	private bool isWriting = false;
	private bool skipRequested = false;
	private int currentLineIndex = 0;

	public override void _Ready()
	{
		// Начинаем с первой строки
		ShowNextLine();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			if (isWriting)
				skipRequested = true;
			else
			{
				try
				{
					WhosaidLabel.Text = whosaid[currentLineIndex];
				}catch(IndexOutOfRangeException){}
				ShowNextLine();
			}
		}
	}

	private async void ShowNextLine()
	{
		if (currentLineIndex >= texts.Length)
		{
			
			if (GetParent() is Zhukov tl) tl.dialogue = false;
			QueueFree();
			return;
		}

		await TypeText(texts[currentLineIndex]);
		currentLineIndex++;
	}

	private async Task TypeText(string text)
	{
		isWriting = true;
		skipRequested = false;
		TextLabel.Text = "";
		var sb = new StringBuilder();

		foreach (char c in text)
		{
			if (skipRequested)
			{
				TextLabel.Text = text;
				break;
			}

			sb.Append(c);
			TextLabel.Text = sb.ToString();
			await ToSignal(GetTree().CreateTimer(TextSpeed), "timeout");
		}

		isWriting = false;
	}
}
