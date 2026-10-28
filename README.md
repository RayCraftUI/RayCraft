# RayCraft examples

Source: https://github.com/RayCraftUI/RayCraft

Runnable examples for the free controls of [RayCraft.Blazor](https://www.nuget.org/packages/RayCraft.Blazor): 160 control pages in 18 groups, each with live examples, their source and the table of parameters. The home page lists the groups and has a search box.

Coverage: 154 of the 160 controls have an example of their own or are shown in use inside another example; the remaining six (DialogHost, Lightbox, ColorDialog, FontDialog, Statistic, Fullscreen) have a page with the parameters and no example yet. Examples that do not compile against the package or need controls outside it are listed with the reason in `tools/excluded-examples.json`.

This repository holds only demo code. The library comes from the NuGet package and its source is not here.

Support: https://support.raycraft.dev (support@raycraft.dev). Security reports: security@raycraft.dev, see SECURITY.md.

## Run

Needs the .NET 10 SDK.

```
dotnet run --project src/RayCraft.Examples
```

Open the address that the command prints. The theme button in the top bar switches between light and dark.

## Layout

- `src/RayCraft.Examples/Examples` has one `.razor` file per example. Their source is embedded and shown on the pages.
- `src/RayCraft.Examples/Docs/ExampleIndex.g.cs` is generated and lists the controls by group with their examples. `ManualDocs.cs` has the written descriptions of the first eight controls. `ControlCatalog.cs` joins them, and `ParameterReflector.cs` reads the parameters from the package.
- `tools/generate-examples.mjs` rebuilds `Examples` and the index from a folder of source examples: `node tools/generate-examples.mjs <folder> <product name in the source>`. `tools/free-controls.json` has the groups, `tools/excluded-examples.json` the skipped examples.
- `src/RayCraft.Examples/Components` has the layout and the page that draws a control.

## Use in your own project

```
dotnet add package RayCraft.Blazor
```

Register `builder.Services.AddRayCraft()`, link `_content/RayCraft.Blazor/css/raycraft.css` and `_content/RayCraft.Blazor/raycraft.js` in `App.razor`, add `@using RayCraft.Blazor.Components` to `_Imports.razor` and set an interactive render mode. `src/RayCraft.Examples/Components/App.razor` and `Program.cs` show all of it.

## License

The code in this repository is under the MIT license (`LICENSE`). The RayCraft.Blazor package has its own license, in the package.

## Screenshots

Light and dark theme, in `docs/images`:

- event-calendar: [light](docs/images/event-calendar.light.png), [dark](docs/images/event-calendar.dark.png)
- form-builder: [light](docs/images/form-builder.light.png), [dark](docs/images/form-builder.dark.png)
- captcha: [light](docs/images/captcha.light.png), [dark](docs/images/captcha.dark.png)
- qr-code: [light](docs/images/qr-code.light.png), [dark](docs/images/qr-code.dark.png)
- barcode: [light](docs/images/barcode.light.png), [dark](docs/images/barcode.dark.png)
- address-field: [light](docs/images/address-field.light.png), [dark](docs/images/address-field.dark.png)
- splitter: [light](docs/images/splitter.light.png), [dark](docs/images/splitter.dark.png)
- property-grid: [light](docs/images/property-grid.light.png), [dark](docs/images/property-grid.dark.png)
