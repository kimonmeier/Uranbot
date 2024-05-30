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

    public IRequest<bool> Request
    {
        get => Event.Request;
        set => Event.Request = value;
    }

    public BaseEvent Event { get; set; } = new();

    public byte[] RequestJson
    {
        get => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Event));
        set => Event = JsonConvert.DeserializeObject<BaseEvent>(Encoding.UTF8.GetString(value)) ?? throw new ArgumentException();
    }
}