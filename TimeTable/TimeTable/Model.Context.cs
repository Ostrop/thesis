﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeTable
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dtbTimtableEntities : DbContext
    {
        public dtbTimtableEntities()
            : base("name=dtbTimtableEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Audiences> Audiences { get; set; }
        public virtual DbSet<Availability> Availability { get; set; }
        public virtual DbSet<Disciplines> Disciplines { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Employees_Disciplines> Employees_Disciplines { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<Specialities> Specialities { get; set; }
        public virtual DbSet<StudyPlan> StudyPlan { get; set; }
        public virtual DbSet<StudyPlan_Disciplines> StudyPlan_Disciplines { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
