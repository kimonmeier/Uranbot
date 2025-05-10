// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

using System.Text;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace UranBot.Public.Database.Entities;

public class DiscordReaction : BaseEntity
{
    public long MessageId { get; set; }

    public virtual DiscordMessage Message { get; set; }

    public string EmoteName { get; set; }

    public IRequest<bool>? RemoveRequest
    {
        get => RemoveEvent.Request;
        set => RemoveEvent.Request = value;
    }

    public BaseEvent RemoveEvent { get; set; } = new();

    public byte[] RemoveRequestJson
    {
        get => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(RemoveEvent));
        set => RemoveEvent = JsonConvert.DeserializeObject<BaseEvent>(Encoding.UTF8.GetString(value)) ?? throw new ArgumentException();
    }
    
    public IRequest<bool> AddRequest
    {
        get => AddEvent.Request;
        set => AddEvent.Request = value;
    }

    public BaseEvent AddEvent { get; set; } = new();

    public byte[] AddRequestJson
    {
        get => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(AddEvent));
        set => AddEvent = JsonConvert.DeserializeObject<BaseEvent>(Encoding.UTF8.GetString(value)) ?? throw new ArgumentException();
    }
}