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
public class PrivateMessageServiceTests
{
  private readonly PrivateMessageService _sut;
  private readonly IRepository<PrivateMessage> _messageRepository = Substitute.For<IRepository<PrivateMessage>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private readonly ILoggerAdapter<PrivateMessageService> _logger = Substitute.For<ILoggerAdapter<PrivateMessageService>>();

  public PrivateMessageServiceTests()
  {
    _sut = new PrivateMessageService(_messageRepository, _userRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllMessages_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllMessages();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving all messages"));
  }

  [Fact]
  public async Task RetrieveAllMessages_ShouldReturnEmptyList_WhenNoMessagesExist()
  {
    // Arrange
    _messageRepository.ListAsync()
      .Returns(new List<PrivateMessage>());

    // Act
    var result = await _sut.RetrieveAllMessages();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllMessages_ShouldReturnMessages_WhenSomeMessagesExist()
  {
    // Arrange
    var expectedMessages = FakePrivateMessagesList();
    _messageRepository.ListAsync()
      .Returns(expectedMessages);

    // Act
    var result = await _sut.RetrieveAllMessages();

    // Assert
    result.Should().BeEquivalentTo(expectedMessages);
  }

  [Fact]
  public async Task RetrieveAllMessages_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _messageRepository.ListAsync()
      .Returns(FakePrivateMessagesList());

    // Act
    await _sut.RetrieveAllMessages();

    // Assert
    _logger.Received(1).LogInformation("Retrieving all messages");
    _logger.Received(1).LogInformation("All messages retrieved in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region RetrieveUserConversations
  [Fact]
  public async Task GetUserConversations_ShouldLogMessageAndException()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.GetUserConversations(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving user (id: {0}) conversations"), Arg.Is(1));
  }

  [Fact]
  public async Task GetUserConversations_ShouldReturnEmptyList_WhenNoConversationsExist()
  {
    // Arrange
    _messageRepository.ListAsync()
      .Returns(new List<PrivateMessage>());

    // Act
    var result = await _sut.GetUserConversations(1);

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task GetUserConversations_ShouldReturnConversations_WhenSomeConversationsExist()
  {
    // Arrange
    var expectedConversations = FakePrivateMessagesList();
    _messageRepository.ListAsync()
      .Returns(expectedConversations);

    // Act
    var result = await _sut.GetUserConversations(1);

    // Assert
    result.Should().BeEquivalentTo(expectedConversations);
  }

  [Fact]
  public async Task GetUserConversations_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _messageRepository.ListAsync()
      .Returns(FakePrivateMessagesList());

    // Act
    await _sut.GetUserConversations(1);

    // Assert
    _logger.Received(1).LogInformation("Retrieving user (id: {0}) conversations", Arg.Is(1));
    _logger.Received(1).LogInformation("User (id: {0}) conversations retrieved in {1}ms", Arg.Is(1), Arg.Any<long>());
  }
  #endregion

  #region Create
  [Fact]
  public async Task PostMessage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.AddAsync(Arg.Any<PrivateMessage>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.PostMessage(FakePrivateMessage());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while posting message"));
  }

  [Fact]
  public async Task PostMessage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var expectedMessage = FakePrivateMessage();
    _messageRepository.AddAsync(Arg.Any<PrivateMessage>())
      .Returns(expectedMessage);

    // Act
    var result = await _sut.PostMessage(expectedMessage);

    // Assert
    result.Should().BeEquivalentTo(expectedMessage);
  }

  [Fact]
  public async Task PostMessage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _messageRepository.AddAsync(Arg.Any<PrivateMessage>())
      .Returns(FakePrivateMessage());

    // Act
    await _sut.PostMessage(FakePrivateMessage());

    // Assert
    _logger.Received(1).LogInformation("Posting message");
    _logger.Received(1).LogInformation("Message posted in {0}ms", Arg.Any<long>());
  }
  #endregion

  #region Patch
  [Fact]
  public async Task SetMessageAsSeen_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.SetMessageAsSeen(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while setting message (id: {0}) as seen"), Arg.Is(1));
  }

  [Fact]
  public async Task SetMessageAsSeen_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var expectedMessage = FakePrivateMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Returns(expectedMessage);

    // Act
    var result = await _sut.SetMessageAsSeen(1);

    // Assert
    result.Should().BeEquivalentTo(expectedMessage);
  }

  [Fact]
  public async Task SetMessageAsSeen_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var messageToPatch = FakePrivateMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Returns(messageToPatch);

    // Act
    await _sut.SetMessageAsSeen(messageToPatch.Id);

    // Assert
    _logger.Received(1).LogInformation("Setting message (id: {0}) as seen", Arg.Is(messageToPatch.Id));
    _logger.Received(1).LogInformation("Message (id: {0}) set as seen in {1}ms", Arg.Is(messageToPatch.Id), Arg.Any<long>());
  }
  #endregion

  #region Update
  [Fact]
  public async Task EditMessage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.EditMessage(FakePrivateMessage());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while editing message (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task EditMessage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var expectedMessage = FakePrivateMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Returns(expectedMessage);

    // Act
    var result = await _sut.EditMessage(expectedMessage);

    // Assert
    result.Should().BeEquivalentTo(expectedMessage);
  }

  [Fact]
  public async Task EditMessage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var expectedMessage = FakePrivateMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Returns(expectedMessage);

    // Act
    await _sut.EditMessage(expectedMessage);

    // Assert
    _logger.Received(1).LogInformation("Editing message (id: {0})", Arg.Is(expectedMessage.Id));
    _logger.Received(1).LogInformation("Message (id: {0}) edited in {1}ms", Arg.Is(expectedMessage.Id), Arg.Any<long>());
  }
  #endregion

  #region Delete
  [Fact]
  public async Task DeleteMessage_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.DeleteMessage(1);

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Something went wrong while deleting message (id: {0})"), Arg.Is(1));
  }

  [Fact]
  public async Task DeleteMessage_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var expectedMessage = FakePrivateMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Returns(expectedMessage);

    // Act
    var result = await _sut.DeleteMessage(expectedMessage.Id);

    // Assert
    result.Should().BeEquivalentTo(expectedMessage);
  }

  [Fact]
  public async Task DeleteMessage_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var messageToDelete = FakePrivateMessage();
    _messageRepository.FirstOrDefaultAsync(Arg.Any<ISpecification<PrivateMessage>>())
      .Returns(messageToDelete);

    // Act
    await _sut.DeleteMessage(messageToDelete.Id);

    // Assert
    _logger.Received(1).LogInformation("Deleting message (id: {0})", Arg.Is(messageToDelete.Id));
    _logger.Received(1).LogInformation("Message (id: {0}) deleted in {1}ms", Arg.Is(messageToDelete.Id), Arg.Any<long>());
  }
  #endregion
}
