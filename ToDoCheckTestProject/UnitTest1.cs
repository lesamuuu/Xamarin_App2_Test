using NUnit.Framework;
using toDoCheck.Services;
using toDoCheck.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToDoCheckTestProject;

public class Tests
{
    private IToDoItemDB _toDoItemDBService;
    private string _dbName;

    [SetUp]
    public void Setup()
    {
        // Generates a new dbName for each test
        _dbName = $"Test_{Guid.NewGuid()}.db3";
        _toDoItemDBService = new ToDoItemDBService(_dbName, true);
    }


    [Test]
    public async Task InsertItemAsync_ShouldInsertItem()
    {
        var item = new ToDoItem { ToDoTask = "Test Task" };
        var insertResult = await _toDoItemDBService.InsertItemAsync(item);

        var retrievedItem = await _toDoItemDBService.GetItemsAsync();
        Assert.That(retrievedItem.Count, Is.EqualTo(1));
        Assert.That(retrievedItem[0].ToDoTask, Is.EqualTo("Test Task"));
    }

    [Test]
    public async Task DeleteItemAsync_ShouldDeleteItem()
    {
        var item = new ToDoItem { ToDoTask = "Test Task" };
        var insertResult = await _toDoItemDBService.InsertItemAsync(item);
        var deleteResult = await _toDoItemDBService.DeleteItemAsync(item);

        var retrievedItems = await _toDoItemDBService.GetItemsAsync();
        Assert.That(retrievedItems.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task ModifyItemAsync_ShouldUpdateItem()
    {
        var item = new ToDoItem { ToDoTask = "Test Task" };
        var insertResult = await _toDoItemDBService.InsertItemAsync(item);

        item.ToDoTask = "Modified Task";
        var modifyResult = await _toDoItemDBService.ModifyItemAsync(item);

        var retrievedItems = await _toDoItemDBService.GetItemsAsync();
        Assert.That(retrievedItems[0].ToDoTask, Is.EqualTo("Modified Task"));
    }

    [Test]
    public async Task InsertOrUpdateItemAsync_ShouldInsertNewItem()
    {
        var item = new ToDoItem { ToDoTask = "Test Task" };
        var insertUpdateResult = await _toDoItemDBService.InsertOrUpdateItemAsync(item);

        var retrievedItems = await _toDoItemDBService.GetItemsAsync();
        Assert.That(retrievedItems.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task InsertOrUpdateItemAsync_ShouldUpdateExistingItem()
    {
        var item = new ToDoItem { ToDoTask = "Test Task" };
        var insertResult = await _toDoItemDBService.InsertItemAsync(item);

        item.ToDoTask = "Modified Task";
        var insertUpdateResult = await _toDoItemDBService.InsertOrUpdateItemAsync(item);

        var retrievedItems = await _toDoItemDBService.GetItemsAsync();
        Assert.That(retrievedItems[0].ToDoTask, Is.EqualTo("Modified Task"));
    }

    [TearDown]
    public void TearDown()
    {
        var testDbPath = System.IO.Path.Combine(Environment.CurrentDirectory, _dbName); 

        if (System.IO.File.Exists(testDbPath))
        {
            System.IO.File.Delete(testDbPath);
        }
    }
}