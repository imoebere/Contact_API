using ContactAPI.Data;
using ContactAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ContactAPI.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class ContactController : Controller
	{
		private readonly ContactDbContext dbContext;

		public ContactController(ContactDbContext dbContext)
        {
			this.dbContext = dbContext;
		}
       
		[HttpGet]
		public async Task<IActionResult> GetAllContact()
		{
			return Ok(await dbContext.contacts.ToListAsync());
		}
		
		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetContact([FromRoute] Guid id)
		{
			var contact = await dbContext.contacts.FindAsync(id);
			if (contact == null)
			{
				return NotFound();
			}
			return Ok(contact);
		}
		
		[HttpPost]
		public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
		{
			var contact = new Contact()
			{
				Id = Guid.NewGuid(),
				FullName = addContactRequest.FullName,
				Email = addContactRequest.Email,
				Phone = addContactRequest.Phone,
				Address = addContactRequest.Address
			};
			await dbContext.contacts.AddAsync(contact);
			await dbContext.SaveChangesAsync();
			return Ok(contact);
		}
		
		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
		{
			var contact = await dbContext.contacts.FindAsync(id);
			if(contact != null)
			{
				contact.FullName = updateContactRequest.FullName;
				contact.Email = updateContactRequest.Email;
				contact.Phone = updateContactRequest.Phone;
				contact.Address = updateContactRequest.Address;
				await dbContext.SaveChangesAsync();
				return Ok(contact);
			}
			return NotFound();
		}
		
		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
		{
			var contact = await dbContext.contacts.FindAsync(id);
			if (contact != null)
			{
				dbContext.contacts.Remove(contact);
				if (await dbContext.SaveChangesAsync() > 0)
					return Ok(contact);
			}
			return NotFound();
		}
	}
}
