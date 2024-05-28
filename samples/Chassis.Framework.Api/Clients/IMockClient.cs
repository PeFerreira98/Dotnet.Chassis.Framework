using PeFerreira98.Chassis.Framework.Http;
using Refit;

namespace PeFerreira98.Chassis.Framework.Api.Clients;

public interface IMockClient : IRefitClient, IDisposable
{
    [Get("/")]
    Task<IApiResponse<ResponseDto>> GetAsync();
}

public class ResponseDto
{
    public string Result { get; set; } = string.Empty;
}
