namespace CounterApp

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout

module Main =
  let buildGrid (width: int) (height: int) =
    let colDefs = String.concat "," (List.replicate width "25")
    let rowDefs = String.concat "," (List.replicate height "25")
    Grid.create [
      Grid.columnDefinitions colDefs
      Grid.rowDefinitions rowDefs
      Grid.children (
        List.init (width * height) (fun i -> 
          TextBlock.create [
            Grid.column (i % width)
            Grid.row (i / width)
            TextBlock.text (string i)
            TextBlock.fontSize 10.0
            TextBlock.horizontalAlignment HorizontalAlignment.Center
            TextBlock.verticalAlignment VerticalAlignment.Center
          ]
        )
      )
    ]

  let view () =
    Component(fun ctx ->
      // let state = ctx.useState 0
      buildGrid 60 30
    )

type MainWindow() =
  inherit HostWindow()
  do
    base.Title <- "Counter Example"
    base.Content <- Main.view ()

type App() =
  inherit Application()
  override this.Initialize() =
    this.Styles.Add (FluentTheme())
    this.RequestedThemeVariant <- Styling.ThemeVariant.Dark
  override this.OnFrameworkInitializationCompleted() =
    match this.ApplicationLifetime with
    | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
        desktopLifetime.MainWindow <- MainWindow()
    | _ -> ()

module Program =
  [<EntryPoint>]
  let main(args: string[]) =
    AppBuilder
      .Configure<App>()
      .UsePlatformDetect()
      .UseSkia()
      .StartWithClassicDesktopLifetime(args)
