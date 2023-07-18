namespace SearchPhotoBot.Infrastructure;

public interface IDataStoreService<TModel>
{
    public Task StoreData(string storeName, TModel Data);
    public Task<TModel> LoadData(string  storeName);
}