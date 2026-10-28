using RayCraft.Examples.Examples;

namespace RayCraft.Examples.Docs;

public static class ManualDocs
{
    public static IReadOnlyList<ControlDoc> All { get; } =
    [
        new(
            "event-calendar",
            "EventCalendar",
            "A month grid that shows events on their days.",
            [new("Events in a month", typeof(EventCalendarBasic))],
            [
                new("Events", "IEnumerable<CalendarEvent>?", "null", "The events to draw. CalendarEvent(Title, Start, End?, Severity) puts an event on every day from Start to End; without End it is a one-day event."),
                new("Month", "DateTime", "the current month", "Any date of the month to show. Use @bind-Month to follow the arrows of the calendar."),
                new("MonthChanged", "EventCallback<DateTime>", "", "Raised with the first day of the new month."),
                new("MaxEventsPerDay", "int", "3", "How many events a day cell lists before it shows a count of the rest."),
                new("FirstDayOfWeek", "DayOfWeek?", "from the culture", "The first column of the grid."),
                new("OnEventClick", "EventCallback<CalendarEvent>", "", "Raised when an event is chosen."),
                new("OnDayClick", "EventCallback<DateTime>", "", "Raised when a day is chosen.")
            ],
            [
                "Severity colors the event: Info (default), Success, Warning, Error or Neutral.",
                "Month names and weekday names follow the culture of the page."
            ]),
        new(
            "form-builder",
            "FormBuilder",
            "Builds a form with validation from a list of field descriptions.",
            [new("Registration form", typeof(FormBuilderBasic))],
            [
                new("Fields", "IEnumerable<FormField>?", "null", "FormField(Id, Label, Kind, Required, Options, Placeholder, Help, Min, Max, Lines). Kind is Text, Email, Password, Number, Date, TextArea, Select, Radio, Checkbox or Switch."),
                new("Values", "IDictionary<string, object?>?", "an empty dictionary", "Holds the values by field Id. Initial values are read from it and the form writes to it."),
                new("OnSubmit", "EventCallback<IReadOnlyDictionary<string, object?>>", "", "Raised with the values after every required field and every rule passed."),
                new("SubmitText", "string?", "from the culture", "Text of the submit button."),
                new("Actions", "RenderFragment?", "null", "Extra content next to the submit button.")
            ],
            [
                "Number fields return double, Date fields DateTime, Checkbox and Switch bool, the other kinds string.",
                "Options is used by Select and Radio. Min and Max limit Number fields. Lines is the height of TextArea."
            ]),
        new(
            "captcha",
            "Captcha",
            "A small arithmetic question that asks for a human answer.",
            [new("Enable a button", typeof(CaptchaBasic))],
            [
                new("Verified", "bool", "false", "True after a correct answer. Use @bind-Verified."),
                new("VerifiedChanged", "EventCallback<bool>", "", "Raised when the state changes.")
            ],
            [
                "It keeps out only the simplest bots. Check the real protection of a public form on the server."
            ]),
        new(
            "qr-code",
            "QrCode",
            "Draws a QR code as inline SVG from a text.",
            [new("Text to code", typeof(QrCodeBasic))],
            [
                new("Value", "string", "required", "The text to encode. An empty text draws nothing."),
                new("Label", "string?", "null", "Accessible name of the picture."),
                new("Margin", "int", "4", "Quiet zone around the code, in modules.")
            ],
            [
                "The code uses fixed black and white colors in every theme so that scanners and printouts work."
            ]),
        new(
            "barcode",
            "Barcode",
            "Draws a one-dimensional barcode as inline SVG.",
            [new("Barcode formats", typeof(BarcodeBasic))],
            [
                new("Value", "string", "required", "The text or digits to encode."),
                new("Format", "BarcodeFormat", "Code128", "Code128, Code39, Ean13, Ean8 or UpcA."),
                new("Label", "string?", "null", "Accessible name of the picture."),
                new("Height", "int", "60", "Height of the bars in pixels."),
                new("ShowText", "bool", "true", "Shows the value under the bars."),
                new("ShowDownload", "bool", "false", "Adds buttons that download the barcode as SVG or PNG."),
                new("FileName", "string", "barcode", "Name of the downloaded file.")
            ],
            [
                "For Ean13, Ean8 and UpcA pass the digits without the check digit (12, 7 and 11 digits) and it is added; a value with the check digit is accepted if the digit is correct."
            ]),
        new(
            "address-field",
            "AddressField",
            "A field for several email addresses, with suggestions and paste of lists.",
            [
                new("To field with suggestions", typeof(AddressFieldBasic)),
                new("Only valid addresses", typeof(AddressFieldStrict))
            ],
            [
                new("Values", "IReadOnlyCollection<string>?", "null", "The addresses, for example \"Ana Petrova <ana@example.com>\". Use @bind-Values."),
                new("ValuesChanged", "EventCallback<IReadOnlyCollection<string>>", "", "Raised when an address is added, edited or removed."),
                new("SuggestionsProvider", "Func<string, CancellationToken, Task<IEnumerable<AddressSuggestion>>>?", "null", "Returns suggestions for the typed text. AddressSuggestion(Address, Name?). The token is cancelled by a newer request."),
                new("MinChars", "int", "1", "Characters needed before suggestions are asked for."),
                new("DebounceMilliseconds", "int", "200", "Pause after typing before the provider is called."),
                new("MaxSuggestions", "int", "8", "Longest list of suggestions."),
                new("MaxItems", "int?", "null", "Most addresses the field accepts."),
                new("AllowInvalid", "bool", "true", "False refuses text that is not an address; true keeps it and marks it."),
                new("AllowDuplicates", "bool", "false", "Allows the same address twice."),
                new("Validator", "Func<string, bool>?", "null", "Extra rule for an address, for example a domain list."),
                new("InvalidCountChanged", "EventCallback<int>", "", "Raised with the number of addresses that are not valid."),
                new("Label", "string?", "null", "Label of the field."),
                new("HelperText", "string?", "null", "Help shown under the field."),
                new("ErrorText", "string?", "null", "Error shown under the field."),
                new("Placeholder", "string?", "null", "Text of the empty input."),
                new("Disabled", "bool", "false", "Disables the field."),
                new("ReadOnly", "bool", "false", "Shows the addresses without editing.")
            ],
            [
                "Enter, a comma or a semicolon ends an address, and a pasted list is split into addresses. Double-click an address to edit it.",
                "Pass a fixed list of contacts through SuggestionsProvider; it can also call a server."
            ]),
        new(
            "splitter",
            "Splitter",
            "Two panes with a bar that moves with the mouse, touch or arrow keys.",
            [
                new("Horizontal and vertical", typeof(SplitterBasic)),
                new("Collapse and narrow screens", typeof(SplitterCollapse))
            ],
            [
                new("First", "RenderFragment?", "null", "Content of the first pane."),
                new("Second", "RenderFragment?", "null", "Content of the second pane."),
                new("Vertical", "bool", "false", "Stacks the panes top and bottom."),
                new("Size", "double", "50", "Size of the first pane in percent. Use @bind-Size."),
                new("SizeChanged", "EventCallback<double>", "", "Raised while the bar moves."),
                new("MinSize", "double", "10", "Smallest size of either pane in percent."),
                new("PersistKey", "string?", "null", "Stores the size in the browser under this key."),
                new("CollapsiblePane", "SplitterPane", "None", "The pane that collapses on Enter or a double-click of the bar."),
                new("Collapsed", "SplitterPane", "None", "The pane that is collapsed now. Use @bind-Collapsed."),
                new("SingleBelow", "Breakpoint?", "null", "Below this width only one pane is shown."),
                new("ActivePane", "SplitterPane", "First", "The pane shown in that narrow mode. Use @bind-ActivePane.")
            ],
            [
                "Give the Splitter a height (Style or Class); its panes fill it.",
                "Focus the bar and press the arrow keys (Shift for bigger steps), Home or End; Enter or a double-click collapses the CollapsiblePane."
            ]),
        new(
            "property-grid",
            "PropertyGrid",
            "Edits the public properties of any object, grouped by category.",
            [new("Object settings", typeof(PropertyGridBasic))],
            [
                new("Value", "object?", "required", "The object to edit. The grid changes its properties in place."),
                new("Label", "string?", "null", "Accessible name of the grid."),
                new("ReadOnly", "bool", "false", "Shows the values without editing."),
                new("OnChanged", "EventCallback<PropertyChange>", "", "Raised with PropertyChange(Name, Value) after a property was set.")
            ],
            [
                "Text, number, boolean, enum and date properties are supported.",
                "[Category], [DisplayName], [Description] and [ReadOnly(true)] from System.ComponentModel shape the rows."
            ])
    ];
}
