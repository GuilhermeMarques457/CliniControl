using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Dentist> Dentists { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<RequestedPatient> RequestedPatients { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Appointment>().ToTable("Appointments");
            builder.Entity<Clinic>().ToTable("Clinics");
            builder.Entity<Dentist>().ToTable("Dentists");
            builder.Entity<Patient>().ToTable("Patients");
            builder.Entity<RequestedPatient>().ToTable("RequestedPatients");
            builder.Entity<Reminder>().ToTable("Reminders");
         
        }
    }
}
