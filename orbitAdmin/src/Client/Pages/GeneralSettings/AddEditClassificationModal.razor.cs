using SchoolV01.Shared.ViewModels.Settings;
using SchoolV01.Client.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Classifications.Commands;

namespace SchoolV01.Client.Pages.GeneralSettings
{
    public partial class AddEditClassificationModal
    {
        [Parameter]public int Id { get; set; } 
        [Parameter]public AddEditClassificationCommand ClassificationModel { get; set; } = new();

        [CascadingParameter]
        private IMudDialogInstance  MudDialog { get; set; }
        private bool _processing = false;

        public void Cancel()
        {
            MudDialog.Cancel();

        }

        private async Task SaveAsync()
        {
            _processing = true;
            var content = HelperMethods.ToJson(ClassificationModel);
            HttpResponseMessage response;
            if (ClassificationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.Classifications, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.Classifications}/{ClassificationModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            _processing = false;
        }

    }
}
