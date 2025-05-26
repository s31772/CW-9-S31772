using apbd_cw9.Data;
using apbd_cw9.DTOs;
using apbd_cw9.Exceptions;
using apbd_cw9.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw9.Services;

public interface IDbService
{
    Task<int> AddPrescriptionAsync(PrescriptionPostDto request, int doctorId);
    Task<PatientWithDetailsDto> GetPatientDetailsAsync(int patientId);

}

public class DbService : IDbService
{
    private readonly AppDbContext _context;

    public DbService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddPrescriptionAsync(PrescriptionPostDto request, int doctorId)
{
    if (request.Medicaments.Count > 10)
        throw new ArgumentException("Max 10 medicaments allowed");

    if (request.DueDate < request.Date)
        throw new ArgumentException("DueDate >= Date.");

    var patient = await _context.Patient.FindAsync(request.Patient.IdPatient);

    if (patient != null)
    {
        if (patient.FirstName != request.Patient.FirstName ||
            patient.LastName != request.Patient.LastName ||
            patient.Birthdate != request.Patient.Birthdate)
        {
            throw new ArgumentException("patient data not consistent with those in the database");
        }
    }
    else
    {
        patient = new Patient
        {
            FirstName = request.Patient.FirstName,
            LastName = request.Patient.LastName,
            Birthdate = request.Patient.Birthdate
        };
        _context.Patient.Add(patient);
        await _context.SaveChangesAsync();
    }

    var doctor = await _context.Doctor.FindAsync(doctorId);
    if (doctor == null)
        throw new NotFoundException("Doctor not found.");

    var allMedicamentIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
    var existingMedicaments = await _context.Medicament
        .Where(m => allMedicamentIds.Contains(m.IdMedicament))
        .Select(m => m.IdMedicament)
        .ToListAsync();

    var missing = allMedicamentIds.Except(existingMedicaments).ToList();
    if (missing.Any())
        throw new NotFoundException($"Medicament not found Id: {string.Join(",", missing)}");

    var prescription = new Prescription
    {
        Date = request.Date,
        DueDate = request.DueDate,
        IdPatient = patient.IdPatient,
        IdDoctor = doctorId
    };

    _context.Prescription.Add(prescription);
    await _context.SaveChangesAsync(); 

    foreach (var med in request.Medicaments)
    {
        _context.Prescription_Medicament.Add(new Prescription_Medicament
        {
            IdMedicament = med.IdMedicament,
            IdPrescription = prescription.IdPrescription,
            Dose = med.Dose,
            Details = med.Description
        });
    }

    await _context.SaveChangesAsync();

    return prescription.IdPrescription;
}

    public async Task<PatientWithDetailsDto> GetPatientDetailsAsync(int patientId)
    {
        var patient = await _context.Patient
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Prescription_Medicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
            throw new NotFoundException("Patient not found.");

        return new PatientWithDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.Prescription_Medicaments.Select(pm => new MedicamentDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }

}

