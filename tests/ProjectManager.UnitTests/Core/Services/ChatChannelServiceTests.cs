using Ardalis.Specification;
using FluentAssertions;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Services;
using ProjectManager.SharedKernel.Interfaces;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;

namespace ProjectManager.UnitTests.Core.Services;
public class ChatChannelServiceTests
{
  private readonly ChatChannelService _sut;
  private readonly IRepository<ChatChannel> _chatChannelRepository = Substitute.For<IRepository<ChatChannel>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly ILoggerAdapter<ChatChannelService> _logger = Substitute.For<ILoggerAdapter<ChatChannelService>>();

  public ChatChannelServiceTests()
  {
    _sut = new ChatChannelService(_chatChannelRepository, _userRepository, _logger);
  }

  #region Create
  [Fact]
  public async Task CreateChatChannel_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _chatChannelRepository.AddAsync(Arg.Any<ChatChannel>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.CreateChatChannel(FakeChatChannel());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while creating channel"));
  }

  [Fact]
  public async Task CreateChatChannel_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var channelToCreate = FakeChatChannel();
    _chatChannelRepository.AddAsync(Arg.Any<ChatChannel>())
      .Returns(channelToCreate);

    // Act
    var result = await _sut.CreateChatChannel(channelToCreate);

    // Assert
    result.Should().Be(channelToCreate);
  }

  [Fact]
  public async Task CreateChatChannel_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var channelToCreate = FakeChatChannel();
    _chatChannelRepository.AddAsync(Arg.Any<ChatChannel>())
      .Returns(channelToCreate);

    // Act
    await _sut.CreateChatChannel(channelToCreate);

    // Assert
    _logger.Received(1).LogInformation("Creating chat channel");
    _logger.Received(1).LogInformation("Channel created in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Update
  [Fact]
  public async Task UpdateChatChannel_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _chatChannelRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ChatChannel>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.UpdateChatChannel(FakeChatChannel());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.LogError(Arg.Is(exception), Arg.Is("Something went wrong while updating channel (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task UpdateChatChannel_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _chatChannelRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ChatChannel>>())
      .Returns(FakeChatChannel());
    _userRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<User>>())
      .Returns(FakeUser());

    // Act
    var result = await _sut.UpdateChatChannel(FakeChatChannel());

    // Assert
    result.Should().BeEquivalentTo(FakeChatChannel());
  }

  [Fact]
  public async Task UpdateChatChannel_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _chatChannelRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ChatChannel>>())
      .Returns(FakeChatChannel());

    // Act
    await _sut.UpdateChatChannel(FakeChatChannel());

    // Assert
    _logger.Received(1).LogInformation("Updating channel (id: {0})", Arg.Is(1));
    _logger.Received(1).LogInformation("Channel (id: {0}) updated in {1}ms", Arg.Is(1), Arg.Any<long>());
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteChatChannel_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _chatChannelRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ChatChannel>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteChatChannel(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while deleting chat channel (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task DeleteChatChannel_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    _chatChannelRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ChatChannel>>())
      .Returns(FakeChatChannel());

    // Act
    var result = await _sut.DeleteChatChannel(1);

    // Assert
    result.Should().BeEquivalentTo(FakeChatChannel());
  }

  [Fact]
  public async Task DeleteChatChannel_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _chatChannelRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<ChatChannel>>())
      .Returns(FakeChatChannel());

    // Act
    await _sut.DeleteChatChannel(1);

    // Assert
    _logger.Received(1).LogInformation("Deleting chat channel (id: {0})", Arg.Is(1));
    _logger.Received(1).LogInformation("Chat channel (id: {0}) deleted in {1}ms", Arg.Is(1), Arg.Any<long>());
  }
  #endregion
}
