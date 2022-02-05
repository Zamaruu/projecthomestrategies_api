using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeStrategiesApi.Helper;
using HomeStrategiesApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HomeStrategiesApi.Auth;

namespace HomeStrategiesApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly HomeStrategiesContext _context;
        private readonly INotificationService _notificationService;

        public NotificationsController(HomeStrategiesContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: api/Notifications
        [HttpGet]
        public IActionResult GetNotifications()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = new AuthenticationClaimsHelper(identity).GetIdClaimFromUser();

            if(userId > 0)
            {
                var openNotifications = _context.Notifications
                                            .Where(n => n.User.UserId.Equals(userId) && n.Seen == false)
                                            .ToList();

                var seenNotifications = _context.Notifications
                                            .Where(n => n.User.UserId.Equals(userId) && n.Seen == true)
                                            .ToList();

                var result = new Dictionary<string, List<Notification>>();
                result.Add("OpenNotfications", openNotifications);
                result.Add ("SeenNotifications", seenNotifications);

                return Ok(result);
            }
            else
            {
                return BadRequest("Benutzer ID ist nicht bekannt!");
            }
        }

        [HttpGet("Open")]
        public IActionResult GetOpenNotificationCount()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = new AuthenticationClaimsHelper(identity).GetIdClaimFromUser();

            var openNotifications = _context.Notifications
                                            .Where(n => n.User.UserId.Equals(userId) && n.Seen == false)
                                            .ToList();

            return Ok(openNotifications);
        }


        // PUT: api/Notifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("SetSeen/{id}")]
        public async Task<IActionResult> PutNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return BadRequest("Benachrichtigung konnte nicht gefunden werden!");
            }
            else
            {
                notification.Seen = true;
                _context.Entry(notification).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
                return Ok("Status erfolgreich geändert");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound("Benachrichtigung konnte nicht gefunden werden!");
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPut("SetAllSeen")]
        public async Task<IActionResult> PutAllOnSeen(List<int> notificationIds)
        {
            foreach (int id in notificationIds)
            {
                var notification = await _context.Notifications.FindAsync(id);

                if (notification == null)
                {
                    return BadRequest("Benachrichtigung konnte nicht gefunden werden!");
                }
                else
                {
                    notification.Seen = true;
                    _context.Entry(notification).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Status aller Benachrichtigungen erfolgreich geändert");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationsExist(notificationIds))
                {
                    return NotFound("Benachrichtigungen konnten nicht gefunden werden!");
                }
                else
                {
                    throw;
                }
            }
        }



        // POST: api/Notifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostNotification(Notification notification)
        {
            var newNotification = await new NotificationHelper(notification, _context, _notificationService).CreateNotification();
            return Ok(newNotification);
        }

        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }

        private bool NotificationsExist(List<int> ids)
        {
            foreach (int id in ids)
            {
                bool exists = _context.Notifications.Any(e => e.NotificationId == id);
                if (!exists)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
