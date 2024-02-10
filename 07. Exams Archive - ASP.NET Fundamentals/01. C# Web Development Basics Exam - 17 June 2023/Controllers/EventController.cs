namespace Homies.Controllers
{
    using Homies.Data;
    using Homies.Data.Common;
    using Homies.Data.Models;
    using Homies.Models.EventViewModels;
    using Homies.Models.TypeViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Security.Claims;
    using static Homies.Data.Common.Constraints.EventConstraints;

    [Authorize]
    public class EventController : Controller
    {
        private HomiesDbContext dbContext;

        public EventController(HomiesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Buttons for "Events Around Me" Page
        [HttpGet]
        public async Task<IActionResult> All()
        {
            //Taking All Events from the Database
            var allEvents = await dbContext.Events
                .AsNoTracking()
                .Select(e => new EventAllViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Organiser = e.Organiser.UserName,
                    Start = e.Start.ToString(Constraints.EventConstraints.defaultDateFormat),
                    Type = e.Type.Name
                }).ToListAsync();

            //Returning All of the Events to the View
            return View(allEvents);
        }


        [HttpPost] 
        public async Task<IActionResult> Join(int id)
        {
            //Finding the Current User
            string userId = GetUserById();

            //If the current event is already joined, the user should be redirected to Event/All or if there are any problems
            if (!ModelState.IsValid || dbContext.EventsParticipants.Any(ep => ep.EventId == id && ep.HelperId == userId))
            {
                return RedirectToAction(nameof(All));
            }

            //Else he should join successfully
            var eventParticipant = new EventParticipant()
            {
                EventId = id,
                HelperId = userId
            };

            await dbContext.EventsParticipants.AddAsync(eventParticipant);
            await dbContext.SaveChangesAsync();

            //Redirecting to All Events
            return RedirectToAction(nameof(Joined));
        }


        //Buttons for "My Joined Events" Page
        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            //Finding the Current User
            string userId = GetUserById();

            //Finding Current User's Joined Events
            var joinedEvents = await dbContext.EventsParticipants
                .Where(ep => ep.HelperId == userId)
                .Select(ep => new EventJoinedViewModel()
                {
                    Id = ep.EventId,
                    Name = ep.Event.Name,
                    Start = ep.Event.Start.ToString(Constraints.EventConstraints.defaultDateFormat),
                    Organiser = ep.Event.Organiser.UserName,
                    Type = ep.Event.Type.Name
                }).ToListAsync();

            //Returning the User's Joined Events to the View
            return View(joinedEvents);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            //Finding the Current User
            string userId = GetUserById();

            //Finding the Current Event
            var currentEvent = await dbContext.EventsParticipants.FirstOrDefaultAsync(ep => ep.EventId == id && ep.HelperId == userId);

            //Leaving the Current Event
            dbContext.EventsParticipants.Remove(currentEvent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        //"Create a New Event" Page
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var eventFormModel = new EventAddViewModel();
            eventFormModel.Types = await GetTypes();
            return View(eventFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventAddViewModel eventForm)
        {
            //Validation of the Event that the User wants to add
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(eventForm.Start, defaultDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                ModelState.AddModelError(nameof(eventForm.Start), $"Invalid date! Format must be {defaultDateFormat}");
            }
            if (!DateTime.TryParseExact(eventForm.End, defaultDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                ModelState.AddModelError(nameof(eventForm.End), $"Invalid date! Format must be {defaultDateFormat}");
            }

            if (!ModelState.IsValid)
            {
                eventForm.Types = await GetTypes();
                return View(eventForm);
            }

            //Adding the new Event
            var newEvent = new Event()
            {
                Name = eventForm.Name,
                Description = eventForm.Description,
                Start = startDate,
                End = endDate,
                TypeId = eventForm.TypeId,
                CreatedOn = DateTime.Now,
                OrganiserId = GetUserById()
            };

            await dbContext.Events.AddAsync(newEvent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        //Edit Button
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //Checking if the User tries to Edit an Event which is not his
            var searchedEvent = await dbContext.Events
                .AsNoTracking()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (!ModelState.IsValid || searchedEvent == null || searchedEvent.OrganiserId != GetUserById())
            {
                return BadRequest();
            }

            //Returning the Model
            var eventEditModel = await dbContext.Events
                .Where(e => e.Id == id)
                .Select(e => new EventEditViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Start = e.Start.ToString(defaultDateFormat),
                    End = e.End.ToString(defaultDateFormat),
                    TypeId = e.TypeId
                })
                .FirstOrDefaultAsync();

            eventEditModel.Types = await GetTypes();
            return View(eventEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventEditViewModel editForm)
        {
            //Validation of the Event that the User wants to Edit
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(editForm.Start, defaultDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                ModelState.AddModelError(nameof(editForm.Start), $"Invalid date! Format must be {defaultDateFormat}");
            }
            if (!DateTime.TryParseExact(editForm.End, defaultDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                ModelState.AddModelError(nameof(editForm.End), $"Invalid date! Format must be {defaultDateFormat}");
            }

            if (!ModelState.IsValid)
            {
                editForm.Types = await GetTypes();
                return View(editForm);
            }

            //Editing the Model
            Event editedEvent = await dbContext.Events.FindAsync(editForm.Id);

            editedEvent.Name = editForm.Name;
            editedEvent.Description = editForm.Description;
            editedEvent.Start = startDate;
            editedEvent.End = endDate;
            editedEvent.TypeId = editForm.TypeId;

            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        //This Method is a Bonus and works for both "Events Around Me" page and "My joined Events" page. It shows the details about an Event.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var eventToDisplay = await dbContext
               .Events
               .Where(e => e.Id == id)
               .Select(e => new EventDetailsViewModel
               {
                   Id = id,
                   Name = e.Name,
                   Description = e.Description,
                   Start = e.Start.ToString(defaultDateFormat),
                   CreatedOn = e.CreatedOn.ToString(defaultDateFormat),
                   End = e.End.ToString(defaultDateFormat),
                   Organiser = e.Organiser.UserName,
                   Type = e.Type.Name
               })
               .FirstOrDefaultAsync();

            return View(eventToDisplay);
        }

        //Private Methods
        private string GetUserById()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }

        private async Task<IEnumerable<TypeViewModel>> GetTypes()
        {
            return await dbContext.Types
                .AsNoTracking()
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToListAsync();
        }
    }
}