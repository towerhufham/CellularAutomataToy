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
  let wrap (n: int) (max: int): int = 
    if n < 0 then max + n else 
      if n >= max then max - n else n 
  let GoL (cells: bool list) (width: int) (height: int): bool list =
    let toXY (i: int) = wrap (i % width) width, wrap (i / width) height 
    let toI (xy: int * int) = snd xy * width + fst xy
    let addXY (xy1: int * int) (xy2: int * int) = wrap (fst xy1 + fst xy2) width, wrap (snd xy1 + snd xy2) height 
    let nei (xy: int * int) = 
      [-1,-1;0,-1;1,-1;-1,0;1,0;-1,1;0,1;1,1]
      |> List.map (fun p -> addXY xy p)
      |> List.map (fun p -> cells[toI p])
      |> List.sumBy (fun b -> if b then 1 else 0)
    List.mapi (fun i c -> 
      match c, nei (toXY i) with
        | true, a when a >= 2 && a <= 3 -> true
        | false, a when a = 3 -> true
        | _ -> false
    ) cells

  let view () =
    Component(fun ctx ->
      let width = 60
      let height = 30
      let cells = ctx.useState (List.replicate (width * height) false)
      let colDefs = String.concat "," (List.replicate width "25")
      let rowDefs = String.concat "," (List.replicate height "25")
      DockPanel.create [
        DockPanel.children [
          Grid.create [
            Grid.columnDefinitions colDefs
            Grid.rowDefinitions rowDefs
            Grid.dock Dock.Top
            Grid.children (
              List.init (width * height) (fun i -> 
                CheckBox.create [
                  Grid.column (i % width)
                  Grid.row (i / width)
                  CheckBox.isChecked cells.Current[i]
                  CheckBox.onChecked (fun _ -> 
                    List.mapi (fun i2 c -> if i = i2 then true else c) cells.Current
                    |> cells.Set)
                  CheckBox.onUnchecked (fun _ -> 
                    List.mapi (fun i2 c -> if i = i2 then false else c) cells.Current
                    |> cells.Set)
                  CheckBox.fontSize 10.0
                  CheckBox.horizontalAlignment HorizontalAlignment.Center
                  CheckBox.verticalAlignment VerticalAlignment.Center
                ]
              )
            )
          ]
          Button.create [
            Button.content "Step"
            Button.dock Dock.Bottom
            Button.onClick (fun _ -> GoL cells.Current width height |> cells.Set)
          ]
          Button.create [
            Button.content "Random"
            Button.dock Dock.Bottom
            Button.onClick (fun _ -> List.init (width * height) (fun _ -> List.randomChoice [true; false]) |> cells.Set)
          ]
        ]
      ]
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
