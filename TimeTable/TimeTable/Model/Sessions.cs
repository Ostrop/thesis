//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeTable.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sessions
    {
        public int SessionId { get; set; }
        public Nullable<int> AudienceId { get; set; }
        public int GroupId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> SessionNumber { get; set; }
    
        public virtual Audiences Audiences { get; set; }
        public virtual Groups Groups { get; set; }
    }
}
