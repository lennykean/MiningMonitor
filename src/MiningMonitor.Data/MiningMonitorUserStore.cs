using System.Linq;

using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;

using LiteDB;

using Microsoft.AspNetCore.Identity;

namespace MiningMonitor.Data
{
    public class QueryableLiteDbUserStore<TUser> : LiteDbUserStore<TUser>, IQueryableUserStore<TUser> where TUser : ApplicationUser, new()
    {
        private readonly LiteCollection<TUser> _users;

        public QueryableLiteDbUserStore(LiteDbContext dbContext) : base(dbContext)
        {
            _users = dbContext.LiteDatabase.GetCollection<TUser>("users");
        }

        #region IQueryableUserStore

        public IQueryable<TUser> Users => _users.FindAll().AsQueryable();

        #endregion
    }
}