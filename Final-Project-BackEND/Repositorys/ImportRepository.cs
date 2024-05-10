using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Final_Project_BackEND.Repositorys
{
    public class ImportRepository : IImportRepository
    {
        private readonly DataContext _context;
        private readonly TimezoneConverter _timezoneConverter;


        public ImportRepository(DataContext db, TimezoneConverter timezoneConverter)
        {
            _context = db;
            _timezoneConverter = timezoneConverter;
        }

        public async Task<ActionResult> CheckImportHeader(CheckImportHeader importHeader)
        {
            try
            {
                var ImportHeaderNumberInDB = await (from a in _context.importHeaders
                                                    where a.courseID == importHeader.courseID
                                                    where a.courseName == importHeader.courseName   
                                                    where a.semester == importHeader.semester
                                                    where a.yearEducation == importHeader.yearEducation
                                                    where a.isenable == 1
                                                    select new
                                                    {
                                                        a.importHeaderNumber
                                                    }).FirstOrDefaultAsync();

                if (ImportHeaderNumberInDB == null)
                {
                    return new OkObjectResult(new { status = 0 });
                }
                else
                {
                    return new OkObjectResult(new { ImportHeaderNumberInDB.importHeaderNumber, status = 1 });
                }
            }
            catch (Exception ex)
            {
                 return new BadRequestObjectResult(new { status = -1 });
            }
        }

        public async Task<ActionResult> GetListExcelFromImportHeader(string importHeaderNumber)
        {
            try
            {
                var ImportHeaderNumberInDB = await (from a in _context.importHeaders
                                                    where a.importHeaderNumber == importHeaderNumber
                                                    where a.isenable == 1
                                                    select a).FirstOrDefaultAsync();

                if (ImportHeaderNumberInDB != null)
                {
                    var listExcel = await (from a in _context.Excels
                                           where a.importHeaderNumber == importHeaderNumber
                                           select new
                                           {
                                               a.no,
                                               a.id,
                                               a.name,
                                               a.grade
                                           }).ToListAsync();

                    if (listExcel != null)
                    {
                        return new OkObjectResult(new { excel = listExcel, status = 1 });
                    }
                    else
                    {
                        return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่มีข้อมูลexcelของรายการนี้" });
                    }
                }
                else
                {
                    return new OkObjectResult(new { status = 0 });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถทำรายการนี้" });
            }
        }

        //public string GenerateInvoiceNumber()
        //{
        //    // You can customize the prefix and format as needed
        //    string prefix = "IN";
        //    string dateFormat = "yyyyMMdd";

        //    // Combine prefix, date, and a unique identifier (in this case, a random number)
        //    string invoiceNumber = $"{prefix}-{DateTime.Now.ToString(dateFormat)}-{GetUniqueIdentifier()}";

        //    // Check if the generated invoice number already exists in the database
        //    if (IsInvoiceNumberExistsInDatabase(invoiceNumber))
        //    {
        //        // If it exists, generate a new one recursively
        //        return GenerateInvoiceNumber();
        //    }

        //    return invoiceNumber;
        //}

        //private int GetUniqueIdentifier()
        //{
        //    // Generate a random number or use a more sophisticated method for uniqueness
        //    Random random = new Random();
        //    return random.Next(1000, 9999);
        //}
        //private bool IsInvoiceNumberExistsInDatabase(string invoiceNumber)
        //{
        //    // Use Entity Framework to check if the invoice number exists in the database
        //    return _context.importHeaders.Any(header => header.importHeaderNumber == invoiceNumber);
        //}


        public async Task<int> SaveScoreByImportAsync(List<Excel> excelList, ImportHeader ImportH, GradeImportHeader SumGrade)
        {
            try
            {
                if (ImportH != null)
                {
                    await AddImportH(ImportH);
                }
                if (excelList != null)
                {
                    await AddExcel(excelList);
                }
                if (SumGrade != null)
                {
                    await AddGrade(SumGrade);
                }


                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<ActionResult> AddImportH(ImportHeader ImportH)
        {
            try
            {
                var checkForUpdateData = (from a in _context.importHeaders
                                          where a.importHeaderNumber == ImportH.importHeaderNumber
                                          select a).FirstOrDefault();

                if (checkForUpdateData != null)
                {
                    //_context.Excels.Update(ImportH); <= อีกวิธีเลือกใช้เอา  
                    ImportH.dateUpdated = DateTime.Now;
                    _context.Entry(checkForUpdateData).CurrentValues.SetValues(ImportH);
                }
                else
                {
                    await _context.importHeaders.AddAsync(ImportH);
                }

                await _context.SaveChangesAsync();

                return new OkObjectResult(new { ImportH = ImportH });
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
            }

        }

        public async Task<ActionResult> AddExcel(List<Excel> newExcel)
        {
            try
            {
                var importHeaderNumbers = newExcel.Select(e => e.importHeaderNumber).ToList();

                // Find existing records in the database with matching importHeaderNumbers
                var existingData = await _context.Excels
                    .Where(e => importHeaderNumbers.Contains(e.importHeaderNumber))
                    .ToListAsync();

                _context.RemoveRange(existingData);

                await _context.Excels.AddRangeAsync(newExcel);

                await _context.SaveChangesAsync();

                return new OkObjectResult(new { excel = newExcel });
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
            }
        }

        public async Task<ActionResult> AddGrade(GradeImportHeader newSumGrade)
        {
            try
            {
                var checkImportHeaderNumber =  (from a in _context.GradeImportHeaders
                                                where a.importHeaderNumber == newSumGrade.importHeaderNumber
                                                     select a).FirstOrDefault();
             
                if (checkImportHeaderNumber != null)
                {
                    newSumGrade.dateUpdated = _timezoneConverter.ConvertToDefaultTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc); ;
                    checkImportHeaderNumber.A = newSumGrade.A;
                    checkImportHeaderNumber.Bplus = newSumGrade.Bplus;
                    checkImportHeaderNumber.B = newSumGrade.B;
                    checkImportHeaderNumber.Cplus = newSumGrade.Cplus;
                    checkImportHeaderNumber.C = newSumGrade.C;
                    checkImportHeaderNumber.Dplus = newSumGrade.Dplus;
                    checkImportHeaderNumber.D = newSumGrade.D;
                    checkImportHeaderNumber.F = newSumGrade.F;
                    checkImportHeaderNumber.W = newSumGrade.W;
                    checkImportHeaderNumber.I = newSumGrade.I;
                    checkImportHeaderNumber.Total = newSumGrade.Total;
                }
                else
                {
                    await _context.GradeImportHeaders.AddAsync(newSumGrade);
                }

                await _context.SaveChangesAsync();


                return new OkObjectResult(new { sumGrade = newSumGrade });
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BadRequestObjectResult(new { error = "เกิดข้อผิดพลาด ไม่สามารถบันทึกลงในระบบ" });
            }
        }

    }
}
