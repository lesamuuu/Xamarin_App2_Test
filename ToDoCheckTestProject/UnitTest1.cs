using toDoCheck.Services;
using toDoCheck.Models;
using toDoCheck.Repositories;

namespace ToDoCheckTestProject;

public class Tests
{
    private ToDoItemDBService<Foo> _DbService;

    [SetUp]
    public void Setup()
    {
        var inMemoryRepository = new InMemoryRepository<Foo>();
        _DbService = new ToDoItemDBService<Foo>(inMemoryRepository);
    }


    [Test]
    public async Task InsertItemAsync_ShouldInsertItem()
    {
        var item = new Foo { Name = "Insert test" };
        var insertResult = await _DbService.InsertItemAsync(item);

        var retrievedItem = await _DbService.GetItemsAsync();
        Assert.That(retrievedItem.Count, Is.EqualTo(1));
        Assert.That(retrievedItem[0].Name, Is.EqualTo("Insert test"));
    }

    [Test]
    public async Task DeleteItemAsync_ShouldDeleteItem()
    {
        var item = new Foo { Name = "Delte test" };
        var insertResult = await _DbService.InsertItemAsync(item);
        var deleteResult = await _DbService.DeleteItemAsync(item);

        var retrievedItems = await _DbService.GetItemsAsync();
        Assert.That(retrievedItems.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateItemAsync_ShouldUpdateItem()
    {
        var item = new Foo { Name = "Modify test" };
        var insertResult = await _DbService.InsertItemAsync(item);

        item.Name = "Modified test";
        var modifyResult = await _DbService.UpdateItemAsync(item);

        var retrievedItems = await _DbService.GetItemsAsync();
        Assert.That(retrievedItems[0].Name, Is.EqualTo("Modified test"));
    }

    [Test]
    public async Task GetAllAsync_ShouldUpdateItem()
    {
        for (int i = 0; i <= 2; i++)
        {
            var item = new Foo { Name = "GetAll test " + i };
            var insertResult = await _DbService.InsertItemAsync(item);
        }
        
        var retrievedItems = await _DbService.GetItemsAsync();
        Assert.That(retrievedItems.Count, Is.EqualTo(3));

        for (int i = 0; i <= 2; i++)
        {
            Assert.That(retrievedItems[i].Name, Is.EqualTo("GetAll test " + i));
        }
    }


    [TearDown]
    public void TearDown()
    {
        _DbService.Clear();
    }
}