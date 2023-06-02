using final_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using models;
using System.Data.Entity.ModelConfiguration.Conventions;
using final_project.Models;
using Duende.IdentityServer.EntityFramework.Interfaces;

namespace final_project.Models.Data
{

    public class identityContext :IdentityDbContext<ApplicationUser>{
        public identityContext(DbContextOptions<identityContext> options) : base(options) {
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schadule> Schadules { get; set; }
        public DbSet<country> Countries { get; set; }
        public DbSet<title> Titles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Department>()
                .HasMany(p => p.Doctors)
                .WithOne(p => p.Department)
                .HasForeignKey(p => p.DepartmentId)
                .IsRequired(false);
            modelBuilder.Entity<Doctor>()
                .HasMany(p => p.Appointments)
                .WithOne(p => p.Doctor)
                .HasForeignKey(p => p.DoctorId)
                .IsRequired(false);
            modelBuilder.Entity<Doctor>()
                .HasMany(p => p.feedbacks)
                .WithOne(p => p.Doctor)
                .HasForeignKey(p => p.DoctorId)
                .IsRequired(false);
            modelBuilder.Entity<Doctor>()
                .HasMany(p => p.Clinics)
                .WithOne(p => p.Doctor)
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            modelBuilder.Entity<title>()
                .HasMany(p => p.Doctors)
                .WithOne(p => p.Title)
                .HasForeignKey(p => p.titleid)
                .IsRequired(false);
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId)
                .IsRequired(false);
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.feedbacks)
                .WithOne(p => p.patient)
                .HasForeignKey(p => p.PatientId)
                .IsRequired(false);
            modelBuilder.Entity<Clinic>()
                        .HasMany(p => p.Schadules)
                        .WithOne(p => p.Clinic)
                        .HasForeignKey(p => p.ClinicId)
                        .IsRequired(true);
            modelBuilder.Entity<country>()
            .HasMany(p => p.Clinics)
            .WithOne(p => p.country)
            .HasForeignKey(p => p.countryid)
            .IsRequired(true);
            modelBuilder.Entity<Schadule>()
                        .HasMany(p => p.Appointments)
                        .WithOne(p => p.schadule)
                        .HasForeignKey(p => p.schaduleId)
                        .IsRequired(true);
        }
    }
}