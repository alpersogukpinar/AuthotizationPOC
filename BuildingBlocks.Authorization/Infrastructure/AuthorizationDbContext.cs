using Microsoft.EntityFrameworkCore;
using System;

namespace BuildingBlocks.Authorization.Infrastructure
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options) { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ActionEntity> Actions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Workgroup> Workgroups { get; set; }
        public DbSet<UserWorkgroup> UserWorkgroups { get; set; }
        public DbSet<PermissionAssignment> PermissionAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasIndex(a => a.Code)
                .IsUnique();

            modelBuilder.Entity<Resource>()
                .HasIndex(r => new { r.ApplicationId, r.Name })
                .IsUnique();
            modelBuilder.Entity<Resource>()
                .HasOne<Application>()
                .WithMany()
                .HasForeignKey(r => r.ApplicationId);

            modelBuilder.Entity<ActionEntity>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.Code)
                .IsUnique();
            modelBuilder.Entity<Permission>()
                .HasOne<Resource>()
                .WithMany()
                .HasForeignKey(p => p.ResourceId);
            modelBuilder.Entity<Permission>()
                .HasOne<ActionEntity>()
                .WithMany()
                .HasForeignKey(p => p.ActionId);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Workgroup>()
                .HasOne<Workgroup>()
                .WithMany()
                .HasForeignKey(wg => wg.ParentId)
                .IsRequired(false);

            modelBuilder.Entity<UserWorkgroup>()
                .HasKey(uwg => new { uwg.UserId, uwg.WorkgroupId });
            modelBuilder.Entity<UserWorkgroup>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(uwg => uwg.UserId);
            modelBuilder.Entity<UserWorkgroup>()
                .HasOne<Workgroup>()
                .WithMany()
                .HasForeignKey(uwg => uwg.WorkgroupId);

            modelBuilder.Entity<PermissionAssignment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(pa => pa.UserId)
                .IsRequired(false);
            modelBuilder.Entity<PermissionAssignment>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(pa => pa.RoleId)
                .IsRequired(false);
            modelBuilder.Entity<PermissionAssignment>()
                .HasOne<Workgroup>()
                .WithMany()
                .HasForeignKey(pa => pa.WorkgroupId)
                .IsRequired(false);
            modelBuilder.Entity<PermissionAssignment>()
                .HasOne<Permission>()
                .WithMany()
                .HasForeignKey(pa => pa.PermissionId);
        }
    }

    public class Application
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class Resource
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class ActionEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class Permission
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public Guid ActionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Department { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class Workgroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class UserWorkgroup
    {
        public Guid UserId { get; set; }
        public Guid WorkgroupId { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class PermissionAssignment
    {
        public Guid Id { get; set; }
        public string SubjectType { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? WorkgroupId { get; set; }
        public Guid PermissionId { get; set; }
        public DateTime SystemDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}