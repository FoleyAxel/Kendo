[<JS>]
module Kendo.Client

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open WebSharper.Kendo.Wrapper

module C = Column
module G = Grid

type Philosopher =
    {
        Name: string
        LastName: string
        Age: int
        Died: EcmaScript.Date
    }

    with static member ofPerson (p: Data.Philosopher) =
            {
                Name = p.Name
                LastName = p.LastName
                Age = p.Age
                Died = p.Died.ToEcma()
            }

let renderData =
    G.Default [
        C.field "Name" "Name"
         |> C.width 150
        C.field "LastName" "Last Name"
        C.field "Age" "Age"
         |> C.asNumber
         |> C.editable
         |> C.alignRight
         |> C.width 120
        C.field "Died" "Died On"
         |> C.shortDateFormat
         |> C.asDate
         |> C.editable
         |> C.width 150
        C.command "Show JSON" (fun v grid ->
            Json.Stringify v
            |> JavaScript.Alert
            grid.SaveRow()
        )
        |> C.width 140
    ]
    |> G.paging 5
    |> G.pageSizer
    |> G.groupable
    |> G.filterable
    |> G.columnResizable
    |> G.reorderable
    |> G.addButton
    |> G.cancelButton
//    |> Grid.withRowSelect (Json.Stringify >> JavaScript.Alert)
    |> G.renderData

let page() =
    let grid =
        Data.philosophers()
        |> Seq.map Philosopher.ofPerson
        |> renderData
    Div [
        Tabs.createTabs [
            Tabs.create "Grid" (fun () -> Div [grid])
            Tabs.create "TreeView" (fun () -> Div [Text "test"])
        ]
    ]