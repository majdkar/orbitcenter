﻿using System;
using System.Collections.Generic;
using SchoolV01.Application.Features.Documents.Commands;
using SchoolV01.Application.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using SchoolV01.Application.Features.DocumentTypes.Queries;
using SchoolV01.Client.Infrastructure.Managers.Misc.Document;
using SchoolV01.Client.Infrastructure.Managers.Misc.DocumentType;

namespace SchoolV01.Client.Pages.Misc
{
    public partial class AddEditDocumentModal
    {
        [Inject] private IDocumentManager DocumentManager { get; set; }
        [Inject] private IDocumentTypeManager DocumentTypeManager { get; set; }

        [Parameter] public AddEditDocumentCommand AddEditDocumentModel { get; set; } = new();
        [CascadingParameter] private IMudDialogInstance  MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool disable = false; 
        private List<GetAllDocumentTypesResponse> _documentTypes = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            disable=true;
            var response = await DocumentManager.SaveAsync(AddEditDocumentModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            disable = false;

        }

        protected override async Task OnInitializedAsync()
        {
          //  await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadDocumentTypesAsync();
        }

        private async Task LoadDocumentTypesAsync()
        {
            var data = await DocumentTypeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _documentTypes = data.Data;
            }
        }

        private IBrowserFile _file;

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var buffer = new byte[_file.Size];
                var extension = Path.GetExtension(_file.Name);
                var format = "application/octet-stream";
                await _file.OpenReadStream(_file.Size).ReadAsync(buffer);
                AddEditDocumentModel.URL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditDocumentModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Document, Extension = extension };
            }
        }

        private async Task<IEnumerable<int>> SearchDocumentTypes(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _documentTypes.Select(x => x.Id);

            return _documentTypes.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}