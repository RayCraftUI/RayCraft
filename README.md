# RayCraft examples

Runnable examples for the free controls of [RayCraft.Blazor](https://www.nuget.org/packages/RayCraft.Blazor): EventCalendar, FormBuilder, Captcha, QrCode, Barcode, AddressField, Splitter and PropertyGrid. Each page has a live example with its source and the table of parameters.

This repository holds only demo code. The library comes from the NuGet package and its source is not here.

Support: https://support.raycraft.dev

## Run

Needs the .NET 10 SDK.

```
dotnet run --project src/RayCraft.Examples
```

Open the address that the command prints. The theme button in the top bar switches between light and dark.

## Layout

- `src/RayCraft.Examples/Examples` has one `.razor` file per example. Their source is embedded and shown on the pages.
- `src/RayCraft.Examples/Docs/ControlCatalog.cs` lists the controls, their examples and parameters.
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
