// Datadog tracer
using Datadog.Trace;

// Adding System to add the date as tag in a span
using System; 

// Initialize a new instance of the WebApplication builder
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Respond "Hello world!" on /
app.MapGet("/", () => "Hello World!");

// Endpoint where we add a tag
app.MapGet("/add-tag", () => {
    // Getting the active span
    var scope = Tracer.Instance.ActiveScope;

    if (scope != null)
    {
        // Adding a custom span tag for use in the Datadog UI. 
        // The tag added here is the current date. The UI already allows scoping by date, this is just for example purposes.
        scope.Span.SetTag("date", DateTime.Today.ToString());
    }

    return "Here we add a tag";
});

// Endpoint where we throw an exception
app.MapGet("/exception", () => {
    // Getting the active span
    var scope = Tracer.Instance.ActiveScope;

    if (scope != null)
    {
        // Throwing an exception by dividing by 0
        int numer = 1;
        int denom = 0;
        int result;
        try
        {
            result = numer/denom;
        }
        catch(Exception e)
        {
            scope.Span.SetException(e);
        }
    }

    return "Throwing an exeption!";
});

// Endpoint where we add a custom span
app.MapGet("/manual-span", () => {
    using (var parentScope = Tracer.Instance.StartActive("manual.sortorders")){
        parentScope.Span.ResourceName = "Parent Resource";
        using (var childScope = Tracer.Instance.StartActive("manual.sortorders.child")){
            // Nest using statements around the code to trace
            childScope.Span.ResourceName = "Child Resource";
        }
    }

    return "Adding a span manually";
});

app.Run();