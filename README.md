# MakizunoUI.MakizunoSpellChecker
<br>"巻き角スペルチェッカー" <br>
SpellChecker for WPF TextBox and ComboBox<br>
This library performs word-by-word spell checking based on a user dictionary loaded from a JSON file.
<br>
このライブラリは、Jsonファイルから読み込んだユーザー辞書を元に、単語単位でスペルチェックを行います。
<br>
email:shin.citysheep@gmail.com
<br>
twitter: https://x.com/sheephuman
<br>

![image](https://github.com/user-attachments/assets/481e436a-cfdc-49e7-9194-89cafceea002)


# How to use it
```xml

    <Window x:Class="FFmpegValidator.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"            
            xmlns:local="clr-namespace:MakizunoUI.Controls" ← use this Library
            ResizeMode="CanResize"
            mc:Ignorable="d"
            Loaded="Window_Loaded"
            Title="MainWindow" Height="280" Width="450">
    <Grid VerticalAlignment="Top">
        <StackPanel Orientation="Vertical">
             <!--use Local:-->
            <local:IdleComboBox x:Name="SpellValidatorComboBox" Width="400" Height="35" FontSize="16" IsEditable="True"            
             SelectionChanged="SpellValidatorComboBox_SelectionChanged"
             Loaded="SpellValidorCombboBox_loaded"/>
             <!--use Local:-->
            <local:IdleTextBox x:Name="SpellValidorBox" Width="400" Height="160"
        FontSize="22" Loaded="Window_Loaded"
        TextWrapping="Wrap"
        AcceptsReturn="True"
        TextChanged="SpellValidorBox_TextChanged"
        />
        </StackPanel>
    </Grid>
</Window>
```

# CodeBehind
```cs

     TextBoxStylingHelper _draw:
      TextBox? childTextbox;

    public MainWindow()
    {
        
        InitializeComponent();

        SpellValidatorComboBox.InputIdle += ComboBox_inputIdle;

       SpellValidorBox.InputIdle += SpellValidorBox_InputIdle;


        SpellValidorBox.SelectionChanged += SpellValidorBox_CaletMove;

        _draw = new TextBoxStylingHelper();

    }

     private void SpellValidorCombboBox_loaded(object sender, RoutedEventArgs e)
 {
     var comboBox = sender as ComboBox;
     if (comboBox == null) return;


     childTextbox = ChildFinder.FindVisualChild<TextBox>(comboBox);
     if (childTextbox is not null)
     {
         childTextbox.GotKeyboardFocus += (s, e) =>
         {
             //  ComboBoxの子TextBoxがフォーカスを得たときの処理
             // proceduer When get childTextBox forcused in ComboBox
             if (childTextbox is not null)
             {
                 childTextbox.SelectionLength = 0; // 全選択を解除
                 childTextbox.SelectionStart = childTextbox.Text.Length; //locate Cursol to end
             }
         };
         childTextbox.Focus(); // ComboBoxの子TextBoxにフォーカスを設定
     }


      private void SpellValidorBox_InputIdle(object? sender, EventArgs e)
  {
      try
      {
          if (sender is not null)
          {

              inputIdleProcedure((TextBox)sender, 2);
          }

      }
      catch (Exception ex)
      {
          // Handle exceptions here
          MessageBox.Show($"Error: {ex.Message}");
      }
    

          void inputIdleProcedure(TextBox textBox, int rectMaegin)
      {             // procedure When input state is Idle 

          try
          {
              if (textBox.IsFocused)
              {


                  Dispatcher.InvokeAsync(() =>
                  {
                      _draw.DrawSinWave(textBox, "rules.json", rectMaegin);    // UI スレッドで実行することを保証
                      
                  });
                  // Handle the InputIdle event here
              }

          }
          catch (Exception ex)
          {
              // Handle exceptions here
              MessageBox.Show($"Error: {ex.Message}");
          }


              private void ComboBox_inputIdle(object? sender, EventArgs e)
    {
        try
        {

            ComboBox? comboBox = sender as ComboBox;


            if (childTextbox is null)
            {
                return;
            }
            else
            {
                if (childTextbox.IsFocused)
                    inputIdleProcedure(childTextbox, 4);
            }

        }
        catch (Exception ex)
        {
            // Handle exceptions here
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
}
  
  
  
  
```


# Json rule Exsample

Option1
```json:rule.json
{
  "input": "inputfile.mp4",
  "output": "output.mp4",
  "options": [
    "-codec:v",
    "libx265",
    "-vf",
    "yadif=0:-1:1",
    "-pix_fmt",
    "yuv420p",
    "-acodec",
    "aac",
    "-threads",
    "2"
  ]
}

```
Option2
```json:rules.json
{
  "profile1": {
    "input": "inputfile.mp4",
    "output": "output.mp4",
    "options": [
      "-codec:v",
      "libx265",
      "-vf",
      "yadif=0:-1:1",
      "-pix_fmt",
      "yuv420p",
      "-acodec",
      "aac",
      "-threads",
      "2",
      "-r 60"
    ]
  },
  "profile2": {
    "input": "inputfile.mp4",
    "output": "output.gif",
    "options": [
      "-filter_complex",
      "[0:v] fps=10,scale=320:-1:flags=lanczos,palettegen=stats_mode=diff [p];[0:v][p] paletteuse"
    ]
  }
}


```

