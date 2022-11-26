namespace CmdConsole

module ColorScheme =

  open System
  open System.Runtime.InteropServices

  module private Imported =

    [<StructLayoutAttribute(LayoutKind.Sequential,Pack=2)>]
    [<Struct>]
    type COORD =
      val mutable x : int16
      val mutable y : int16

    [<StructLayoutAttribute(LayoutKind.Sequential,Pack=2)>]
    [<Struct>]
    type SMALL_RECT =
      val mutable Left   : int16
      val mutable Top    : int16
      val mutable Right  : int16
      val mutable Bottom : int16

    type COLORREF = uint

    [<StructLayoutAttribute(LayoutKind.Sequential)>]
    [<Struct>]
    type CONSOLE_SCREEN_BUFFER_INFOEX =
      val mutable cbSize               : int
      val mutable dwSize               : COORD
      val mutable dwCursorPosition     : COORD
      val mutable wAttributes          : int16
      val mutable srWindow             : SMALL_RECT
      val mutable dwMaximumWindowSize  : COORD
      val mutable wPopupAttributes     : int16
      val mutable bFullscreenSupported : bool
      val mutable black                : COLORREF
      val mutable darkBlue             : COLORREF
      val mutable darkGreen            : COLORREF
      val mutable darkCyan             : COLORREF
      val mutable darkRed              : COLORREF
      val mutable darkMagenta          : COLORREF
      val mutable darkYellow           : COLORREF
      val mutable gray                 : COLORREF
      val mutable darkGray             : COLORREF
      val mutable blue                 : COLORREF
      val mutable green                : COLORREF
      val mutable cyan                 : COLORREF
      val mutable red                  : COLORREF
      val mutable magenta              : COLORREF
      val mutable yellow               : COLORREF
      val mutable white                : COLORREF

    let STD_OUTPUT_HANDLE = -11

    [<DllImportAttribute("kernel32.dll",SetLastError=true)>]
    extern IntPtr GetStdHandle(int nStdHndle)

    [<DllImportAttribute("kernel32.dll",SetLastError=true)>]
    extern IntPtr GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, CONSOLE_SCREEN_BUFFER_INFOEX& ConsoleScreenBufferInfo)

    [<DllImportAttribute("kernel32.dll",SetLastError=true)>]
    extern IntPtr SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, CONSOLE_SCREEN_BUFFER_INFOEX& ConsoleScreenBufferInfo)


  open Imported

  type RGB = {
    Red   : int
    Green : int
    Blue  : int
  }

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

  let ToRGB (colorRef:uint) =
    (
         colorRef         &&& (uint 0xFF) |> int
      , (colorRef >>>  8) &&& (uint 0xFF) |> int
      , (colorRef >>> 16) &&& (uint 0xFF) |> int
    )

  let ToCOLORREF red green blue =
    let r =  uint red
    let g = (uint green) <<< 8
    let b = (uint blue)  <<< 16
    r + g + b

  let updateColorPalette (colorScheme:ColorScheme) =

    let mutable screenBuffer = new CONSOLE_SCREEN_BUFFER_INFOEX(
        cbSize = Marshal.SizeOf(typeof<CONSOLE_SCREEN_BUFFER_INFOEX>)
    )

    GetConsoleScreenBufferInfoEx(GetStdHandle(STD_OUTPUT_HANDLE), &screenBuffer ) |> ignore

    screenBuffer.black       <- (ToCOLORREF colorScheme.black.Red        colorScheme.black.Green       colorScheme.black.Blue)
    screenBuffer.darkBlue    <- (ToCOLORREF colorScheme.darkBlue.Red     colorScheme.darkBlue.Green    colorScheme.darkBlue.Blue)
    screenBuffer.darkGreen   <- (ToCOLORREF colorScheme.darkGreen.Red    colorScheme.darkGreen.Green   colorScheme.darkGreen.Blue)
    screenBuffer.darkCyan    <- (ToCOLORREF colorScheme.darkCyan.Red     colorScheme.darkCyan.Green    colorScheme.darkCyan.Blue)
    screenBuffer.darkRed     <- (ToCOLORREF colorScheme.darkRed.Red      colorScheme.darkRed.Green     colorScheme.darkRed.Blue)
    screenBuffer.darkMagenta <- (ToCOLORREF colorScheme.darkMagenta.Red  colorScheme.darkMagenta.Green colorScheme.darkMagenta.Blue)
    screenBuffer.darkYellow  <- (ToCOLORREF colorScheme.darkYellow.Red   colorScheme.darkYellow.Green  colorScheme.darkYellow.Blue)
    screenBuffer.gray        <- (ToCOLORREF colorScheme.gray.Red         colorScheme.gray.Green        colorScheme.gray.Blue)
    screenBuffer.darkGray    <- (ToCOLORREF colorScheme.darkGray.Red     colorScheme.darkGray.Green    colorScheme.darkGray.Blue)
    screenBuffer.blue        <- (ToCOLORREF colorScheme.blue.Red         colorScheme.blue.Green        colorScheme.blue.Blue)
    screenBuffer.green       <- (ToCOLORREF colorScheme.green.Red        colorScheme.green.Green       colorScheme.green.Blue)
    screenBuffer.cyan        <- (ToCOLORREF colorScheme.cyan.Red         colorScheme.cyan.Green        colorScheme.cyan.Blue)
    screenBuffer.red         <- (ToCOLORREF colorScheme.red.Red          colorScheme.red.Green         colorScheme.red.Blue)
    screenBuffer.magenta     <- (ToCOLORREF colorScheme.magenta.Red      colorScheme.magenta.Green     colorScheme.magenta.Blue)
    screenBuffer.yellow      <- (ToCOLORREF colorScheme.yellow.Red       colorScheme.yellow.Green      colorScheme.yellow.Blue)
    screenBuffer.white       <- (ToCOLORREF colorScheme.white.Red        colorScheme.white.Green       colorScheme.white.Blue)

    SetConsoleScreenBufferInfoEx(GetStdHandle(STD_OUTPUT_HANDLE), &screenBuffer ) |> ignore