using MiniEshop.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MiniEshop.Services
{
    /// <summary>
    /// Сервис по работе с файлами
    /// Если представить, что этот сервис будет сохранять не только Image,
    /// а какие-то другие еще типы файлов(pdf, word и e.g), которые будут сохраняться в отдельной "статической" подпапке
    /// тогда следует в сервис воткнуть медиатор(MediatorR), который будет разруливать,
    /// что куда класть,
    /// и методы станут обобщенными, то есть вместо UploadImageAsync будет абстрактный метод
    /// UploadAsync, а параметром будет идти уже сам объект запроса, и медиатор будет разруливать, как этот запрос обработать.
    /// Для тестового задания это упрощено!!!!
    /// </summary>
    public interface IFileService
    {
        Task<FileLink> UploadImageAsync(Stream stream, string sourceFileName);

        Task<string> GetImagePathAsync(Guid id);

        Task<string> DeleteImageAsync(Guid id);
    }
}
