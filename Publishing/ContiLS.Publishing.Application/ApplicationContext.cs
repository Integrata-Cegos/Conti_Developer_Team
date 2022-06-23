using ContiLS.IsbnGenerator.API;
using ContiLS.IsbnGenerator.Impl;
using ContiLS.Store.API;
using ContiLS.Store.Impl;
using ContiLS.Books.API;
using ContiLS.Books.Impl;

public static class ApplicationContext{
    static ApplicationContext(){
        _isbnService = new RandomIsbnService();
        _storeService = new StoreService();
        _storeService.SetStock("books", new Isbn(4,5,6,7), 100);
        _storeService.SetStock("books", new Isbn(4,5,6,8), 10);
        _booksService = new BooksService(_isbnService, _storeService);
    }
    private static IIsbnService _isbnService;
    public static IIsbnService IIsbnService(){
        return _isbnService;
    }

    private static IStoreService _storeService;

    public static IStoreService IStoreService(){
        return _storeService;
    }  
    private static IBooksService _booksService;

    public static IBooksService IBooksService(){
        return _booksService;
    }  

}