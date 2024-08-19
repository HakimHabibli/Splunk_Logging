using LoggingExample.Data;
using LoggingExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoggingExample.Controllers;

public class EmployeeController : Controller
{
    private readonly AppDbContext _context;
    private readonly FileLogger _fileLogger;
    private readonly SplunkLogger _splunkLogger;

    public EmployeeController(SplunkLogger splunkLogger, FileLogger fileLogger, AppDbContext context)
    {
        _splunkLogger = splunkLogger;
        _fileLogger = fileLogger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _context.Employees.ToListAsync();

        return View(employees);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        string logMessage = $"Employee created: {employee.FirstName} {employee.LastName}";
        await _fileLogger.LogEventAsync(logMessage, "INFO");
        await _splunkLogger.LogEventAsync(logMessage, "INFO");
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Edit() => View();

    [HttpPost]
    public async Task<IActionResult> Edit(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();

        string logMessage = $"Employee updated: {employee.FirstName} {employee.LastName}";
        await _fileLogger.LogEventAsync(logMessage, "INFO");
        await _splunkLogger.LogEventAsync(logMessage, "INFO");

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete() => View();

    // Delete
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            string logMessage = $"Employee deleted: {employee.FirstName} {employee.LastName}";
            await _fileLogger.LogEventAsync(logMessage, "INFO");
            await _splunkLogger.LogEventAsync(logMessage, "INFO");
        }

        return RedirectToAction(nameof(Index));
    }
}
