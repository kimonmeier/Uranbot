namespace UranBot.Public.Database.Entities.Base;

public sealed class BaseEvent
{
    [JsonConverter(typeof(InterfaceConverter<IRequest<bool>>))]
    public IRequest<bool> Request { get; set; }
}