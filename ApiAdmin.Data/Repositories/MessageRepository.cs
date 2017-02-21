using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Interfaces;

namespace ApiAdmin.Data.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(DataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
