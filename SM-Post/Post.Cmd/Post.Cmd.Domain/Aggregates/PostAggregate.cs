using System;
using System.Runtime.CompilerServices;
using CQRS.Core.Domain;
using Post.Core.Domain;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {

        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post!");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}!");
            }

            RaiseEvent(new MessageUpdateEvent
            {
                Id = _id,
                message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post!");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.id;
        }

        public void AddComment(string comment, string usename)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add an comment inactive post!");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}!");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommendAddedEvent @event)
        {
            _id = @event.id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));

        }

        public void EditComment(Guid commentId, string EventCommandEventArgs, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit a comment of an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by anothr user!");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = commentId,
                Username = username.
                EditDate = DateTime.Now
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by anothr user!");
            }

            RaiseEvent(new CommentRemovedEvent {
                Id = _id,
                CommentId = commentId,
            });
        }
        
        public void Apply(CommentRemovedEvent @event) {
            _id = @event.id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username) {
            if(_active) {
                throw new InvalidOperationException("The post has already been removed");
            }
            if(!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase)){
                throw new InvalidOperationException("You are not allowed to delete a post that was made by someone else");
            }
            RaseEvent(new PostRemovedEvent(
                Id = _id
            ));
        }

        public void Apply(PostRemovedEvent @event) {
            _id = @event.Id;
            _active = false;
        }
    }
}