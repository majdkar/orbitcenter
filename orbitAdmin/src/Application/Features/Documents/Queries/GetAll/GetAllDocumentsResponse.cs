﻿using System;

namespace SchoolV01.Application.Features.Documents.Queries
{
    public class GetAllDocumentsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Number { get; set; }
        public DateTime? Date { get; set; }
        public string User { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string URL { get; set; }
        public string DocumentType { get; set; }
        public int? DocumentTypeId { get; set; }
    }
}