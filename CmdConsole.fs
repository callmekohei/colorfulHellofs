namespace Sample

module MyConsole =

  module CmdConsole =

    open System
    open System.Runtime.InteropServices

    [<Struct; StructLayout(LayoutKind.Sequential, Size = 4)>]
    type RGB = {
      Red   : byte
      Green : byte
      Blue  : byte
    }

    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type ColorScheme = {
      black       : RGB
      darkBlue    : RGB
      darkGreen   : RGB
      darkCyan    : RGB
      darkRed     : RGB
      darkMagenta : RGB
      darkYellow  : RGB
      gray        : RGB
      darkGray    : RGB
      blue        : RGB
      green       : RGB
      cyan        : RGB
      red         : RGB
      magenta     : RGB
      yellow      : RGB
      white       : RGB
    }

    module private DllImported =

      [<Struct; StructLayout(LayoutKind.Sequential)>]
      type COORD = { X : int16; Y : int16 }

      [<Struct; StructLayoutAttribute(LayoutKind.Sequential)>]
      type SMALL_RECT = { Left : int16; Top : int16; Right : int16; Bottom : int16 }

      [<Struct; StructLayoutAttribute(LayoutKind.Sequential)>]
      type CONSOLE_SCREEN_BUFFER_INFOEX =
        val mutable cbSize               : int
        val mutable dwSize               : COORD
        val mutable dwCursorPosition     : COORD
        val mutable wAttributes          : int16
        val mutable srWindow             : SMALL_RECT
        val mutable dwMaximumWindowSize  : COORD
        val mutable wPopupAttributes     : int16
        val mutable bFullscreenSupported : bool
        val mutable colorScheme          : ColorScheme

      [<Struct>]
      [<StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)>]
      type CONSOLE_FONT_INFO_EX =
          val mutable cbSize: uint32
          val mutable nFont: uint32
          val mutable dwFontSize: COORD
          val mutable FontFamily: int
          val mutable FontWeight: int
          [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
          val mutable FaceName: string

      [<DllImportAttribute("kernel32.dll",SetLastError=true)>]
      extern IntPtr GetStdHandle(int nStdHndle)

      [<DllImportAttribute("kernel32.dll",SetLastError=true)>]
      extern IntPtr GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, CONSOLE_SCREEN_BUFFER_INFOEX& ConsoleScreenBufferInfo)

      [<DllImportAttribute("kernel32.dll",SetLastError=true)>]
      extern IntPtr SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, CONSOLE_SCREEN_BUFFER_INFOEX& ConsoleScreenBufferInfo)

      [<DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
      extern bool GetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx)

      [<DllImport("kernel32.dll", SetLastError = true)>]
      extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, CONSOLE_FONT_INFO_EX consoleCurrentFontEx)


    module Util =

      open DllImported

      [<Literal>]
      let STD_OUTPUT_HANDLE = -11

      [<Literal>]
      let TMPF_TRUETYPE = 4

      [<Literal>]
      let LF_FACESIZE = 32

      type FontParams = {
        Name : string
        Size : int16
      }

      type ColorParams = {
        backGroundColor : int
        foreGroundColor : int
      }

      let updateColorScheme (colorScheme:ColorScheme) =
        let mutable screenBuffer = new CONSOLE_SCREEN_BUFFER_INFOEX(
            cbSize = Marshal.SizeOf(typeof<CONSOLE_SCREEN_BUFFER_INFOEX>)
        )
        GetConsoleScreenBufferInfoEx(GetStdHandle(STD_OUTPUT_HANDLE), &screenBuffer ) |> ignore
        screenBuffer.colorScheme <- colorScheme
        SetConsoleScreenBufferInfoEx(GetStdHandle(STD_OUTPUT_HANDLE), &screenBuffer ) |> ignore

      let updateFontInfo (fontParams:FontParams) =
        let hnd = GetStdHandle(STD_OUTPUT_HANDLE)
        let mutable newFontInfo = CONSOLE_FONT_INFO_EX()
        newFontInfo.cbSize <- uint32 (Marshal.SizeOf(newFontInfo))
        newFontInfo.FontFamily <- TMPF_TRUETYPE
        newFontInfo.FaceName <- fontParams.Name
        newFontInfo.dwFontSize <- {X = int16 1 ; Y = fontParams.Size}
        SetCurrentConsoleFontEx(hnd, false, newFontInfo) |> ignore

      let consoleEncodingUTF8 (flg:bool) =
        if flg then
          Console.InputEncoding   <- Text.UTF8Encoding.UTF8
          Console.OutputEncoding  <- Text.UTF8Encoding.UTF8

      let consoleColor (colorParams:ColorParams) =
        match colorParams.backGroundColor with
        | 0  -> Console.BackgroundColor <- ConsoleColor.Black
        | 1  -> Console.BackgroundColor <- ConsoleColor.DarkBlue
        | 2  -> Console.BackgroundColor <- ConsoleColor.DarkGreen
        | 3  -> Console.BackgroundColor <- ConsoleColor.DarkCyan
        | 4  -> Console.BackgroundColor <- ConsoleColor.DarkRed
        | 5  -> Console.BackgroundColor <- ConsoleColor.DarkMagenta
        | 6  -> Console.BackgroundColor <- ConsoleColor.DarkYellow
        | 7  -> Console.BackgroundColor <- ConsoleColor.Gray
        | 8  -> Console.BackgroundColor <- ConsoleColor.DarkGray
        | 9  -> Console.BackgroundColor <- ConsoleColor.Blue
        | 10 -> Console.BackgroundColor <- ConsoleColor.Green
        | 11 -> Console.BackgroundColor <- ConsoleColor.Cyan
        | 12 -> Console.BackgroundColor <- ConsoleColor.Red
        | 13 -> Console.BackgroundColor <- ConsoleColor.Magenta
        | 14 -> Console.BackgroundColor <- ConsoleColor.Yellow
        | _  -> Console.BackgroundColor <- ConsoleColor.White
        match colorParams.foreGroundColor with
        | 0  -> Console.ForegroundColor <- ConsoleColor.Black
        | 1  -> Console.ForegroundColor <- ConsoleColor.DarkBlue
        | 2  -> Console.ForegroundColor <- ConsoleColor.DarkGreen
        | 3  -> Console.ForegroundColor <- ConsoleColor.DarkCyan
        | 4  -> Console.ForegroundColor <- ConsoleColor.DarkRed
        | 5  -> Console.ForegroundColor <- ConsoleColor.DarkMagenta
        | 6  -> Console.ForegroundColor <- ConsoleColor.DarkYellow
        | 7  -> Console.ForegroundColor <- ConsoleColor.Gray
        | 8  -> Console.ForegroundColor <- ConsoleColor.DarkGray
        | 9  -> Console.ForegroundColor <- ConsoleColor.Blue
        | 10 -> Console.ForegroundColor <- ConsoleColor.Green
        | 11 -> Console.ForegroundColor <- ConsoleColor.Cyan
        | 12 -> Console.ForegroundColor <- ConsoleColor.Red
        | 13 -> Console.ForegroundColor <- ConsoleColor.Magenta
        | 14 -> Console.ForegroundColor <- ConsoleColor.Yellow
        | _  -> Console.ForegroundColor <- ConsoleColor.White