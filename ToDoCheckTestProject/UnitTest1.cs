using toDoCheck.Services;
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
        const int numItems = 3;

        for (int i = 1; i <= numItems; i++)
        {
            var item = new Foo { Name = "GetAll test " + i };
            var insertResult = await _DbService.InsertItemAsync(item);
        }
        
        var retrievedItems = await _DbService.GetItemsAsync();
        Assert.That(retrievedItems.Count, Is.EqualTo(numItems));

        for (int i = 1; i <= numItems; i++)
        {
            Assert.That(retrievedItems[i-1].Name, Is.EqualTo("GetAll test " + i));
        }
    }

    [Test]
    public async Task Search_ShouldReturnAll()
    {
        const int numItems = 3;
        for (int i = 1; i <= numItems; i++)
        {
            var item = new Foo { Name = "Item test " + i };
            var insertResult = await _DbService.InsertItemAsync(item);
        }

        List<Foo> retrievedList = await _DbService.Search("");

        Assert.That(retrievedList.Count() == numItems);
    }

    [Test]
    public async Task Search_ShouldReturnOneItem()
    {
        const int numItems = 3;
        for (int i = 1; i <= numItems; i++)
        {
            var item = new Foo { Name = "Item test " + i };
            var insertResult = await _DbService.InsertItemAsync(item);
        }
        

        List<Foo> retrievedList = await _DbService.Search("1");

        Assert.That(retrievedList.Count() == 1);
    }

    [Test]
    public async Task Search_ShouldReturnThreeItems()
    {
        const int numItems = 3;
        for (int i = 1; i <= numItems; i++)
        {
            var item = new Foo { Name = "Item test " + i };
            var insertResult = await _DbService.InsertItemAsync(item);
        }


        List<Foo> retrievedList = await _DbService.Search("Item");

        Assert.That(retrievedList.Count() == numItems);
    }

    [TearDown]
    public void TearDown()
    {
        _DbService.Clear();
    }
}