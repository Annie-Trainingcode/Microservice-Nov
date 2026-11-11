using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleService.DTOs;
using VehicleService.Models;
using VehicleService.Repositories;
using VehicleService.Services;

namespace VehicleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _repository;
        //private readonly ICacheService _cacheService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(
            IVehicleRepository repository,
          //  ICacheService cacheService,
            ILogger<VehiclesController> logger)
        {
            _repository = repository;
         //   _cacheService = cacheService;
            _logger = logger;
        }
         [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAllVehicles()
        {
        }
           [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle(Guid id)
        {
          
        }
         [HttpPost]
        public async Task<ActionResult<VehicleDto>> CreateVehicle(CreateVehicleDto createDto)
        {
             try
            {
                var vehicle = new Vehicle
                {
                    LicensePlate = createDto.LicensePlate,
                    Model = createDto.Model,
                    Status = createDto.Status,
                    Latitude = createDto.Latitude,
                    Longitude = createDto.Longitude,
                    LastKnownLocation = createDto.LastKnownLocation,
                    CurrentSpeed = createDto.CurrentSpeed,
                    FuelLevel = createDto.FuelLevel
                };

                var createdVehicle = await _repository.CreateVehicleAsync(vehicle);
                var vehicleDto = MapToDto(createdVehicle);

                // Invalidate cache
               // await _cacheService.RemoveDataAsync("vehicles:all");

                return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicle.VehicleID }, vehicleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vehicle");
                return StatusCode(500, "An error occurred while creating the vehicle");
            }
        }

            
        
          [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(Guid id, UpdateVehicleDto updateDto)
        {
        }
         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
        }
        
        [HttpPost("{id}/location")]
        public async Task<IActionResult> UpdateVehicleLocation(Guid id, VehicleLocationUpdateDto locationUpdate)
        {
        }
          private VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                VehicleID = vehicle.VehicleID,
                LicensePlate = vehicle.LicensePlate,
                Model = vehicle.Model,
                Status = vehicle.Status,
                Latitude = vehicle.Latitude,
                Longitude = vehicle.Longitude,
                LastKnownLocation = vehicle.LastKnownLocation,
                CurrentSpeed = vehicle.CurrentSpeed,
                FuelLevel = vehicle.FuelLevel,
                LastUpdated = vehicle.LastUpdated
            };
        }

    }
}