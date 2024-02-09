using Wolverine;
using Wolverine.AmazonSqs;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(wolverine =>
{
    wolverine
        .UseAmazonSqsTransportLocally();

    wolverine
        .PublishMessage<ProductCreated>()
        .ToSqsQueue("product-created");

    wolverine
        .ListenToSqsQueue("product-created")
        .ConfigureDeadLetterQueue("product-created-error");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();

public record ProductCreated(Guid Id);

public static class ProductCreatedHandler
{
    public static void Handle(ProductCreated message)
    {
        Console.WriteLine(message.Id);
    }
}
