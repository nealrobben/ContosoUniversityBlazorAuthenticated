using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebUI.Shared;

namespace Application.File.Commands;

public class UploadFilesCommand : IRequest<IList<UploadResult>>
{
    public IEnumerable<IFormFile> Files { get; private set; }

    public UploadFilesCommand(IEnumerable<IFormFile> files)
    {
        Files = files;
    }
}


public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, IList<UploadResult>>
{
    private const int maxAllowedFiles = 3;
    private const long maxFileSize = 1024 * 1024 * 15;

    private readonly IProfilePictureService _profilePictureService;
    private readonly ILogger<UploadFilesCommandHandler> _logger;

    public UploadFilesCommandHandler(IProfilePictureService profilePictureService, ILogger<UploadFilesCommandHandler> logger)
    {
        _profilePictureService = profilePictureService;
        _logger = logger;
    }

    public async Task<IList<UploadResult>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        var filesProcessed = 0;
        List<UploadResult> uploadResults = [];

        foreach (var file in request.Files)
        {
            var uploadResult = new UploadResult
            {
                FileName = file.FileName
            };

            if (filesProcessed < maxAllowedFiles)
            {
                if (file.Length == 0)
                {
                    _logger.LogInformation("{FileName} length is 0 (Err: 1)",
                        file.FileName);
                }
                else if (file.Length > maxFileSize)
                {
                    _logger.LogInformation("{FileName} of {Length} bytes is " +
                        "larger than the limit of {Limit} bytes (Err: 2)",
                        file.FileName, file.Length, maxFileSize);
                }
                else
                {
                    try
                    {
                        var trustedFileNameForFileStorage = (Guid.NewGuid()).ToString() + ".jpg"; //May not actually be a jpeg, fix later

                        await using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            await _profilePictureService.WriteImageFile(trustedFileNameForFileStorage, ms);
                        }

                        _logger.LogInformation("{FileName} saved",
                            file.FileName);
                        uploadResult.Uploaded = true;
                        uploadResult.StoredFileName = trustedFileNameForFileStorage;
                    }
                    catch (IOException ex)
                    {
                        _logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                            file.FileName, ex.Message);
                        uploadResult.ErrorCode = 3;
                    }
                }

                filesProcessed++;
            }
            else
            {
                _logger.LogInformation("{FileName} not uploaded because the " +
                    "request exceeded the allowed {Count} of files (Err: 4)",
                    file.FileName, maxAllowedFiles);
                uploadResult.ErrorCode = 4;
            }

            uploadResults.Add(uploadResult);
        }

        return uploadResults;
    }
}