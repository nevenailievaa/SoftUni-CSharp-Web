namespace SeminarHub.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SeminarHub.Data;
    using SeminarHub.Data.Models;
    using SeminarHub.Models.Category;
    using SeminarHub.Models.Seminar;
    using System.Globalization;
    using System.Security.Claims;
    using static SeminarHub.Data.Common.Constants;

    [Authorize]
    public class SeminarController : Controller
    {
        private SeminarHubDbContext dbContext;

        public SeminarController(SeminarHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            //Getting all of the Seminars from the database
            var allSeminars = await dbContext.Seminars
                .AsNoTracking()
                .Select(s => new SeminarAllViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    DateAndTime = s.DateAndTime.ToString(defaultDateTimeFormat),
                    Organizer = s.Organizer.UserName
                })
                .ToListAsync();

            //Returning all of the Seminars to the view
            return View(allSeminars);
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            //Getting the current User's Id
            var currentUserId = GetUserById();

            //Getting all the Seminars that the current User has joined (Subscribed to)
            var currentUsersSeminars = await dbContext.SeminarsParticipants
                .AsNoTracking()
                .Where(sp => sp.ParticipantId == currentUserId)
                .Select(sp => new SeminarJoinedViewModel()
                {
                    Id = sp.Seminar.Id,
                    Topic = sp.Seminar.Topic,
                    Lecturer = sp.Seminar.Lecturer,
                    Category = sp.Seminar.Category.Name,
                    DateAndTime = sp.Seminar.DateAndTime.ToString(defaultDateTimeFormat),
                    Organizer = sp.Seminar.Organizer.UserName
                })
                .ToListAsync();

            //Returning the joined Seminars to the view
            return View(currentUsersSeminars);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            //Getting the current User's Id
            var currentUserId = GetUserById();

            //Checking if there is not a Seminar with that Id
            if (!await dbContext.Seminars
                .AsNoTracking()
                .AnyAsync(e => e.Id == id))
            {
                return BadRequest();
            }

            //Checking if the current User already joined this Seminar
            if (await dbContext.SeminarsParticipants
                .AsNoTracking()
                .AnyAsync(ep => ep.SeminarId == id && ep.ParticipantId == currentUserId))
            {
                return RedirectToAction(nameof(All));
            }

            //Joining the Seminar
            var seminarParticipant = new SeminarParticipant()
            {
                SeminarId = id,
                ParticipantId = currentUserId
            };

            //Saving the changes and redirecting the User
            await dbContext.SeminarsParticipants.AddAsync(seminarParticipant);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            //Getting the current User's Id
            var currentUserId = GetUserById();

            //Checking if there is not a Seminar with that Id
            if (!await dbContext.Seminars
                .AsNoTracking()
                .AnyAsync(e => e.Id == id))
            {
                return BadRequest();
            }

            var seminarParticipant = await dbContext.SeminarsParticipants
               .Where(ep => ep.SeminarId == id && ep.ParticipantId == currentUserId)
               .FirstOrDefaultAsync();

            //Checking if the User hasn't joined that Seminar
            if (seminarParticipant == null)
            {
                return BadRequest();
            }

            //Removing the User from the current Seminar's Participants and redirecting him
            dbContext.SeminarsParticipants.Remove(seminarParticipant);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //Passing the Seminar Form
            var seminarForm = new SeminarAddViewModel();
            seminarForm.Categories = await GetCategories();
            return View(seminarForm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarAddViewModel seminarForm)
        {
            //Validation of the Seminar that the User wants to add
            //Null Validation
            if (seminarForm == null)
            {
                return BadRequest();
            }

            //Date Validation
            DateTime dateTime;

            if (!DateTime.TryParseExact(seminarForm.DateAndTime, defaultDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                ModelState.AddModelError(nameof(seminarForm.DateAndTime), $"Invalid date! Format must be {defaultDateTimeFormat}");
            }

            //Returning the User to the same page if some of the edited fields are not valid
            if (!ModelState.IsValid)
            {
                seminarForm.Categories = await GetCategories();
                return View(seminarForm);
            }

            //Adding the new Seminar
            Seminar newSeminar = new Seminar()
            {
                Topic = seminarForm.Topic,
                Lecturer = seminarForm.Lecturer,
                Details = seminarForm.Details,
                DateAndTime = dateTime,
                Duration = seminarForm.Duration,
                CategoryId = seminarForm.CategoryId,
                OrganizerId = GetUserById()
            };

            //Saving the changes to the database and redirecting the User to the "All Seminars" page
            await dbContext.Seminars.AddAsync(newSeminar);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //Searching the Seminar that the User wants to edit
            var searchedSeminar = await dbContext.Seminars
                .AsNoTracking()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            //Seminar is not found
            if (searchedSeminar == null)
            {
                return BadRequest();
            }

            //If the User tries to edit a Seminar which is not created by him
            if (searchedSeminar.OrganizerId != GetUserById())
            {
                return Unauthorized();
            }

            //Filling the edit form and returning it to the view
            var seminarToEdit = await dbContext.Seminars
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new SeminarEditViewModel()
                {
                    Id = e.Id,
                    Topic = e.Topic,
                    Lecturer = e.Lecturer,
                    Details = e.Details,
                    DateAndTime = e.DateAndTime.ToString(defaultDateTimeFormat),
                    Duration = e.Duration,
                    CategoryId = e.CategoryId
                })
                .FirstOrDefaultAsync();

            //Null Validation
            if (seminarToEdit == null)
            {
                return BadRequest();
            }

            seminarToEdit.Categories = await GetCategories();
            return View(seminarToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarEditViewModel seminarForm)
        {
            //Validation of the Seminar that the User wants to edit
            //Null Validation
            if (seminarForm == null)
            {
                return BadRequest();
            }

            //Date Validation
            DateTime dateTime;

            if (!DateTime.TryParseExact(seminarForm.DateAndTime, defaultDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                ModelState.AddModelError(nameof(seminarForm.DateAndTime), $"Invalid date! Format must be {defaultDateTimeFormat}");
            }

            //Returning the User to the same page if some of the edited fields are not valid
            if (!ModelState.IsValid)
            {
                seminarForm.Categories = await GetCategories();
                return View(seminarForm);
            }

            //Editing the Seminar, saving the changes to the database and redirecting the User
            var editEvent = await dbContext.Seminars
                .Where(e => e.Id == seminarForm.Id)
                .FirstOrDefaultAsync();

            //Null Validation
            if (editEvent == null)
            {
                return BadRequest();
            }

            editEvent.Topic = seminarForm.Topic;
            editEvent.Lecturer = seminarForm.Lecturer;
            editEvent.Details = seminarForm.Details;
            editEvent.DateAndTime = dateTime;
            editEvent.Duration = seminarForm.Duration;
            editEvent.CategoryId = seminarForm.CategoryId;

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //Searching the Seminar that the User wants to see
            var searchedSeminar = await dbContext.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SeminarDetailsViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    DateAndTime = s.DateAndTime.ToString(defaultDateTimeFormat),
                    Duration = s.Duration,
                    Lecturer = s.Lecturer,
                    Category = s.Category.Name,
                    Details = s.Details,
                    Organizer = s.Organizer.UserName
                })
                .FirstOrDefaultAsync();

            //Checking if the Seminar exists
            if (searchedSeminar == null)
            {
                return BadRequest();
            }

            //Returning the view of the Seminar
            return View(searchedSeminar);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //Getting the current User's Id
            var currentUserId = GetUserById();

            //Finding the Seminar
            var searchedSeminar = await dbContext.Seminars
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            //Checking if there is not a Seminar with that Id
            if (searchedSeminar == null)
            {
                return BadRequest();
            }

            //Checking if the User that wants to Delete the Event is not it's creator
            if (searchedSeminar.OrganizerId != GetUserById())
            {
                return Unauthorized();
            }

            var seminar = new SeminarDeleteViewModel()
            {
                Id = id,
                Topic = searchedSeminar.Topic,
                DateAndTime = searchedSeminar.DateAndTime
            };

            //Returning the view of the Seminar
            return View(seminar);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Finding the Seminar and it's Participants
            var currentSeminar = await dbContext.Seminars
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            var seminarParticipants = await dbContext.SeminarsParticipants
                .Where(sp => sp.SeminarId == id)
                .ToListAsync();

            //Checking if the Seminar exists
            if (currentSeminar == null)
            {
                return BadRequest();
            }

            //Checking if the User that wants to Delete the Seminar is not it's creator
            if (currentSeminar.OrganizerId != GetUserById())
            {
                return Unauthorized();
            }

            //Checking if there are any participants of this Seminar and removing them
            if (seminarParticipants != null && seminarParticipants.Any())
            {
                dbContext.SeminarsParticipants.RemoveRange(seminarParticipants);
            }

            //Removing the Seminar and redirecting the User
            dbContext.Seminars.Remove(currentSeminar);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
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

        //Getting the Categories of the Seminars
        private async Task<ICollection<CategoryViewModel>> GetCategories()
        {
            return await dbContext.Categories
                .Select(t => new CategoryViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }
    }
}