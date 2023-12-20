using Application.Common.Interfaces;
using Domain.Entities.Projections;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.File.Commands;

public class UploadFileCommand : IRequest<UploadResult>
{
    public IFormFile File { get; private set; }

    public UploadFileCommand(IFormFile file)
    {
        File = file;
    }
}


public class UploadFilesCommandHandler : IRequestHandler<UploadFileCommand, UploadResult>
{
    private const long maxFileSize = 1024 * 1024 * 15;

    private readonly IProfilePictureService _profilePictureService;
    private readonly ILogger<UploadFilesCommandHandler> _logger;

    public UploadFilesCommandHandler(IProfilePictureService profilePictureService, ILogger<UploadFilesCommandHandler> logger)
    {
        _profilePictureService = profilePictureService;
        _logger = logger;
    }

    public async Task<UploadResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var uploadResult = new UploadResult
        {
            FileName = request.File.FileName
        };

        if (request.File.Length == 0)
        {
            _logger.LogInformation("{FileName} length is 0 (Err: 1)",
                request.File.FileName);
        }
        else if (request.File.Length > maxFileSize)
        {
            _logger.LogInformation("{FileName} of {Length} bytes is " +
                "larger than the limit of {Limit} bytes (Err: 2)",
                request.File.FileName, request.File.Length, maxFileSize);
        }
        else
        {
            try
            {
                var trustedFileNameForFileStorage = (Guid.NewGuid()).ToString() + ".jpg"; //May not actually be a jpeg, fix later

                await using (var ms = new MemoryStream())
                {
                    await request.File.CopyToAsync(ms, cancellationToken);
                    await _profilePictureService.WriteImageFile(trustedFileNameForFileStorage, ms);
                }

                _logger.LogInformation("{FileName} saved",
                    request.File.FileName);
                uploadResult.Uploaded = true;
                uploadResult.StoredFileName = trustedFileNameForFileStorage;
            }
            catch (IOException ex)
            {
                _logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                    request.File.FileName, ex.Message);
                uploadResult.ErrorCode = 3;
            }
        }

        return uploadResult;
    }
}