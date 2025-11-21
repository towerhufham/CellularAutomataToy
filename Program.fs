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
  let view () =
    Component(fun ctx ->
      let state = ctx.useState 0
      Grid.create [
        Grid.columnDefinitions [
          ColumnDefinition (GridLength 100)
          ColumnDefinition (GridLength 100)
        ]
        Grid.rowDefinitions [
          RowDefinition (GridLength 100)
          RowDefinition (GridLength 100)
        ]
        Grid.children [
          TextBlock.create [
            Grid.column 0
            Grid.row 0
            TextBlock.text "1"
          ]
          TextBlock.create [
            Grid.column 1
            Grid.row 0
            TextBlock.text "2"
          ]
          TextBlock.create [
            Grid.column 0
            Grid.row 1
            TextBlock.text "3"
          ]
          TextBlock.create [
            Grid.column 1
            Grid.row 1
            TextBlock.text "4"
          ]
        ]
      ]
      // DockPanel.create [
      //   DockPanel.children [
      //     Button.create [
      //       Button.dock Dock.Bottom
      //       Button.onClick (fun _ -> state.Set(state.Current - 1))
      //       Button.content "-"
      //       Button.horizontalAlignment HorizontalAlignment.Stretch
      //       Button.horizontalContentAlignment HorizontalAlignment.Center
      //     ]
      //     Button.create [
      //       Button.dock Dock.Bottom
      //       Button.onClick (fun _ -> state.Set(state.Current + 1))
      //       Button.content "+"
      //       Button.horizontalAlignment HorizontalAlignment.Stretch
      //       Button.horizontalContentAlignment HorizontalAlignment.Center
      //     ]
      //     TextBlock.create [
      //       TextBlock.dock Dock.Top
      //       TextBlock.fontSize 48.0
      //       TextBlock.verticalAlignment VerticalAlignment.Center
      //       TextBlock.horizontalAlignment HorizontalAlignment.Center
      //       TextBlock.text (string state.Current)
      //     ]
      //     TextBlock.create [
      //       TextBlock.dock Dock.Top
      //       TextBlock.fontSize 48.0
      //       TextBlock.verticalAlignment VerticalAlignment.Center
      //       TextBlock.horizontalAlignment HorizontalAlignment.Center
      //       TextBlock.text "Hi :3"
      //     ]
      //   ]
      // ]
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
