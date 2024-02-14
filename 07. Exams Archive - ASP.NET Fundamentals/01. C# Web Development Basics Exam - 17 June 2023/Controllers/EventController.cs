namespace Homies.Controllers
{
    using Homies.Data;
    using Homies.Data.Models;
    using Homies.Models.Event;
    using Homies.Models.Type;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Claims;
    using static Homies.Data.Common.Constants;

    [Authorize]
    public class EventController : Controller
    {
        private HomiesDbContext dbContext;

        public EventController(HomiesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Button for "Events Around Me" page that shows all the events
        [HttpGet]
        public async Task<IActionResult> All()
        {
            //Getting all of the events from the database
            var allEvents = await dbContext.Events
                .AsNoTracking()
                .Select(e => new EventAllViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(dateTimeFormat),
                    Organiser = e.Organiser.UserName,
                    Type = e.Type.Name
                }).ToListAsync();

            //Returning all of the events to the view
            return View(allEvents);
        }


        //Buttons for "Create a New Event" page that allows the User to create a new Event
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //Passing the Add Form
            var eventForm = new EventAddViewModel();
            eventForm.Types = await GetTypes();
            return View(eventForm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventAddViewModel eventForm)
        {
            //Validation of the Event that the User wants to add
            //Null Validation
            if (eventForm == null)
            {
                return BadRequest();
            }

            //Dates Validation
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(eventForm.Start, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                ModelState.AddModelError(nameof(eventForm.Start), $"Invalid date! Format must be {dateTimeFormat}");
            }
            if (!DateTime.TryParseExact(eventForm.End, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                ModelState.AddModelError(nameof(eventForm.End), $"Invalid date! Format must be {dateTimeFormat}");
            }

            //Returning the User to the same page if some of the edited fields are not valid
            if (!ModelState.IsValid)
            {
                //This is used to show the error messages of the ModelState if there are any
                List<string> errorMessages = new List<string>();
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }

                eventForm.Types = await GetTypes();
                return View(eventForm);
            }

            //Adding the new Event
            Event newEvent = new Event()
            {
                Name = eventForm.Name,
                Description = eventForm.Description,
                Start = startDate,
                End = endDate,
                TypeId = eventForm.TypeId,
                CreatedOn = DateTime.Now,
                OrganiserId = GetUserById()
            };

            //Saving the changes to the database and redirecting the User to the "Events Around Me" page
            await dbContext.AddAsync(newEvent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        //"Edit" button that allows the User to edit an event only if he created it
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //Searching the event that the User wants to edit
            var searchedEvent = await dbContext.Events
                .AsNoTracking()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            //Event is not found
            if (searchedEvent == null)
            {
                return BadRequest();
            }

            //If the User tries to edit an Event, which is not his
            if (searchedEvent.OrganiserId != GetUserById())
            {
                return Unauthorized();
            }

            //Filling the edit form and returning it to the view
            var eventToEdit = await dbContext.Events
                .Where(e => e.Id == id)
                .Select(e => new EventEditViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description=e.Description,
                    Start = e.Start.ToString(dateTimeFormat),
                    End = e.End.ToString(dateTimeFormat),
                    TypeId = e.TypeId
                })
                .FirstOrDefaultAsync();

            //Null Validation
            if (eventToEdit == null)
            {
                return BadRequest();
            }

            eventToEdit.Types = await GetTypes();
            return View(eventToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventEditViewModel eventForm)
        {
            //Validation of the Event that the User wants to edit
            //Null Validation
            if (eventForm == null)
            {
                return BadRequest();
            }

            //Dates Validation
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(eventForm.Start, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                ModelState.AddModelError(nameof(eventForm.Start), $"Invalid date! Format must be {dateTimeFormat}!");
            }
            if (!DateTime.TryParseExact(eventForm.End, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                ModelState.AddModelError(nameof(eventForm.End), $"Invalid date! Format must be {dateTimeFormat}!");
            }

            //Returning the User to the same page if some of the edited fields are not valid
            if (!ModelState.IsValid)
            {
                //This is used to show the error messages of the ModelState
                List<string> errorMessages = new List<string>();
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }

                eventForm.Types = await GetTypes();
                return View(eventForm);
            }

            //Editing the Event, saving the changes to the database and redirecting the User
            var editEvent = await dbContext.Events
                .Where(e => e.Id == eventForm.Id)
                .FirstOrDefaultAsync();

            //Null Validation
            if (editEvent == null)
            {
                return BadRequest();
            }

            editEvent.Name = eventForm.Name;
            editEvent.Description = eventForm.Description;
            editEvent.Start = startDate;
            editEvent.End = endDate;

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }


        //"Join" button that allows the User to join an event if he didn't already join it
        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            //Getting the current User's Id
            var currentUser = GetUserById();

            //Checking if there is not an Event with that Id
            if (!await dbContext.Events
                .AsNoTracking()
                .AnyAsync(e => e.Id == id))
            {
                return BadRequest();
            }

            //Checking if the current User already joined this Event
            if (await dbContext.EventsParticipants
                .AsNoTracking()
                .AnyAsync(ep => ep.EventId == id && ep.HelperId == currentUser))
            {
                return RedirectToAction(nameof(All));
            }

            //Joining the Event
            var userEvent = new EventParticipant()
            {
                EventId = id,
                HelperId = currentUser
            };

            //Saving the changes and redirecting the User
            await dbContext.EventsParticipants.AddAsync(userEvent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        //Button for "My Joined Events" page that shows all the events that the current User has joined
        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            //Getting the current User's Id
            var currentUserId = GetUserById();

            //Getting all the Events that the current User has joined
            var currentUserEvents = await dbContext.EventsParticipants
                .Where(ep => ep.HelperId == currentUserId)
                .Select(ep => new EventJoinedViewModel()
                {
                    Id = ep.EventId,
                    Name = ep.Event.Name,
                    Start = ep.Event.Start.ToString(dateTimeFormat),
                    Type = ep.Event.Type.Name
                })
                .ToListAsync();

            //Returning the joined Events to the view
            return View(currentUserEvents);
        }


        //"Leave the Event" button that allows the User to Leave an event if he joined it
        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            //Getting the current User's Id
            var currentUser = GetUserById();

            //Checking if there is not an Event with that Id
            if (!await dbContext.Events
                .AsNoTracking()
                .AnyAsync(e => e.Id == id))
            {
                return BadRequest();
            }

            var userEvent = await dbContext.EventsParticipants
                .Where(ep => ep.EventId == id && ep.HelperId == currentUser)
                .FirstOrDefaultAsync();

            //Checking if the user hasn't joined that event
            if (userEvent == null)
            {
                return BadRequest();
            }

            //Removing the User from the current Event's participants and redirecting him
            dbContext.EventsParticipants.Remove(userEvent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        //"View Details" button that deletes an Event by it's creator
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //Finding the Event and it's Participants
            var currentEvent = await dbContext.Events
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            var eventParticipants = await dbContext.EventsParticipants
                .Where(ep => ep.EventId == id)
                .ToListAsync();

            //Checking if the Event exists
            if (currentEvent == null)
            {
                return BadRequest();
            }
            //Checking if the User that wants to Delete the Event is not it's creator
            if (currentEvent.OrganiserId != GetUserById())
            {
                return Unauthorized();
            }
            //Checking if there are any participants of this event and removing them
            if (eventParticipants != null && eventParticipants.Any())
            {
                dbContext.EventsParticipants.RemoveRange(eventParticipants);
            }

            //Removing the Event and redirecting the User
            dbContext.Events.Remove(currentEvent);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        //"View Details" button that shows an Event's details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //Searching the Event that the User wants to see
            var searchedEvent = await dbContext.Events
                .Where(e => e.Id == id)
                .Select(e => new EventDetailsViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Start = e.Start.ToString(dateTimeFormat),
                    End = e.End.ToString(dateTimeFormat),
                    Organiser = e.Organiser.UserName,
                    CreatedOn = e.CreatedOn.ToString(dateTimeFormat),
                    Type = e.Type.Name
                })
                .FirstOrDefaultAsync();

            //Checking if the Event exists
            if (searchedEvent == null)
            {
                return BadRequest();
            }

            //Returning the view of the Event
            return View(searchedEvent);
        }

        //Getting the current User's Id
        private string GetUserById()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }

        //Getting the Types of Events
        private async Task<ICollection<TypeViewModel>> GetTypes()
        {
            return await dbContext.Types
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }
    }
}