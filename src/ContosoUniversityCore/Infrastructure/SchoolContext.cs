namespace ContosoUniversityCore.Infrastructure
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Domain;

    public class SchoolContext : DbContext
    {
        private DbContextTransaction _currentTransaction;


        public SchoolContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<CourseInstructor>().HasKey(ci => new { ci.CourseID, ci.InstructorID });

            modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CloseTransaction()
        {
            CloseTransaction(exception: null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    _currentTransaction.Rollback();
                    return;
                }

                SaveChanges();

                _currentTransaction?.Commit();
            }
            catch (Exception)
            {
                if (_currentTransaction?.UnderlyingTransaction.Connection != null)
                {
                    _currentTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }
}