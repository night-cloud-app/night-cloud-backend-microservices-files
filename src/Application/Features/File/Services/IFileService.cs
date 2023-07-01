﻿using Files.Application.Features.File.DataTransferObjects;

namespace Files.Application.Features.File.Services;
public interface IFileService
{
    Task<FileDto> GetFileAsync(Guid userId, Guid fileId, CancellationToken cancellationToken = default);
    Task<ICollection<FileDto>> GetFilesAsync(Guid userId, bool trackChanges = false, CancellationToken cancellationToken = default);
    Task UpdateFileAsync(Guid userId, UpdateFileDto dto, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(Guid userId, Guid fileId, CancellationToken cancellationToken = default);
    Task<FileDto> CreateFileAsync(CreateFileDto dto, CancellationToken cancellationToken = default);
    Task UploadFileAsync(FormFileDto dto, Guid userId, CancellationToken cancellationToken = default);
    Task<DownloadFileDto> DownloadFileAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}