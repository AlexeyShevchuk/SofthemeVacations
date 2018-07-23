using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Vacations.Model.Models
{
    public partial class VacationsDBContext : DbContext
    {
        public VacationsDBContext()
        {
        }

        public VacationsDBContext(DbContextOptions<VacationsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeStatus> EmployeeStatus { get; set; }
        public virtual DbSet<JobTitle> JobTitle { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vacation> Vacation { get; set; }
        public virtual DbSet<VacationStatus> VacationStatus { get; set; }

        // Unable to generate entity type for table 'dbo.EmployeeTeam'. Please see the warning messages.
        //TODO: if (!optionsBuilder.IsConfigured)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EmployeeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.EmployeeStatusId).HasColumnName("EmployeeStatusID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.JobTitleId).HasColumnName("JobTitleID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Skype).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.TelephoneNumber).HasMaxLength(20);

                entity.Property(e => e.WorkEmail).HasMaxLength(256);

                entity.HasOne(d => d.EmployeeStatus)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.EmployeeStatusId)
                    .HasConstraintName("Employee_EmployeeStatusID_FK");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.JobTitleId)
                    .HasConstraintName("Employee_JobTitleID_FK");
            });

            modelBuilder.Entity<EmployeeStatus>(entity =>
            {
                entity.Property(e => e.EmployeeStatusId)
                    .HasColumnName("EmployeeStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.Property(e => e.JobTitleId)
                    .HasColumnName("JobTitleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.TeamId)
                    .HasColumnName("TeamID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.TeamLeadId).HasColumnName("TeamLeadID");

                entity.HasOne(d => d.TeamLead)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.TeamLeadId)
                    .HasConstraintName("Team_TeamLeadID_FK");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .HasColumnName("TransactionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

                entity.Property(e => e.Сomment).HasMaxLength(200);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("Transaction_EmployeeID_FK");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .HasConstraintName("Transaction_TransactionTypeID_FK");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(e => e.TransactionTypeId)
                    .HasColumnName("TransactionTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId)
                    .HasName("UQ__User__7AD04FF06F4F1F30")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Password).HasMaxLength(300);

                entity.Property(e => e.PersonalEmail)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.EmployeeId)
                    .HasConstraintName("User_EmployeeID_FK");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("User_RoleID_FK");
            });

            modelBuilder.Entity<Vacation>(entity =>
            {
                entity.Property(e => e.VacationId)
                    .HasColumnName("VacationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndVocationDate).HasColumnType("date");

                entity.Property(e => e.StartVocationDate).HasColumnType("date");

                entity.Property(e => e.VacationStatusId).HasColumnName("VacationStatusID");

                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Vacation)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("Vacation_EmployeeID_FK");

                entity.HasOne(d => d.VacationStatus)
                    .WithMany(p => p.Vacation)
                    .HasForeignKey(d => d.VacationStatusId)
                    .HasConstraintName("Vacation_VacationStatusID_FK");
            });

            modelBuilder.Entity<VacationStatus>(entity =>
            {
                entity.Property(e => e.VacationStatusId)
                    .HasColumnName("VacationStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }
    }
}
