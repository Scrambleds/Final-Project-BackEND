using Final_Project_BackEND.Data;
using Final_Project_BackEND.Entity;
using Final_Project_BackEND.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Final_Project_BackEND.Service
{
    public class ImportService : IImportService
    {
        private readonly IImportRepository _importRepository;
        private readonly DataContext _context;

        public ImportService(IImportRepository importRepository, DataContext db)
        {
            _importRepository = importRepository;
            _context = db;
        }

        public async Task<ActionResult> CheckImportHeader(CheckImportHeader importHeader)
        {
            return await _importRepository.CheckImportHeader(importHeader);
        }

        public async Task<ActionResult> GetListExcelFromImportHeader(string importHeaderNumber)
        {
            return await _importRepository.GetListExcelFromImportHeader(importHeaderNumber);
        }

        public async Task<int> SaveScoreByImportAsync(List<Excel> excelList, ImportHeader ImportH, GradeImportHeader SumGrade)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!string.IsNullOrEmpty(ImportH.importHeaderNumber))
                    {
                        var checkByImportHeader = (from a in _context.importHeaders
                                                   where a.importHeaderNumber == ImportH.importHeaderNumber
                                                   select a).FirstOrDefault();

                        if (checkByImportHeader != null)
                        {
                            ImportH = checkByImportHeader;
                        }

                    }
                    else
                    {
                        var checkImportHeader = (from a in _context.importHeaders
                                                 where a.courseID == ImportH.courseID
                                                 where a.courseName == ImportH.courseName
                                                 where a.yearEducation == ImportH.yearEducation
                                                 where a.semester == ImportH.semester
                                                 select a).FirstOrDefault();
                        
                        if (checkImportHeader != null)
                        {
                            ImportH.importHeaderNumber = checkImportHeader.importHeaderNumber;
                            ImportH.importHeaderID = checkImportHeader.importHeaderID;
                        }
                    }

                    string datetime = DateTime.Now.ToString("fffffff").Substring(0, 4);
                    string invoiceNumber = $"{ImportH.courseID}-{ImportH.yearEducation}-{ImportH.semester}-{datetime}";



                    ImportH.importHeaderNumber = string.IsNullOrEmpty(ImportH.importHeaderNumber) ? invoiceNumber : ImportH.importHeaderNumber;
                    ImportH.importHeaderID = ImportH.importHeaderID != 0 ? ImportH.importHeaderID : 0;
                    ImportH.isenable = 1;
                    foreach (var item in excelList)
                    {
                        item.importHeaderNumber = ImportH.importHeaderNumber;
                    }
                    SumGrade.importHeaderNumber = ImportH.importHeaderNumber;
                    // Your data processing logic goes here     
                    var entity = await _importRepository.SaveScoreByImportAsync(excelList, ImportH, SumGrade);

                    await transaction.CommitAsync();
                    return entity;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
