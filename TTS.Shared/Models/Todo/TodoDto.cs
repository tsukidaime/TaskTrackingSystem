using System;

namespace TTS.Shared.Models.Todo
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }

        public Guid JobId { get; set; }
    }
}