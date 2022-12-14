namespace Sample

module Main =

  open System.IO
  open System.Text.Json
  open MyConsole.CmdConsole
  open MyConsole.CmdConsole.Util

  type JsonParams = {
    ColorTable             : ColorTable
    FontParams             : FontParams
    ColorParams            : ColorParams
    ConsoleEncodingUnicode : bool
  }

  [<EntryPoint>]
  let main args =

    let stream = new FileStream("./cmdconsolesettings.json", FileMode.Open, FileAccess.Read)
    let cmdParams = JsonSerializer.Deserialize<JsonParams>(stream)
    cmdParams.ColorTable             |> updateColorScheme
    cmdParams.FontParams             |> updateFontInfo
    cmdParams.ColorParams            |> consoleColor
    cmdParams.ConsoleEncodingUnicode |> consoleEncodingUTF8

    Greeting.hello ()

    0