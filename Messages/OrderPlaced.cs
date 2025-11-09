namespace Messages;

public record OrderPlaced
{
    public string OrderId { get; init; }
    public int PlaceId { get; init; }
}