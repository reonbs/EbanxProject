using System;
using System.Collections.Generic;
using System.Linq;
using EBANX.Core.Interface;
using EBANX.Core.Models;

namespace EBANX.Core.Repository
{
    public class Repository : IRepository
    {
        public List<Account> _dbContext { get; set; }

        public Repository()
        {
            _dbContext = new List<Account>();
        }

        public void Add(Account account)
        {
            _dbContext.Add(account);
        }

        public Account Get(string id)
        {
            return _dbContext.FirstOrDefault(x => x.Id == id);
        }

        public bool Exist(string id)
        {
            return _dbContext.Any(x => x.Id == id);
        }

        public void Update(Account account)
        {
            var _account = _dbContext.FirstOrDefault(x => x.Id == account.Id);
            _account.Amount = account.Amount;
        }
    }
}
