using System;
using System.Linq;
using Challenge.Dtos;
using Microsoft.AspNetCore.Mvc;
using Challenge.Repositories;
using System.Collections.Generic;
using Challenge.Models;


namespace Challenge.Controllers
{
    // Annotation to declare the class as Api controller
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IInMemoryReports repository;

        // Apply Dependency Injection
        public ReportController(IInMemoryReports repository)
        {
            this.repository = repository;
        }


        // Define Routes
        // GET reports/
        [HttpGet]
        public IEnumerable<ReportDto> GetReports()
        {
            var reports = repository.GetReports().Select(report => report.AsDto());

            return reports;
        }


        // GET reports/{id}
        [HttpGet(":id")]
        // Return ActionResult because it allows us to return Either a Report
        // Or a Not Found status
        public ActionResult<ReportDto> GetReport(Guid Id)
        {
            var report = repository.GetReport(Id);

            if (report == null)
            {
                return NotFound();
            }

            return report.AsDto();
        }

        // POST reports/
        [HttpPost]
        public ActionResult<ReportDto> LogReport(CreateReportDto reportDto)
        {
            //Create new Report
            Report report = new()
            {
                Id = Guid.NewGuid(),
                Name = reportDto.Name,
                Type = reportDto.Type,
                CreatedDate = DateTimeOffset.UtcNow
            };

            //Save report in the repository/Database
            repository.CreateReport(report);

            //Return Report created
            return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report.AsDto());
        }
    }
}