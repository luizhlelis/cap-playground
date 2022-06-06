using Microsoft.EntityFrameworkCore;

namespace Catalog.Consumer.Idempotency;

public interface IStorageHelper
{
    bool IsMessageExistsError(DbUpdateException ex);
}
