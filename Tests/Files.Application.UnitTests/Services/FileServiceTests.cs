using AutoMapper;
using Files.Application.Common.Exceptions;
using Files.Application.Extensions.Interfaces;
using Files.Application.Features.File;
using Files.Application.Extensions.Services;
using Files.Infrastructure.Persistence.RepositoryManagers;
using MassTransit;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using NightCloud.Common.QueueMessaging.File;
using Directory = Files.Domain.Entities.Directory.Directory;
using File = Files.Domain.Entities.File.File;
namespace Files.Application.UnitTests.Services;

public class FileServiceTests
{
    private readonly IFileService _service;
    private readonly Mock<IRepositoryManager> _repositoryManager;

    public FileServiceTests()
    {
        var mapper = new MapperConfiguration(x =>
        {
            x.AddProfile<MappingProfile>();
        }).CreateMapper();
        Mock<IPublishEndpoint> publishEndpoint = new();
        Mock<IRequestClient<RetrieveFile>> client = new();
        Mock<ILogger<FileService>> logger = new();
        _repositoryManager = new Mock<IRepositoryManager>();
        _service = new FileService(
            _repositoryManager.Object,mapper, publishEndpoint.Object, client.Object, logger.Object);
    }

    [Fact]
    public async Task GetFile_NotExistingFileId_ThrowsInvalidFileIdBadRequest()
    {
        _repositoryManager.Setup(x => x.DirectoryRepository.FindAll(It.IsAny<bool>()))
            .Returns(Enumerable.Empty<Directory>().AsQueryable().BuildMock());

        var func = async () => await _service.GetFileAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidDirectoryIdBadRequestException>(func);
    }
    [Fact]
    public async Task GetFile_ExistingFileId_ReturnsValidFile()
    {
        var userId = Guid.NewGuid();
        var directoryId = Guid.NewGuid();
        var fileId = Guid.NewGuid();
        var file = new File()
        {
            Id = fileId,
            UserId = userId,
            DirectoryId = directoryId
        };
        
        var directoriesList = new List<Directory>()
        {
            new()
            {
                Id = directoryId,
                UserId = userId,
                Files = new List<File>{file}
            }
        };


        _repositoryManager.Setup(x => x.DirectoryRepository.FindAll(It.IsAny<bool>()))
            .Returns(directoriesList.AsQueryable().BuildMock());

        var result = await _service.GetFileAsync(userId, directoryId, fileId);
        
        Assert.Equal(file.Id, result.Id);
    }
}