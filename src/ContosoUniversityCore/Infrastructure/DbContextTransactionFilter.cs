namespace ContosoUniversityCore.Infrastructure
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class DbContextTransactionFilter : IActionFilter
    {
        private readonly SchoolContext _dbContext;

        public DbContextTransactionFilter(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _dbContext.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _dbContext.CloseTransaction(context.Exception);
                return;
            }

            try
            {
                _dbContext.CloseTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.CloseTransaction(ex);

                throw;
            }
        }
    }
}