﻿using System;

namespace GodOx.Blog.API.Models.Dtos.Input
{
    public class MessageInput
    {
        public string UserName { get; set; }
        public string Types { get; set; }
        public string BusinessId { get; set; }
        public string IP { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int TenantId { get; set; }
        public string Content { get; set; }
    }
}
