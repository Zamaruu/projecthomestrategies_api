using HomeStrategiesApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStrategiesApi.Helper
{
    public class NotificationHelper
    {
        public Notification Notification { get; set; }
        private readonly HomeStrategiesContext _context;
        private readonly INotificationService _notificationService;

        public NotificationHelper(Notification notification, HomeStrategiesContext context, INotificationService notificationService)
        {
            Notification = notification;
            _context = context;
            _notificationService = notificationService; 
        }

        public async Task<Notification> CreateNotification()
        {
            _context.Notifications.Add(Notification);
            await _context.SaveChangesAsync();
            return Notification;
        }

        public async void CreateNotificationForHousehold(int householdId, int excludeCreatorId)
        {
            var household = await _context.Households
                                            .Include(h => h.HouseholdMember)
                                            .Where(h => h.HouseholdId.Equals(householdId))
                                            .FirstOrDefaultAsync();

            foreach (User user in household.HouseholdMember)
            {
                if (user.UserId != excludeCreatorId)
                {
                    Notification.User = user;
                    _context.Notifications.Add(Notification);
                    CreateFcmNotification(user.FcmToken);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async void CreateFcmNotification(string devideId)
        {
            if(devideId == null)
            {
                return;
            }

            var fcmNotification = new FirebaseNotification
            {
                  DeviceId = devideId,
                  IsAndroiodDevice = true,
                  Title = "Neue Benachrichtigung",
                  Body = Notification.Content,
            };

            var result = await _notificationService.SendNotification(fcmNotification);
        }
    }
}
