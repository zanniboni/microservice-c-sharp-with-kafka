using CQRS.Core.Messages;

namespace CQRS.Core.Events
{
    public abstract class BaseEvent: Message
    {
        protected BaseEvent(string Type) {
            this.Type = Type;
        }
        
        public int Version { get; set; }
        public string Type { get; set; }
    }
}