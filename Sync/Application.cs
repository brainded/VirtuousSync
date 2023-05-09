using DAL;
using DAL.Repository;
using System.Threading.Tasks;
using Services;

namespace Sync
{
    public class Application
    {
        private readonly IVirtuousService mVirtuousService;
        private readonly IRepository<Contact> mContactRepository;
        public Application(IVirtuousService virtuousService, IRepository<Contact> contactRepo)
        {

            mVirtuousService = virtuousService;
            mContactRepository = contactRepo;
        }

        public async Task Sync()
        {
            var skip = 0;
            var take = 100;
            var maxContacts = 1000;
            var hasMore = true;

            do
            {
                var contacts = await mVirtuousService.GetContactsAsync(skip, take, "AZ");
                //var contacts = await mVirtuousService.GetContactsAsync(skip, take, string.Empty);
                skip += take;

                await mContactRepository.AddRangeAsync(contacts.List);
                hasMore = skip > maxContacts;
            }
            while (!hasMore);

            await mContactRepository.SaveChangesAsync();

        }
    }
}
