﻿namespace Sample

module Main =

  open System.IO
  open System.Text.Json
  open DllImported
  open CmdConsole

  type JsonParams = {
    ColorScheme         : ColorScheme
    FontParams          : FontParams
    ColorParams         : ColorParams
    ConsoleEncodingUTF8 : bool
  }

  [<EntryPoint>]
  let main args =

    let stream = new FileStream("./cmdconsolesettings.json", FileMode.Open, FileAccess.Read)
    let cmdParams = JsonSerializer.Deserialize<JsonParams>(stream)
    cmdParams.ColorScheme         |> updateColorScheme
    cmdParams.FontParams          |> updateFont
    cmdParams.ColorParams         |> consoleColor
    cmdParams.ConsoleEncodingUTF8 |> consoleEncodingUTF8

    Greeting.hello ()

    0