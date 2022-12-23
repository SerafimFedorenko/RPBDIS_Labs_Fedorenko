using WebApp.Data;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class AcceptedRecyclablesController : ControllerBase
    {
        private readonly RecPointContext _context;
        public AcceptedRecyclablesController(RecPointContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        public List<AcceptedRecyclableViewModel> Get()
        {
            IQueryable <AcceptedRecyclable> acceptedRecyclables = 
                _context.AcceptedRecyclables.Include(a => a.Employee).Include(a => a.Storage).Include(a => a.RecyclableType);
            IQueryable<AcceptedRecyclableViewModel> accRecs = acceptedRecyclables.Select(a => new AcceptedRecyclableViewModel
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                StorageId = a.StorageId,
                RecyclableTypeId = a.RecyclableTypeId,
                EmployeeFIO = a.Employee.Surname + " " + a.Employee.Name + " " + a.Employee.Patronymic,
                StorageName = a.Storage.Name,
                RecyclableTypeName = a.RecyclableType.Name,
                Date = a.Date,
                Quantity = a.Quantity
            });
            return accRecs.ToList();
        }

        [HttpGet("FilteredAcceptedRecyclables")]
        [Produces("application/json")]
        public List<AcceptedRecyclableViewModel> GetFilteredAcceptedRecyclables(int employeeId, int storageId, int recyclableTypeId)
        {
            IQueryable<AcceptedRecyclable> acceptedRecyclables =
                _context.AcceptedRecyclables.Include(a => a.Employee).Include(a => a.Storage).Include(a => a.RecyclableType);
            IQueryable<AcceptedRecyclableViewModel> accRecs = acceptedRecyclables.Select(a => new AcceptedRecyclableViewModel
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                StorageId = a.StorageId,
                RecyclableTypeId = a.RecyclableTypeId,
                EmployeeFIO = a.Employee.Surname + " " + a.Employee.Name + " " + a.Employee.Patronymic,
                StorageName = a.Storage.Name,
                RecyclableTypeName = a.RecyclableType.Name,
                Date = a.Date,
                Quantity = a.Quantity
            });
            if (employeeId > 0)
            {
                accRecs = accRecs.Where(a => a.EmployeeId == employeeId);

            }
            if (storageId > 0)
            {
                accRecs = accRecs.Where(a => a.StorageId == storageId);

            }
            if (recyclableTypeId > 0)
            {
                accRecs = accRecs.Where(a => a.RecyclableTypeId == recyclableTypeId);

            }
            return accRecs.ToList();
        }

        [HttpGet("storages")]
        [Produces("application/json")]
        public IEnumerable<Storage> GetStorages()
        {
            return _context.Storages.ToList();
        }

        [HttpGet("employees")]
        [Produces("application/json")]
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("recyclableTypes")]
        [Produces("application/json")]
        public IEnumerable<RecyclableType> GetRecyclableTypes()
        {
            return _context.RecyclableTypes.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            AcceptedRecyclable acceptedRecyclable = _context.AcceptedRecyclables.FirstOrDefault(x => x.Id == id);
            if (acceptedRecyclable == null)
                return NotFound();
            return new ObjectResult(acceptedRecyclable);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AcceptedRecyclable acceptedRecyclable)
        {
            if (acceptedRecyclable == null)
            {
                return BadRequest();
            }

            _context.AcceptedRecyclables.Add(acceptedRecyclable);
            _context.SaveChanges();
            return Ok(acceptedRecyclable);
        }

        [HttpPut]
        public IActionResult Put([FromBody] AcceptedRecyclable acceptedRecyclable)
        {
            if (acceptedRecyclable == null)
            {
                return BadRequest();
            }
            if (!_context.AcceptedRecyclables.Any(x => x.Id == acceptedRecyclable.Id))
            {
                return NotFound();
            }

            _context.Update(acceptedRecyclable);
            _context.SaveChanges();
            return Ok(acceptedRecyclable);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            AcceptedRecyclable acceptedRecyclable = _context.AcceptedRecyclables.FirstOrDefault(x => x.Id == id);
            if (acceptedRecyclable == null)
            {
                return NotFound();
            }
            _context.AcceptedRecyclables.Remove(acceptedRecyclable);
            _context.SaveChanges();
            return Ok(acceptedRecyclable);
        }
    }
}
