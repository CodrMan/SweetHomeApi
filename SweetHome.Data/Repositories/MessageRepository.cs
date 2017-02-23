using SweetHome.Core.Entities;
using SweetHome.Core.Interfaces;

namespace SweetHome.Data.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(DataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
