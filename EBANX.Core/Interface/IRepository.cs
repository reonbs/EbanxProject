using System;
using EBANX.Core.Models;

namespace EBANX.Core.Interface
{
    public interface IRepository
    {
        void Add(Account account);
        Account Get(string id);
        bool Exist(string id);
        void Update(Account account);
        void Clear();
    }
}
