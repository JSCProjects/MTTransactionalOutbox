namespace Messages;

public record PlaceOrder
{
    public string OrderId { get; init; }
    public int Amount { get; init; } = 3000;
}