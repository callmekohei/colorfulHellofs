open System
open System.Text.Json
open System.IO
open CmdConsole.ColorScheme

type CmdColorPalette = {
  Scheme0 : ColorScheme
  Scheme1 : ColorScheme
  Scheme2 : ColorScheme
}

let stream = new FileStream("./colorScheme.json", FileMode.Open, FileAccess.Read)
let cmdColorPalette = JsonSerializer.Deserialize<CmdColorPalette>(stream)

match Random().Next(3)  with
| 0 ->
  // cmd console default scheme
  // see also : https://devblogs.microsoft.com/commandline/updating-the-windows-console-colors/
  printfn "----- pattern0 -----"
  cmdColorPalette.Scheme0 |> updateColorPalette
| 1 ->
  // color scheme like visual studio code color
  //
  printfn "----- pattern1 -----"
  cmdColorPalette.Scheme1 |> updateColorPalette
| _ ->
  // color scheme like tokyo night
  // see also : https://github.com/enkia/tokyo-night-vscode-theme
  printfn "----- pattern2 -----"
  cmdColorPalette.Scheme2 |> updateColorPalette

// devhawk/colorconsole.fs(https://gist.github.com/devhawk/4719d1b369170b206cd88b9da16e1b8a)
// helper function to set the console collor and automatically set it back when disposed
let consoleColor (fc : ConsoleColor) =
    let current = Console.ForegroundColor
    Console.ForegroundColor <- fc
    { new IDisposable with
          member x.Dispose() = Console.ForegroundColor <- current }

let cprintfn (color:ConsoleColor) (str:string) =
  Printf.kprintf
    (fun s -> use _ = consoleColor color in printfn "%s" s)
    ( Printf.StringFormat<'a,unit>(str) )

"hello world!" |> cprintfn ConsoleColor.Black
"hello world!" |> cprintfn ConsoleColor.DarkBlue
"hello world!" |> cprintfn ConsoleColor.DarkGreen
"hello world!" |> cprintfn ConsoleColor.DarkCyan
"hello world!" |> cprintfn ConsoleColor.DarkRed
"hello world!" |> cprintfn ConsoleColor.DarkMagenta
"hello world!" |> cprintfn ConsoleColor.DarkYellow
"hello world!" |> cprintfn ConsoleColor.Gray
"------------" |> printfn "%s"
"hello world!" |> cprintfn ConsoleColor.DarkGray
"hello world!" |> cprintfn ConsoleColor.Blue
"hello world!" |> cprintfn ConsoleColor.Green
"hello world!" |> cprintfn ConsoleColor.Cyan
"hello world!" |> cprintfn ConsoleColor.Red
"hello world!" |> cprintfn ConsoleColor.Magenta
"hello world!" |> cprintfn ConsoleColor.Yellow
"hello world!" |> cprintfn ConsoleColor.White

"""
  　　　おおおおおお
  　　おおおおおおおおお
  　　おおおおおおお
  　おおおおおおおおおお
  　おおおおおおおおおおお
  　おおおおおおおおおお
  　　　おおおおおおお
  　　おおおおおお
  　おおおおおおおおおお
  おおおおおおおおおおおお
  おおおおおおおおおおおお
  おおおおおおおおおおおお
  おおおおおおおおおおおお
  　　おおお　　おおお
  　おおお　　　　おおお
  おおおお　　　　おおおお
    """ |> cprintfn ConsoleColor.Green