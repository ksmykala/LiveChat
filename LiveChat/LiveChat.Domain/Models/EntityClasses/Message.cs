//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiveChat.Domain.Models.EntityClasses
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public System.Guid Id { get; set; }
        public string Content { get; set; }
        public Nullable<System.Guid> ConversationId { get; set; }
        public System.DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
    }
}
